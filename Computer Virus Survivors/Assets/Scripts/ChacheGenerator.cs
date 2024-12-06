using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;

public abstract class ChacheInfo<T> : ScriptableObject where T : ChacheInfo<T>
{
    public static T GenerateChecheFromPrefab(List<GameObject> prefab)
    {
        T instance = CreateInstance<T>();
        return instance.Generate(prefab);
    }

    protected abstract T Generate(List<GameObject> prefabs);

}

public abstract class ChacheGenerator<T> : MonoBehaviour where T : ChacheInfo<T>
{
    public List<GameObject> chachePrefabs;
    protected string cacheSavePath;

    protected abstract void SetCacheSavePath();

    public void GenerateChaches()
    {
#if UNITY_EDITOR
        if (chachePrefabs != null)
        {
            T cachedInfo = ChacheInfo<T>.GenerateChecheFromPrefab(chachePrefabs);
            AssetDatabase.CreateAsset(cachedInfo, cacheSavePath);
            AssetDatabase.SaveAssets();
        }
#endif
    }

    public abstract void LoadGameObjects();


    // 아래의 코드는 각 캐시 생성기마다 다르기 때문에 각각의 캐시 생성기에서 구현해야함

    // [CustomEditor(typeof(__구현_클래스_이름__))]
    // public class ChacheGeneratorEditor : Editor
    // {
    //     public override void OnInspectorGUI()
    //     {
    //         DrawDefaultInspector();

    //         if (GUILayout.Button("__프리팹_로드_버튼__"))
    //         {
    //             LoadVirusPrefabs();
    //         }

    //         if (GUILayout.Button("Generate Chaches"))
    //         {
    //             GenerateChaches();
    //         }
    //     }

    //     private void LoadVirusPrefabs()
    //     {
    //         ChacheGenerator<T> manager = (ChacheGenerator<T>) target;
    //         manager.SetCacheSavePath();
    //         manager.LoadGameObjects();

    //     }

    //     private void GenerateChaches()
    //     {
    //         ChacheGenerator<T> manager = (ChacheGenerator<T>) target;
    //         manager.SetCacheSavePath();
    //         manager.GenerateChaches();

    //     }

    // }
}

