using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : Singleton<AudioManager>
{
    [SerializeField] public AudioMixer audioMixer;

    [SerializeField] private BgmManager bgmManager;
    [SerializeField] private BtnSoundManager btnSoundManager;
    [SerializeField] private UISoundManager uiSoundManager;
    [SerializeField] private SFXManager sfxManager;

    private void Awake()
    {
        Initialize();
        StartCoroutine(FadeIn());
    }

    public override void Initialize()
    {
        bgmManager?.Initialize();
        btnSoundManager?.Initialize();
        uiSoundManager?.Initialize();
        sfxManager?.Initialize();
    }

    private IEnumerator FadeIn()
    {
        float elapsedTime = 0;
        while (elapsedTime < 1f)
        {
            SetVolume(elapsedTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    private void SetVolume(float volume)
    {
        float dB = Mathf.Log10(volume) * 20;
        audioMixer.SetFloat("MasterVolume", dB);
    }

}
