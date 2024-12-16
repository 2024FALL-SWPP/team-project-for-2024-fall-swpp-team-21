using System.Collections.Generic;
using UnityEngine;

public class UISoundManager : Singleton<UISoundManager>
{
    private List<AudioSource> audioSourcePool;

    public override void Initialize()
    {
        audioSourcePool = new List<AudioSource>();
    }

    private AudioSource NewAudioSource()
    {
        AudioSource audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.outputAudioMixerGroup = AudioManager.instance.audioMixer.FindMatchingGroups("UI")[0];
        audioSourcePool.Add(audioSource);

        return audioSource;
    }

    private AudioSource GetAudioSource()
    {
        foreach (var audioSource in audioSourcePool)
        {
            if (!audioSource.isPlaying)
            {
                return audioSource;
            }
        }

        return NewAudioSource();
    }

    public void PlaySound(AudioClip clip)
    {
        if (clip == null)
        {
            return;
        }
        AudioSource audioSource = GetAudioSource();
        audioSource.clip = clip;
        audioSource.Play();
    }
}
