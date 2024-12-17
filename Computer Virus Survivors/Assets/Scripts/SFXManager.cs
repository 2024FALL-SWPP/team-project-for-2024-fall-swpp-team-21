using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;
using UnityEngine.Audio;

public class SFXManager : Singleton<SFXManager>
{
    [SerializeField] private AudioMixerGroup audioMixerGroup;
    [SerializeField] private AudioMixerGroup audioMixerGroup_Virus;
    private List<TimeScaledAudioSource> audioSourcePool;
    private List<TimeScaledAudioSource> audioSourcePool_Virus;
    private int poolSize = 16;
    private int poolSize_Virus = 8;

    public override void Initialize()
    {
        if (audioSourcePool == null)
        {
            audioSourcePool = new List<TimeScaledAudioSource>();
        }

        if (audioSourcePool_Virus == null)
        {
            audioSourcePool_Virus = new List<TimeScaledAudioSource>();
        }
    }

    private TimeScaledAudioSource NewAudioSource(bool isVirus = false)
    {
        if (isVirus)
        {
            if (audioSourcePool_Virus.Count >= poolSize_Virus)
            {
                return null;
            }

            AudioSource audioSource = gameObject.AddComponent<AudioSource>();
            TimeScaledAudioSource timeScaledAudioSource = new TimeScaledAudioSource(audioSource);
            timeScaledAudioSource.playOnAwake = false;
            timeScaledAudioSource.outputAudioMixerGroup = audioMixerGroup_Virus;
            audioSourcePool_Virus.Add(timeScaledAudioSource);

            return timeScaledAudioSource;
        }
        else
        {

            if (audioSourcePool.Count >= poolSize)
            {
                return null;
            }

            AudioSource audioSource = gameObject.AddComponent<AudioSource>();
            TimeScaledAudioSource timeScaledAudioSource = new TimeScaledAudioSource(audioSource);
            timeScaledAudioSource.playOnAwake = false;
            timeScaledAudioSource.outputAudioMixerGroup = audioMixerGroup;
            audioSourcePool.Add(timeScaledAudioSource);

            return timeScaledAudioSource;
        }
    }

    private TimeScaledAudioSource GetAudioSource(bool isVirus = false)
    {
        List<TimeScaledAudioSource> asPool = null;
        if (isVirus)
        {
            if (audioSourcePool_Virus == null)
            {
                audioSourcePool_Virus = new List<TimeScaledAudioSource>();
            }
            asPool = audioSourcePool_Virus;
        }
        else
        {
            if (audioSourcePool == null)
            {
                audioSourcePool = new List<TimeScaledAudioSource>();
            }
            asPool = audioSourcePool;
        }

        foreach (var audioSource in asPool)
        {
            if (!audioSource.isPlaying)
            {
                return audioSource;
            }
        }

        return NewAudioSource(isVirus);
    }

    public void PlaySound_Virus(AudioClip clip)
    {
        if (clip == null)
        {
            return;
        }
        TimeScaledAudioSource audioSource = GetAudioSource(isVirus: true);
        if (audioSource == null)
        {
            return;
        }
        Debug.Log("PlaySound Virus SFX : " + clip.name);
        audioSource.clip = clip;
        audioSource.Play();
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

        foreach (var audioSource in audioSourcePool_Virus)
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
        public AudioMixerGroup outputAudioMixerGroup
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
