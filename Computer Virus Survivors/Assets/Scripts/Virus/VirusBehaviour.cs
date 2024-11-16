using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirusBehaviour : MonoBehaviour
{

    public event Action<VirusBehaviour> OnDie;

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
#if !WEAPON_LAB
        Vector3 moveDirection = Vector3.ProjectOnPlane(
            (player.transform.position - transform.position).normalized,
            Vector3.up);
        transform.Translate(virusData.moveSpeed * Time.deltaTime * moveDirection, Space.World);
        transform.rotation = Quaternion.LookRotation(moveDirection);
#endif
    }

    protected virtual void Die()
    {
        OnDie?.Invoke(this);

        GameObject expGem = PoolManager.instance.GetObject(PoolType.ExpGem, transform.position, transform.rotation);
        expGem.GetComponent<ExpGem>().Initialize(virusData.dropExp);

        PoolManager.instance.ReturnObject(virusData.poolType, gameObject);
    }

    public void GetDamage(int damage)
    {
        if (damage != 0)
        {

            currentHP -= damage;
            PoolManager.instance.GetObject(PoolType.DamageIndicator)
                .GetComponent<DamageIndicator>().Initialize(damage, transform.position);
            if (currentHP <= 0)
            {
                Die();
            }

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
