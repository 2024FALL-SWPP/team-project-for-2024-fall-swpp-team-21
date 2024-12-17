using UnityEngine;


[CreateAssetMenu(fileName = "SFXPreset", menuName = "ScriptableObj/SFXPreset", order = 1)]
public class SFXPreset : ScriptableObject
{
    [Header("공격 이팩트 사운드(동시)")]
    [SerializeField] private AudioClip[] sfxClips;

    [Header("바이러스용 사운드")]
    [SerializeField] private bool isVirus;

    public AudioClip[] SfxClips => sfxClips;

    public void Play()
    {
        if (isVirus)
        {
            foreach (var audioClip in sfxClips)
            {
                SFXManager.instance.PlaySound_Virus(audioClip);
            }
        }
        else
        {
            foreach (var audioClip in sfxClips)
            {
                SFXManager.instance.PlaySound(audioClip);
            }
        }
    }
}
