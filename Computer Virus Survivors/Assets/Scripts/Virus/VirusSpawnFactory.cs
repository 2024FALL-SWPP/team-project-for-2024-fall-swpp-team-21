using System;
using System.Collections;
using UnityEngine;

public class VirusSpawnFactory : Singleton<VirusSpawnFactory>
{

    public override void Initialize()
    {

    }

    public void SpawnVirus(PoolType index, Vector3 position, Action<VirusBehaviour> delegateOnCreated)
    {
        StartCoroutine(SpawnStart(index, position, delegateOnCreated));
    }

    private IEnumerator SpawnStart(PoolType index, Vector3 position, Action<VirusBehaviour> delegateOnCreated)
    {
        Vector3 effectOffset = new Vector3(0, 0.5f, 0);
        VirusSpawnEffect spawnEffect = PoolManager.instance.GetObject(PoolType.Virus_SpawnEffect, position + effectOffset, Quaternion.identity).GetComponent<VirusSpawnEffect>();
        float spawnDuration = spawnEffect.GetComponent<ParticleSystem>().main.duration;

        yield return new WaitForSeconds(spawnDuration / 2f);

        Spawn(index, position, delegateOnCreated);
    }

    private void Spawn(PoolType index, Vector3 position, Action<VirusBehaviour> delegateOnCreated)
    {
        VirusBehaviour virus = PoolManager.instance.GetObject
        (
            index,
            position,
            Quaternion.identity
        ).GetComponent<VirusBehaviour>();

        delegateOnCreated(virus);

        StartCoroutine(VirusSizeUp(virus));
    }

    private IEnumerator VirusSizeUp(VirusBehaviour virus)
    {
        float duration = 1f;
        float elapsedTime = 0f;
        float startSize = 0;
        float targetSize = virus.gameObject.transform.localScale.x;

        while (elapsedTime < duration)
        {
            virus.gameObject.transform.localScale = Vector3.one * Mathf.Lerp(startSize, targetSize, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        virus.transform.localScale = Vector3.one * targetSize;
    }
}
