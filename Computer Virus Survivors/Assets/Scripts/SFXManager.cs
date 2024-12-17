using System.Collections.Generic;
using System.Collections;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;
using UnityEngine.Audio;
using System;

public class SFXManager : Singleton<SFXManager>
{
    [SerializeField] private AudioMixerGroup audioMixerGroup;
    [SerializeField] private AudioMixerGroup audioMixerGroup_Virus;
    [SerializeField] private AudioMixerGroup audioMixerGroup_Sequence;
    private List<TimeScaledAudioSource> audioSourcePool;
    private List<TimeScaledAudioSource> audioSourcePool_Virus;
    private List<TimeScaledAudioSource> sequenceAudioSourcePool;

    private Dictionary<int, Tuple<Coroutine, TimeScaledAudioSource>> playingSequence = new Dictionary<int, Tuple<Coroutine, TimeScaledAudioSource>>();

    private int poolSize = 128;
    private int poolSize_Virus = 64;
    private int sequencePoolSize = 32;

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

        if (sequenceAudioSourcePool == null)
        {
            sequenceAudioSourcePool = new List<TimeScaledAudioSource>();
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
            TimeScaledAudioSource timeScaledAudioSource = new TimeScaledAudioSource(audioSource, 100);
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

    private TimeScaledAudioSource GetSequenceAudioSource()
    {
        if (sequenceAudioSourcePool.Count >= sequencePoolSize)
        {
            return null;
        }

        foreach (var audioSource in sequenceAudioSourcePool)
        {
            if (!audioSource.isPlaying)
            {
                return audioSource;
            }
        }

        AudioSource newAudioSource = gameObject.AddComponent<AudioSource>();
        TimeScaledAudioSource timeScaledAudioSource = new TimeScaledAudioSource(newAudioSource, 10);
        timeScaledAudioSource.playOnAwake = false;
        timeScaledAudioSource.outputAudioMixerGroup = audioMixerGroup_Sequence;
        sequenceAudioSourcePool.Add(timeScaledAudioSource);

        return timeScaledAudioSource;
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

    public void PlaySoundSequence(SFXElement[] sfxElements, int id = -1)
    {

        TimeScaledAudioSource audioSource = GetSequenceAudioSource();
        if (audioSource == null)
        {
            return;
        }
        Debug.Log("PlaySoundSequence SFX : " + id);
        Coroutine newSequence = StartCoroutine(PlaySequence(audioSource, sfxElements));

        if (id != -1)
        {
            if (playingSequence.ContainsKey(id))
            {
                StopSoundSequence(id);
            }
            playingSequence.Add(id, new Tuple<Coroutine, TimeScaledAudioSource>(newSequence, audioSource));
        }
    }

    public void StopSoundSequence(int id = -1)
    {
        if (id != -1)
        {
            if (playingSequence.ContainsKey(id))
            {
                if (playingSequence[id] != null)
                {
                    StopCoroutine(playingSequence[id].Item1);
                    playingSequence[id].Item2.Stop();
                }
                playingSequence.Remove(id);
            }
        }
    }

    private IEnumerator PlaySequence(TimeScaledAudioSource audioSource, SFXElement[] sfxElements)
    {

        foreach (var clipInfo in sfxElements)
        {
            // 클립 설정
            audioSource.clip = clipInfo.clip;

            // 시작 시간부터 재생
            audioSource.time = clipInfo.startTime;
            audioSource.Play();

            // 루프 처리
            if (clipInfo.isLoop)
            {
                for (int i = 0; i < clipInfo.loopCount; i++)
                {
                    Debug.Log("Wait for loop : " + (clipInfo.endTime - clipInfo.startTime));
                    yield return new WaitForSeconds(clipInfo.endTime - clipInfo.startTime);
                    audioSource.Stop();
                    audioSource.time = clipInfo.startTime; // 다시 시작
                    audioSource.Play();
                }
            }
            else
            {
                // 루프가 아닌 경우 클립 재생 대기
                yield return new WaitForSeconds(clipInfo.endTime - clipInfo.startTime);
            }

            // 현재 클립 종료
            audioSource.Stop();
        }
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

        foreach (var audioSource in sequenceAudioSourcePool)
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
        public float time
        {
            get => audioSource.time;
            set => audioSource.time = value;
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

        public TimeScaledAudioSource(AudioSource audioSource, int priority = 128)
        {
            this.audioSource = audioSource;
            audioSource.priority = priority;
            originalAudioPitch = audioSource.pitch;
        }

        public void Play()
        {
            if (audioSource.clip == null)
            {
                return;
            }
            audioSource.Play();
        }

        public void Stop()
        {
            audioSource.Stop();
        }

        public void SetTimeScale(float timeScale)
        {
            audioSource.pitch = originalAudioPitch * timeScale;
        }
    }
}
