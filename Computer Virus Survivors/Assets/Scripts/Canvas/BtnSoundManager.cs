using UnityEngine;

public class BtnSoundManager : Singleton<BtnSoundManager>
{
    private AudioSource audioSource;

    public override void Initialize()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.priority = 1;
        audioSource.outputAudioMixerGroup = AudioManager.instance.audioMixer.FindMatchingGroups("BTN")[0];
    }

    public void PlaySound(AudioClip clip)
    {
        if (clip == null)
        {
            return;
        }
        audioSource.clip = clip;
        audioSource.Play();
    }
}
