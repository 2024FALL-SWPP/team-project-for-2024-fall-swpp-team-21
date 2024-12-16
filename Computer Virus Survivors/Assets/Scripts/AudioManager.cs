using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : Singleton<AudioManager>
{
    [SerializeField] public AudioMixer audioMixer;
    [SerializeField] private BgmManager bgmManager;
    [SerializeField] private BtnSoundManager btnSoundManager;
    [SerializeField] private UISoundManager uiSoundManager;

    private void Awake()
    {
        Initialize();
    }

    public override void Initialize()
    {
        bgmManager.Initialize();
        btnSoundManager.Initialize();
        uiSoundManager.Initialize();
    }

}
