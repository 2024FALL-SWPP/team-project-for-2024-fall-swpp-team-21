using UnityEngine;


[CreateAssetMenu(fileName = "SFXPreset", menuName = "ScriptableObj/SFXPreset", order = 1)]
public class SFXPreset : ScriptableObject
{
    [Header("공격 이팩트 사운드")]
    [SerializeField] private AudioClip[] sfxClips;

    public AudioClip[] SfxClips => sfxClips;

    private AudioClip GetRandomClip()
    {
        return sfxClips[Random.Range(0, sfxClips.Length)];
    }

    public void Play()
    {
        SFXManager.instance.PlaySound(GetRandomClip());
    }
}
