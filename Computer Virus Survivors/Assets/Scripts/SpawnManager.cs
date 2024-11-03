using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    /* Temp version of SpawnManager*/

    [SerializeField] private GameObject player;
    [SerializeField] private Vector2 spawnRange;


    // Update is called once per frame
    public void Spawn(PoolType index)
    {
        float x = Random.Range(-spawnRange.x, spawnRange.x);
        float z = Random.Range(-spawnRange.y, spawnRange.y);

        Vector3 spawnPosition = player.transform.position + new Vector3(x, 0, z);

        GameObject virus = PoolManager.instance.GetObject
        (
            index,
            spawnPosition,
            Quaternion.LookRotation(player.transform.position - spawnPosition)
        );

        virus.GetComponent<VirusBehaviour>().Initialize(player);
    }

}
