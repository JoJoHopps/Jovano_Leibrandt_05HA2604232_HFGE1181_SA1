using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class SaveFlowController : MonoBehaviour
{
    private enum StartMode { NewGame, LoadSave, CarryOver }
    private static StartMode nextMode = StartMode.NewGame;

    [Header("Scene names (must match the Build Profiles scene list)")]
    public string firstLevelName = "Level1";
    public string creditsSceneName = "Credits";

    [Header("Tick this ONLY in MainMenu and Credits")]
    public bool isMenuScene;

    [Header("Menu only")]
    public GameObject noSaveMessage;

    [Header("Level scenes only")]
    public Transform playerSpawn;
    public WinCondition winCondition;
    public ShootHandler shootHandler;
    public GameObject gameOverPanel;
    public float gameOverDelay = 2f;

    private bool transitionHandled;
    private bool gameOverStarted;

    private IEnumerator Start()
    {
        Time.timeScale = 1f;

        if (isMenuScene)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            if (noSaveMessage != null) noSaveMessage.SetActive(false);
            yield break;
        }

        if (gameOverPanel != null) gameOverPanel.SetActive(false);

        // Wait one frame so every Start() has run (HealthHandler.Start resets health to max)
        yield return null;

        ApplyStartMode();
        nextMode = StartMode.NewGame;
        SaveEverything();
    }

    private void Update()
    {
        if (isMenuScene) return;

        // Stops clicks on pause buttons from firing arrows
        if (shootHandler != null && UIManager.Instance != null)
            shootHandler.enabled = !UIManager.Instance.IsPaused;

        CheckGameOver();
        CheckLevelComplete();
    }

    // ---------- Level logic ----------
    private void ApplyStartMode()
    {
        GameManager gm = GameManager.Instance;
        if (gm == null || gm.playerObject == null || nextMode == StartMode.NewGame) return;

        try
        {
            if (SaveSystem.LoadPlayer() == null) return;

            gm.LoadPlayer();

            if (nextMode == StartMode.LoadSave)
            {
                if (SaveSystem.LoadEnemy() != null)
                {
                    gm.LoadEnemies();
                    foreach (GameObject e in gm.enemies)
                    {
                        if (e == null || !e.activeInHierarchy) continue;
                        NavMeshAgent agent = e.GetComponent<NavMeshAgent>();
                        if (agent != null && agent.enabled) agent.Warp(e.transform.position);
                    }
                }
            }
            else if (playerSpawn != null) // CarryOver: keep health/ammo, but start at this level's spawn
            {
                gm.playerObject.transform.position = playerSpawn.position;
                gm.playerObject.transform.rotation = playerSpawn.rotation;
            }

            Rigidbody rb = gm.playerObject.GetComponent<Rigidbody>();
            if (rb != null) rb.linearVelocity = Vector3.zero;
        }
        catch (Exception e)
        {
            Debug.LogWarning("Load failed: " + e.Message);
        }
    }

    private void SaveEverything()
    {
        GameManager gm = GameManager.Instance;
        if (gm == null || gm.playerObject == null) return;
        try { gm.SavePlayer(); gm.SaveEnemies(); }
        catch (Exception e) { Debug.LogWarning("Save failed: " + e.Message); }
    }

    private void CheckLevelComplete()
    {
        if (transitionHandled || winCondition == null || winCondition.sceneEnemy != null) return;
        transitionHandled = true;

        if (winCondition.sceneToLoadName == creditsSceneName)
        {
            DeleteSaves();                 // game finished: no save remains
            nextMode = StartMode.NewGame;
        }
        else
        {
            try { GameManager.Instance.SavePlayer(); }
            catch (Exception e) { Debug.LogWarning("Save failed: " + e.Message); }
            nextMode = StartMode.CarryOver;
        }
    }

    private void CheckGameOver()
    {
        if (gameOverStarted || transitionHandled || GameManager.Instance == null) return;
        GameObject p = GameManager.Instance.playerObject;
        if (p == null || !p.activeInHierarchy)
        {
            gameOverStarted = true;
            StartCoroutine(GameOverRoutine());
        }
    }

    private IEnumerator GameOverRoutine()
    {
        if (gameOverPanel != null) gameOverPanel.SetActive(true);
        yield return new WaitForSeconds(gameOverDelay);
        nextMode = StartMode.NewGame;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // ---------- Menu buttons ----------
    public void NewGame()
    {
        DeleteSaves();
        nextMode = StartMode.NewGame;
        SceneManager.LoadScene(firstLevelName);
    }

    public void LoadGame()
    {
        PlayerData data = null;
        try { data = SaveSystem.LoadPlayer(); }
        catch (Exception e) { Debug.LogWarning("Could not read save: " + e.Message); }

        if (data == null || string.IsNullOrEmpty(data.sceneName))
        {
            if (noSaveMessage != null) noSaveMessage.SetActive(true);
            return;
        }

        nextMode = StartMode.LoadSave;
        SceneManager.LoadScene(data.sceneName);
    }

    public void QuitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    private static void DeleteSaves()
    {
        string[] files = { "PlayerData.dat", "EnemyData.dat" };
        foreach (string f in files)
        {
            string p = Path.Combine(Application.persistentDataPath, f);
            if (File.Exists(p)) File.Delete(p);
        }
    }
}