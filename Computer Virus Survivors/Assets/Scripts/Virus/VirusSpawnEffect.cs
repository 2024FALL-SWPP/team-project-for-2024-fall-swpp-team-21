using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirusSpawnEffect : MonoBehaviour
{
    private void OnParticleSystemStopped()
    {
        PoolManager.instance.ReturnObject(PoolType.Virus_SpawnEffect, gameObject);
    }
}
