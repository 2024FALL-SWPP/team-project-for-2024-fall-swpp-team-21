
using System.Collections.Generic;
using UnityEngine;


public enum PoolType
{
    ExpGem,
    Virus_Weak,
    Virus_Trojan,
    Virus_Ransomware,
    Proj_PacketStream,
    Proj_ChainLightning,
    Proj_CoreDump,
    Proj_Drone,
    Proj_Beam,
    Proj_VaccineRing,
    DamageIndicator
}

public class PoolManager : Singleton<PoolManager>
{

    [System.Serializable]
    public class PoolObject
    {
        public PoolType poolName;
        public GameObject prefab;
        public int initialPoolSize;
    }

    [SerializeField] private List<PoolObject> prefabEntries;

    private Dictionary<PoolType, Queue<GameObject>> poolDict;

    public override void Initialize()
    {
        poolDict = new Dictionary<PoolType, Queue<GameObject>>();

        for (int i = 0; i < prefabEntries.Count; i++)
        {
            poolDict[prefabEntries[i].poolName] = new Queue<GameObject>();

            for (int j = 0; j < prefabEntries[i].initialPoolSize; j++)
            {
                GameObject obj = Instantiate(prefabEntries[i].prefab);
                obj.SetActive(false);
                poolDict[prefabEntries[i].poolName].Enqueue(obj);
            }
        }

    }

    public GameObject GetObject(PoolType poolType)
    {
        return GetObject(poolType, Vector3.zero, Quaternion.identity);
    }

    // Get함수는 Instantiate와 동일한 작동을 해야 하는 함수
    public GameObject GetObject(PoolType poolType, Vector3 position, Quaternion rotation)
    {
        GameObject selected;

        if (poolDict[poolType].Count > 0)
        {
            selected = poolDict[poolType].Dequeue();
            selected.transform.SetPositionAndRotation(position, rotation);
            selected.SetActive(true);
        }
        else
        {
            selected = Instantiate(prefabEntries.Find(x => x.poolName == poolType).prefab, position, rotation);
        }

        return selected;
    }


    public void ReturnObject(PoolType poolType, GameObject returned)
    {
        returned.SetActive(false);
        poolDict[poolType].Enqueue(returned);
    }
}
