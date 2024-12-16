using UnityEngine;


[CreateAssetMenu(fileName = "CanvasSoundPreset", menuName = "ScriptableObj/CanvasSoundPreset", order = 1)]
public class CanvasSoundPreset : ScriptableObject
{
    [Header("캔버스 진입")]
    [SerializeField] private AudioClip enterSound;

    [Header("캔버스 나감")]
    [SerializeField] private AudioClip exitSound;


    public AudioClip EnterSound => enterSound;
    public AudioClip ExitSound => exitSound;
}
