using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirusBehaviour : MonoBehaviour
{

    [SerializeField] protected VirusData virusData;
    protected GameObject player;
    protected PlayerController playerController;
    protected Rigidbody rb;


    protected int currentHP;

    protected virtual void Start()
    {
        player = GameManager.instance.Player;
        playerController = player.GetComponent<PlayerController>();
        rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        currentHP = virusData.maxHP;
    }


    protected void Move()
    {
        Vector3 moveDirection = Vector3.ProjectOnPlane(
            (player.transform.position - transform.position).normalized,
            Vector3.up);
        transform.Translate(virusData.moveSpeed * Time.deltaTime * moveDirection, Space.World);
        transform.rotation = Quaternion.LookRotation(moveDirection);
    }

    protected void Die()
    {
        PoolManager.instance.ReturnObject(virusData.poolType, gameObject);
        SpawnManager.instance.OnVirusDestroyed();

        GameObject expGem = PoolManager.instance.GetObject(PoolType.ExpGem, transform.position, transform.rotation);
        expGem.GetComponent<ExpGem>().Initialize(virusData.dropExp);
    }

    public void GetDamage(int damage)
    {
        currentHP -= damage;
        if (currentHP <= 0)
        {
            Die();
        }
    }

    protected void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerController.GetDamage(virusData.contactDamage);
        }
    }

    protected void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerController.GetDamage(virusData.contactDamage);
        }
    }
}
