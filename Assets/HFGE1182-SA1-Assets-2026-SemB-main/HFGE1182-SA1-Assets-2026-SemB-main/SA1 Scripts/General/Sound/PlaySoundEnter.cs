using UnityEngine;

public class PlaySoundEnter : StateMachineBehaviour
{
    [SerializeField] private string soundName;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        SoundManager.Instance.Play(soundName);
    }
}
