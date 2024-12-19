using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;

public abstract class CacheInfo<T> : ScriptableObject where T : CacheInfo<T>
{
    public static T GenerateChecheFromPrefab(List<GameObject> prefab)
    {
        T instance = CreateInstance<T>();
        return instance.Generate(prefab);
    }

    protected abstract T Generate(List<GameObject> prefabs);

}

public abstract class CacheGenerator<T> : MonoBehaviour where T : CacheInfo<T>
{
    public List<GameObject> cachePrefabs;
    protected string cacheSavePath;

    protected abstract void SetCacheSavePath();

    public void GenerateCaches()
    {
#if UNITY_EDITOR
        if (cachePrefabs != null)
        {
            T cachedInfo = CacheInfo<T>.GenerateChecheFromPrefab(cachePrefabs);
            AssetDatabase.CreateAsset(cachedInfo, cacheSavePath);
            AssetDatabase.SaveAssets();
        }
#endif
    }

    public abstract void LoadGameObjects();


    // 아래의 코드는 각 캐시 생성기마다 다르기 때문에 각각의 캐시 생성기에서 구현해야함

    // [CustomEditor(typeof(__구현_클래스_이름__))]
    // public class CacheGeneratorEditor : Editor
    // {
    //     public override void OnInspectorGUI()
    //     {
    //         DrawDefaultInspector();

    //         if (GUILayout.Button("__프리팹_로드_버튼__"))
    //         {
    //             LoadVirusPrefabs();
    //         }

    //         if (GUILayout.Button("Generate Caches"))
    //         {
    //             GenerateCaches();
    //         }
    //     }

    //     private void LoadVirusPrefabs()
    //     {
    //         CacheGenerator<T> manager = (CacheGenerator<T>) target;
    //         manager.SetCacheSavePath();
    //         manager.LoadGameObjects();

    //     }

    //     private void GenerateCaches()
    //     {
    //         CacheGenerator<T> manager = (CacheGenerator<T>) target;
    //         manager.SetCacheSavePath();
    //         manager.GenerateCaches();

    //     }

    // }
}

