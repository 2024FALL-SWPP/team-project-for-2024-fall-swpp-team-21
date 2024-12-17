using System;
using UnityEngine;


[CreateAssetMenu(fileName = "SFXSequencePreset", menuName = "ScriptableObj/SFXSequencePreset", order = 1)]
public class SFXSequencePreset : ScriptableObject
{
    [Header("공격 이팩트 사운드 시퀸스")]
    [SerializeField] private SFXElement[] sfxElements;

    [Header("아이디")]
    [SerializeField] private int id = -1;

    public SFXElement[] SFXElements => sfxElements;

    public void Play()
    {
        SFXManager.instance.PlaySoundSequence(SFXElements, id);
    }

    public void Stop()
    {
        SFXManager.instance.StopSoundSequence(id);
    }

}

[Serializable]
public struct SFXElement
{
    public AudioClip clip;
    [Header("시작 시간, 종료 시간 (end == 0 : 끝까지)")]
    public Vector2 playTimeline;
    public bool isLoop;
    public int loopCount;

    public float startTime => playTimeline.x;
    public float endTime
    {
        get
        {
            if (playTimeline.y == 0)
            {
                return clip.length;
            }
            else
            {
                return playTimeline.y;
            }
        }
    }
}
