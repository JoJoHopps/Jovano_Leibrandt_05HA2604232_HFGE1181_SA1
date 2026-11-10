using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class PlayerData
{
    public int currentAmmo;
    public int totalAmmo;
    public float health;
    public float[] position;
    public string sceneName;

    public PlayerData(ShootHandler playerShootHandler, HealthHandler playerHealthHandler)
    {
        currentAmmo = playerShootHandler.GetCurrentAmmo();
        totalAmmo = playerShootHandler.GetTotalAmmo();
        health = playerHealthHandler.GetHealth();
        position = new float[3];
        position[0] = playerShootHandler.transform.position.x;
        position[1] = playerShootHandler.transform.position.y;
        position[2] = playerShootHandler.transform.position.z;
        sceneName = SceneManager.GetActiveScene().name;
    }

}
