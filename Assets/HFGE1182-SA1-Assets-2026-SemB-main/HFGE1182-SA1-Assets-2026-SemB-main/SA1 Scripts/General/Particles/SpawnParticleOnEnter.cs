using UnityEngine;

public class SpawnParticleOnEnter:StateMachineBehaviour
{
    public float duration;
    public GameObject particlePrefab;
    public Transform spawnPoint;
    
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        GameObject particle = Instantiate(particlePrefab, spawnPoint.position, Quaternion.identity);
        Destroy(particle,duration);
    }
}
