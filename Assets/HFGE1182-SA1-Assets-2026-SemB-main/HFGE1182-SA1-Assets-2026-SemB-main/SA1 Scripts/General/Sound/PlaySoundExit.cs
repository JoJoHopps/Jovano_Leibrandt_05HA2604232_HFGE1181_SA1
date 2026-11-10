using UnityEngine;

public class PlaySoundExit : StateMachineBehaviour
{
    [SerializeField] private string soundName;

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        SoundManager.Instance.Play(soundName);
    }

}
