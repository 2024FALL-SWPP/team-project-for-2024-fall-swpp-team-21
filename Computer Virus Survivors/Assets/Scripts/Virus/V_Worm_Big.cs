using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class V_Worm_Big : VirusBehaviour
{
    [SerializeField] private float spawnPeriod;
    [SerializeField] private float spawnNum;
    [SerializeField] private float spawnRange;

    [SerializeField] private SFXSequencePreset spawnSFXPreset;

    private float attackTimer = 0.0f;

    protected override void OnEnable()
    {
        base.OnEnable();
        attackTimer = 0.0f;
        SpawnManager.instance.SpawnTurret(0);
    }

    private void FixedUpdate()
    {
        Move();
        if (attackTimer >= spawnPeriod)
        {
            SpawnWorms();
            attackTimer = 0.0f;
        }
        attackTimer += Time.deltaTime;
        rb.velocity = Vector3.zero;
    }

    private void SpawnWorms()
    {
        spawnSFXPreset.Play();
        for (int i = 0; i < spawnNum; i++)
        {
            float x = transform.position.x + Random.Range(-spawnRange, spawnRange);
            float z = transform.position.z + Random.Range(-spawnRange, spawnRange);
            SpawnManager.instance.Spawn(PoolType.Virus_Worm, x, z, false);
        }
    }
}
