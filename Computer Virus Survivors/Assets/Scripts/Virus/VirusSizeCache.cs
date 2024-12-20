using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

public sealed class VirusSizeCache : CacheInfo<VirusSizeCache>
{
    [Serializable]
    public struct KeyValPair
    {
        public PoolType poolType;
        public float size;

        public KeyValPair(PoolType poolType, float size)
        {
            this.poolType = poolType;
            this.size = size;
        }

        // public KeyValPair(KeyValPair other)
        // {
        //     poolType = other.poolType;
        //     size = other.size;
        // }
    }

    [Serializable]
    public class Dict
    {
        public List<KeyValPair> list;
        private static KeyValPairComparer _comparer = new KeyValPairComparer();

        public Dict()
        {
            list = new List<KeyValPair>();
        }

        public void Add(PoolType poolType, float size)
        {
            list.Add(new KeyValPair(poolType, size));
            list.Sort(_comparer);
        }

        public float Get(PoolType poolType)
        {
            int index = list.BinarySearch(new KeyValPair(poolType, 0), _comparer);
            if (index >= 0)
            {
                return list[index].size;
            }
            return 0;
        }

        private class KeyValPairComparer : IComparer<KeyValPair>
        {
            public int Compare(KeyValPair x, KeyValPair y)
            {
                return x.poolType.CompareTo(y.poolType);
            }
        }
    }

    [SerializeField]
    private Dict virusSizes;

    public float GetVirusSize(PoolType poolType)
    {
        return virusSizes.Get(poolType);
    }

    protected override VirusSizeCache Generate(List<GameObject> prefabs)
    {
#if UNITY_EDITOR
        VirusSizeCache info = CreateInstance<VirusSizeCache>();
        info.virusSizes = new Dict();
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
#else
        return null;
#endif
    }
}
