using UnityEngine;

public class BtnSoundManager : Singleton<BtnSoundManager>
{
    private AudioSource audioSource;

    public override void Initialize()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.outputAudioMixerGroup = AudioManager.instance.audioMixer.FindMatchingGroups("UI")[0];
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
