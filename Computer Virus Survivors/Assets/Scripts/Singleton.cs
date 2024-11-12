using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    // 원래는
    // private static T instance
    // public static T Instance
    // 이었지만 instance를 쓰는 스크립트가 너무 많아서 임시로 ist로 바꿈
    // public인데 소문자인게 킹받음;;
    private static T ist;
    public static T instance
    {
        get
        {
            if (ist == null)
            {
                ist = FindObjectOfType<T>();
                if (ist == null)
                {
                    ist = new GameObject(typeof(T).Name).AddComponent<T>();
                }
            }
            return ist;
        }
    }

    public abstract void Initialize();
}
