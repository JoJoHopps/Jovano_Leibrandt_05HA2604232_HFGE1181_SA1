using UnityEngine;

[System.Serializable]
public class EnemyData
{
    public bool activeInScene;
    public float health;
    public float[] position;

    public EnemyData(HealthHandler enemyHealthHandler)
    {
        activeInScene = enemyHealthHandler.gameObject.activeInHierarchy;
        health = enemyHealthHandler.GetHealth();
        position = new float[3];
        position[0] = enemyHealthHandler.transform.position.x;
        position[1] = enemyHealthHandler.transform.position.y;
        position[2] = enemyHealthHandler.transform.position.z;
    }
}
