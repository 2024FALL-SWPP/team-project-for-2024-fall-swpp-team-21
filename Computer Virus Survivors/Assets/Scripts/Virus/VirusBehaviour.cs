using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirusBehaviour : MonoBehaviour
{
    protected GameObject player;
    protected int maxHP;
    protected float moveSpeed;
    protected int dropExp;
    protected int contactDamage;

    protected int currentHP;

    protected PoolManager poolManager;

    public GameObject expPrefab;

    public void Initialize(GameObject player, PoolManager poolManager)
    {
        this.player = player;
        this.poolManager = poolManager;
    }

    protected void OnEnable()
    {
        currentHP = maxHP;
    }

    // protected void Update()
    // {
    //     Move();
    // }

    protected void Move()
    {
        Vector3 playerPos = new Vector3(player.transform.position.x, 0, player.transform.position.z);
        Vector3 myPos = new Vector3(transform.position.x, 0, transform.position.z);
        Vector3 moveDirection = (playerPos - myPos).normalized;
        transform.Translate(moveSpeed * Time.deltaTime * moveDirection, Space.World);
        transform.rotation = Quaternion.LookRotation(moveDirection);
    }

    protected void Die()
    {
        gameObject.SetActive(false);

        // Drop exp
        GameObject expGem = poolManager.Get(0, transform.position);
        expGem.GetComponent<ExpGem>().Initialize(dropExp);
        expGem.SetActive(true);
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
            collision.gameObject.GetComponent<PlayerController>().GetDamage(contactDamage);
        }
    }

    protected void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerController>().GetDamage(contactDamage);
        }
    }
}
