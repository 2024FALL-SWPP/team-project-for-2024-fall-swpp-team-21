using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirusBehaviour : MonoBehaviour
{

    [SerializeField] protected VirusData virusData;
    protected GameObject player;
    protected PoolManager poolManager;
    protected PlayerController playerController;

    protected int currentHP;

    public void Initialize(GameObject player, PoolManager poolManager)
    {
        if (this.player == null)
        {
            this.player = player;
            this.poolManager = poolManager;
            playerController = player.GetComponent<PlayerController>();
        }
    }

    protected void OnEnable()
    {
        currentHP = virusData.maxHP;
    }

    // protected void Update()
    // {
    //     Move();
    // }

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

        gameObject.SetActive(false);
        // Drop exp
        GameObject expGem = poolManager.Get(PoolType.ExpGem, transform.position, Quaternion.LookRotation(Vector3.forward));
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

    // protected void Attack(
    // {

    // }

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
