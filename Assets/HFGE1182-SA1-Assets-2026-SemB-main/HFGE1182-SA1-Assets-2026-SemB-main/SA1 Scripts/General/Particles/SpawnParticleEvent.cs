using UnityEngine;

public class SpawnParticleEvent : MonoBehaviour
{
    public float duration;
    public GameObject particlePrefab;
    public Transform spawnPoint;

    public void SpawnParticle()
    {
        GameObject particle = Instantiate(particlePrefab, spawnPoint.position, Quaternion.identity);
        Destroy(particle,duration);
    }
}
