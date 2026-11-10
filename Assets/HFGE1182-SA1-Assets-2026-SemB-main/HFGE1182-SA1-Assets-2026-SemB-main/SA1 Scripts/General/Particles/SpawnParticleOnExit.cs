using UnityEngine;

public class SpawnParticleOnExit:StateMachineBehaviour
{
    public float duration;
    public GameObject particlePrefab;
    public Transform spawnPoint;

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        GameObject particle = Instantiate(particlePrefab, spawnPoint.position, Quaternion.identity);
        Destroy(particle,duration);
    }
}
