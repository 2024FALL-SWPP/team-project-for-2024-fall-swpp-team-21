using System;
using System.Collections;
using UnityEngine;

public class BgmManager : Singleton<BgmManager>
{
    [SerializeField] private BgmPlayList bgmPlayList;
    [SerializeField] private float fadeTime = 2f;
    private AudioSource audioSource;

    public override void Initialize()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.outputAudioMixerGroup = AudioManager.instance.audioMixer.FindMatchingGroups("BGM")[0];
        PlayBgm();
    }

    public void PlayBgm()
    {
        audioSource.clip = NextBgm();
        audioSource.loop = false;
        if (audioSource.clip != null)
        {
            audioSource.Play();
            StopAllCoroutines();
            StartCoroutine(SoundFadeIn());
            StartCoroutine(WaitUntilBgmFinished());
        }
    }

    public void StopBgm()
    {
        StopAllCoroutines();
        StartCoroutine(SoundFadeOut(() =>
        {
            audioSource.Stop();
            audioSource.volume = 0;
            PlayBgm();
        }));
    }

    private IEnumerator WaitUntilBgmFinished()
    {
        yield return new WaitUntil(() => audioSource.clip.length - audioSource.time < fadeTime);
        StopBgm();
    }

    private AudioClip NextBgm()
    {
        int record = UnityEngine.Random.Range(0, bgmPlayList.BgmList.Length);
        return bgmPlayList.BgmList[record];
    }

    private IEnumerator SoundFadeIn()
    {
        float elapsedTime = 0;
        while (elapsedTime < fadeTime)
        {
            elapsedTime += Time.deltaTime;
            audioSource.volume = elapsedTime / fadeTime;
            yield return null;
        }
    }

    private IEnumerator SoundFadeOut(Action callback = null)
    {
        Debug.Log("SoundFadeOut");
        float elapsedTime = 0;
        while (elapsedTime < fadeTime)
        {
            elapsedTime += Time.deltaTime;
            audioSource.volume = 1 - elapsedTime / fadeTime;
            yield return null;
        }
        callback?.Invoke();
    }
}
