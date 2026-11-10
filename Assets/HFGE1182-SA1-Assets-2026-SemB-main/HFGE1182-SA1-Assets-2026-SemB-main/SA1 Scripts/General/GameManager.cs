using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameObject playerObject;
    public GameObject[] enemies;
    private string sceneName;

    private void Awake()
    {
        // Singleton pattern (optional but useful)
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void SavePlayer()
    {
        SaveSystem.SavePlayer(playerObject.GetComponent<ShootHandler>(), playerObject.GetComponent<HealthHandler>());
    }

    public void SaveEnemies()
    {
        
        SaveSystem.SaveEnemy(enemies);
        
    }

    public void LoadPlayer()
    {
        PlayerData data = SaveSystem.LoadPlayer();
        playerObject.GetComponent<ShootHandler>().SetCurrentAmmo(data.currentAmmo);
        playerObject.GetComponent<ShootHandler>().SetTotalAmmo(data.totalAmmo);
        playerObject.GetComponent<HealthHandler>().SetHealth(data.health);
        playerObject.transform.position = new Vector3(data.position[0], data.position[1], data.position[2]);
    }

    public void LoadEnemies()
    {
        int i = 0;
        foreach (var enemy in enemies)
        {
            EnemySaveData data = SaveSystem.LoadEnemy();
            enemy.SetActive(data.enemies[i].activeInScene);
            enemy.GetComponent<HealthHandler>().SetHealth(data.enemies[i].health);
            enemy.transform.position = new Vector3(data.enemies[i].position[0], data.enemies[i].position[1], data.enemies[i].position[2]);
            i++;
        }
    }
    
}
