using UnityEngine;


[CreateAssetMenu(fileName = "SFXPreset", menuName = "ScriptableObj/SFXPreset", order = 1)]
public class SFXPreset : ScriptableObject
{
    [Header("공격 이팩트 사운드(동시)")]
    [SerializeField] private AudioClip[] sfxClips;

    public AudioClip[] SfxClips => sfxClips;

    public void Play()
    {
        foreach (var audioClip in sfxClips)
        {
            SFXManager.instance.PlaySound(audioClip);
        }
    }
}
