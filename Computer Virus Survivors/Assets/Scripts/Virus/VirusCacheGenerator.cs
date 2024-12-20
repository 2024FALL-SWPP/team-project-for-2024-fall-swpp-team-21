#if UNITY_EDITOR
using UnityEditor;
using System.Collections.Generic;
using UnityEngine;

public sealed class VirusCacheGenerator : CacheGenerator<VirusSizeCache>
{
    protected override void SetCacheSavePath()
    {
        cacheSavePath = "Assets/Scripts/Scriptable/VirusSizeTable.asset";
    }

    public override void LoadGameObjects()
    {
        cachePrefabs = new List<GameObject>();

        string[] prefabGuids = AssetDatabase.FindAssets("t:Prefab", new[] { "Assets/Prefabs" });

        foreach (string guid in prefabGuids)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guid);
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);

            if (prefab != null && prefab.GetComponent<VirusBehaviour>() != null)
            {
                cachePrefabs.Add(prefab);
            }
        }


    }



    /// <summary>
    /// 인스펙터 창에서 자동으로 선택 오브젝트를 로드하는 버튼을 추가
    /// 캐시 생성 버튼을 추가
    /// </summary>
    [CustomEditor(typeof(VirusCacheGenerator))]
    public class VirusCacheGeneratorEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            if (GUILayout.Button("Load Virus Prefabs"))
            {
                LoadVirusPrefabs();
            }

            if (GUILayout.Button("Generate Caches"))
            {
                GenerateCaches();
            }
        }

        private void LoadVirusPrefabs()
        {
            VirusCacheGenerator manager = (VirusCacheGenerator) target;
            manager.SetCacheSavePath();
            manager.LoadGameObjects();

        }

        private void GenerateCaches()
        {
            VirusCacheGenerator manager = (VirusCacheGenerator) target;
            manager.SetCacheSavePath();
            manager.GenerateCaches();

        }

    }
}
#endif
