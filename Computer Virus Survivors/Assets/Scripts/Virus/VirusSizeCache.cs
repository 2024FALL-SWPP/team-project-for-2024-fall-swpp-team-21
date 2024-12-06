using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

public sealed class VirusSizeCache : ChacheInfo<VirusSizeCache>
{
    public Dictionary<PoolType, float> virusSizes;

    protected override VirusSizeCache Generate(List<GameObject> prefabs)
    {
#if UNITY_EDITOR
        VirusSizeCache info = CreateInstance<VirusSizeCache>();
        info.virusSizes = new Dictionary<PoolType, float>();
        foreach (var prefab in prefabs)
        {
            GameObject prefabInstance = PrefabUtility.LoadPrefabContents(AssetDatabase.GetAssetPath(prefab));
            VirusBehaviour virus = prefabInstance.GetComponent<VirusBehaviour>();
            if (virus == null)
            {
                throw new Exception("VirusBehaviour 컴포넌트가 없는 프리팹입니다. : " + prefab.name);
            }
            Debug.Log("Virus : " + virus.name + " PoolType : " + virus.GetPoolType() + " Size : " + virus.GetVirusSize());
            info.virusSizes.Add(virus.GetPoolType(), virus.GetVirusSize());

            PrefabUtility.UnloadPrefabContents(prefabInstance);
        }
        return info;
#endif
    }
}
