using UnityEngine;


[CreateAssetMenu(fileName = "BtnSoundPreset", menuName = "ScriptableObj/BtnSoundPreset", order = 1)]
public class BtnSoundPreset : ScriptableObject
{
    [Header("마우스 진입")]
    [SerializeField] private AudioClip mouseEnter;

    [Header("마우스 나감")]
    [SerializeField] private AudioClip mouseExit;

    [Header("버튼 클릭(성공)")]
    [SerializeField] private AudioClip buttonClick;

    [Header("버튼 클릭(실패)")]
    [SerializeField] private AudioClip buttonClickFail;

    public AudioClip MouseEnter => mouseEnter;
    public AudioClip MouseExit => mouseExit;
    public AudioClip ButtonClick => buttonClick;
    public AudioClip ButtonClickFail => buttonClickFail;
}
