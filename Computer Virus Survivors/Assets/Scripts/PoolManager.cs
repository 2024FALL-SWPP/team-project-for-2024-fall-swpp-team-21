
using System.Collections.Generic;
using UnityEngine;


public enum PoolType
{
    ExpGem,
    HealPack,
    ExpMagnet,
    Virus_Chip,
    Virus_Chip_Angry,
    Virus_Corona,
    Virus_Corona_Angry,
    Virus_Worm,
    Virus_Worm_Angry,
    Virus_Adware,
    Virus_Adware_Angry,
    Virus_Trojan,
    Virus_Worm_Big,
    Virus_Ransomware,
    Proj_PacketStream,
    Proj_ChainLightning,
    Proj_CoreDump,
    Proj_Drone,
    Proj_Beam,
    Proj_VaccineRing,
    VProj_Beam,
    VProj_EncryptionSpike,
    VProj_CorruptedZone,
    VProj_TrackingBolt,
    VProj_DataBurst,
    DamageIndicator,
    Turret,
    Virus_TrashCan,
    VFX_Virus_SpawnEffect,
    VProj_Mail,
    VFX_BasicHit1,
    VFX_BasicHit2,
    VFX_BasicHit3,
    VFX_Explosion,
    VFX_IceHit,
    VFX_LightningHit,
    VFX_MagicHit,
    VFX_BasicHit1_Crit,
    VFX_BasicHit2_Crit,
    VFX_BasicHit3_Crit,
    VFX_Explosion_Crit,
    VFX_IceHit_Crit,
    VFX_LightningHit_Crit,
    VFX_MagicHit_Crit,
    VProj_Mail_Angry,
    RedZone,
    TurretLaserLine,
    None = -1,

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
