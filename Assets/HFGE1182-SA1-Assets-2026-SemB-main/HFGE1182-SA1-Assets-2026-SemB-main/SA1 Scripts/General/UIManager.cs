using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class UIManager : MonoBehaviour
{
    public GameObject hud;
    public GameObject pauseMenu;

    public TextMeshProUGUI txtAmmo;
    public ShootHandler shootHandler;

    public Slider sldHealth;
    public HealthHandler healthHandler;
    
    public static UIManager Instance { get; private set; }

    public bool IsPaused { get; private set; }

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

    private void Start()
    {
        ResumeGame();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (IsPaused)
                ResumeGame();
            else
                PauseGame();
        }
        UpdateHealthUI();
        UpdateAmmoUI();
    }
    
    public void UpdateHealthUI()
    {
        if (healthHandler == null || sldHealth == null)
            return;

        sldHealth.maxValue = healthHandler.GetMaxHealth();
        sldHealth.value = healthHandler.GetHealth();
    }

    private void UpdateAmmoUI()
    {
        if (shootHandler == null || txtAmmo == null)
            return;

        txtAmmo.text =
            $"{shootHandler.GetCurrentAmmo()} / {shootHandler.GetTotalAmmo()}";
    }

    public void PauseGame()
    {
        IsPaused = true;

        Time.timeScale = 0f;

        if (pauseMenu != null)
            pauseMenu.SetActive(true);

        if (hud != null)
            hud.SetActive(false);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ResumeGame()
    {
        IsPaused = false;

        Time.timeScale = 1f;

        if (pauseMenu != null)
            pauseMenu.SetActive(false);

        if (hud != null)
            hud.SetActive(true);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void QuitGame()
    {
      
        Application.Quit();

#if UNITY_EDITOR
        
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}

