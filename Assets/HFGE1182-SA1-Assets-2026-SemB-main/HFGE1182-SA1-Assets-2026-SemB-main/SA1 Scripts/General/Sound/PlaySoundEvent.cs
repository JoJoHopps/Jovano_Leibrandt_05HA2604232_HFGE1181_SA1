using UnityEngine;

public class PlaySoundEvent : MonoBehaviour
{
    public void PlaySound(string soundName)
    {
        SoundManager.Instance.Play(soundName);
    }
}
