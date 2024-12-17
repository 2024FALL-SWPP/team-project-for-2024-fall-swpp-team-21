using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;

public class SFXManager : Singleton<SFXManager>
{
    private List<TimeScaledAudioSource> audioSourcePool;
    private int poolSize = 16;

    public override void Initialize()
    {
        if (audioSourcePool == null)
        {
            audioSourcePool = new List<TimeScaledAudioSource>();
        }
    }

    private TimeScaledAudioSource NewAudioSource()
    {
        if (audioSourcePool.Count >= poolSize)
        {
            return null;
        }

        AudioSource audioSource = gameObject.AddComponent<AudioSource>();
        TimeScaledAudioSource timeScaledAudioSource = new TimeScaledAudioSource(audioSource);
        timeScaledAudioSource.playOnAwake = false;
        timeScaledAudioSource.outputAudioMixerGroup = AudioManager.instance.audioMixer.FindMatchingGroups("SFX")[0];
        audioSourcePool.Add(timeScaledAudioSource);

        return timeScaledAudioSource;
    }

    private TimeScaledAudioSource GetAudioSource()
    {
        if (audioSourcePool == null)
        {
            audioSourcePool = new List<TimeScaledAudioSource>();
        }
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
        TimeScaledAudioSource audioSource = GetAudioSource();
        if (audioSource == null)
        {
            return;
        }
        Debug.Log("PlaySound SFX : " + clip.name);
        audioSource.clip = clip;
        audioSource.Play();
    }

    private void Update()
    {
        foreach (var audioSource in audioSourcePool)
        {
            audioSource.SetTimeScale(Time.timeScale);
        }
    }

    private class TimeScaledAudioSource
    {
        public AudioSource audioSource;
        public float originalAudioPitch;
        public AudioClip clip
        {
            get => audioSource.clip;
            set => audioSource.clip = value;
        }
        public bool isPlaying
        {
            get => audioSource.isPlaying;
        }
        public bool playOnAwake
        {
            get => audioSource.playOnAwake;
            set => audioSource.playOnAwake = value;
        }
        public UnityEngine.Audio.AudioMixerGroup outputAudioMixerGroup
        {
            get => audioSource.outputAudioMixerGroup;
            set => audioSource.outputAudioMixerGroup = value;
        }

        public TimeScaledAudioSource(AudioSource audioSource)
        {
            this.audioSource = audioSource;
            audioSource.priority = 128;
            originalAudioPitch = audioSource.pitch;
        }

        public void Play()
        {
            audioSource.Play();
        }

        public void SetTimeScale(float timeScale)
        {
            audioSource.pitch = originalAudioPitch * timeScale;
        }
    }
}
