using System;
using System.Collections;
using UnityEngine;

public class VirusSpawnFactory : Singleton<VirusSpawnFactory>
{

    public override void Initialize()
    {

    }


    /// <summary>
    /// 바이러스를 생성합니다.
    /// </summary>
    /// <param name="index">생성할 바이러스의 풀타입</param>
    /// <param name="position">바이러스를 생성할 위치</param>
    /// <param name="callbackOnCreated">바이러스 스폰 직후 호출할 콜백. 인자엔 호출된 바이러스가 들어감.</param>
    public void SpawnVirus(PoolType index, Vector3 position, Action<VirusBehaviour> callbackOnCreated)
    {
        StartCoroutine(SpawnStart(index, position, callbackOnCreated));
    }

    private IEnumerator SpawnStart(PoolType index, Vector3 position, Action<VirusBehaviour> callbackOnCreated)
    {
        Vector3 effectOffset = new Vector3(0, 0.5f, 0);
        VirusSpawnEffect spawnEffect = PoolManager.instance.GetObject(PoolType.Virus_SpawnEffect, position + effectOffset, Quaternion.identity).GetComponent<VirusSpawnEffect>();
        float spawnDuration = spawnEffect.GetComponent<ParticleSystem>().main.duration;

        yield return new WaitForSeconds(spawnDuration / 2f);

        Spawn(index, position, callbackOnCreated);
    }

    private void Spawn(PoolType index, Vector3 position, Action<VirusBehaviour> callbackOnCreated)
    {
        VirusBehaviour virus = PoolManager.instance.GetObject
        (
            index,
            position,
            Quaternion.identity
        ).GetComponent<VirusBehaviour>();

        callbackOnCreated(virus);

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
