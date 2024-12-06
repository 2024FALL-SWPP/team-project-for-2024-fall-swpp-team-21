using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public sealed class VirusChacheGenerator : ChacheGenerator<VirusSizeCache>
{
    protected override void SetCacheSavePath()
    {
        cacheSavePath = "Assets/Scripts/Scriptable/VirusSizeTable.asset";
    }

    public override void LoadGameObjects()
    {
        chachePrefabs = new List<GameObject>();

        string[] prefabGuids = AssetDatabase.FindAssets("t:Prefab", new[] { "Assets/Prefabs" });

        foreach (string guid in prefabGuids)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guid);
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);

            if (prefab != null && prefab.GetComponent<VirusBehaviour>() != null)
            {
                chachePrefabs.Add(prefab);
            }
        }


    }



    /// <summary>
    /// 인스펙터 창에서 자동으로 선택 오브젝트를 로드하는 버튼을 추가
    /// 캐시 생성 버튼을 추가
    /// </summary>
    [CustomEditor(typeof(VirusChacheGenerator))]
    public class VirusChacheGeneratorEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            if (GUILayout.Button("Load Virus Prefabs"))
            {
                LoadVirusPrefabs();
            }

            if (GUILayout.Button("Generate Chaches"))
            {
                GenerateChaches();
            }
        }

        private void LoadVirusPrefabs()
        {
            VirusChacheGenerator manager = (VirusChacheGenerator) target;
            manager.SetCacheSavePath();
            manager.LoadGameObjects();

        }

        private void GenerateChaches()
        {
            VirusChacheGenerator manager = (VirusChacheGenerator) target;
            manager.SetCacheSavePath();
            manager.GenerateChaches();

        }

    }
}
