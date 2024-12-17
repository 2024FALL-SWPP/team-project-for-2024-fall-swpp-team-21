using UnityEngine;

[CreateAssetMenu(fileName = "BgmPlayList", menuName = "ScriptableObj/BgmPlayList", order = 1)]
public class BgmPlayList : ScriptableObject
{
    [Header("BGM 리스트")]
    [SerializeField] private AudioClip[] bgmList;

    public AudioClip[] BgmList => bgmList;
}
