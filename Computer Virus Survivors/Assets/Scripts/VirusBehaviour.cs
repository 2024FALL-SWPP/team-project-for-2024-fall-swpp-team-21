using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirusBehaviour : MonoBehaviour
{
    protected GameObject player;
    protected int maxHP;
    protected int currentHP;
    protected float moveSpeed;
    protected int dropExp;
    protected int contactDamage;

    // // Start is called before the first frame update
    // void Start()
    // {
        
    // }

    public void Initialize(GameObject player)
    {
        this.player = player;
    }

    // // Update is called once per frame
    // void Update()
    // {
    //     Move();
    // }

    protected void Move()
    {
        Vector3 moveDirection = (player.transform.position - transform.position).normalized;
        transform.Translate(moveSpeed * Time.deltaTime * moveDirection, Space.World);
        transform.rotation = Quaternion.LookRotation(moveDirection);
    }

    protected void Die()
    {
        // TODO: Object pooling
        Destroy(gameObject);

        // TODO: Drop exp + object pooling
        // Instantiate(expPrefab, transform.position, expPrefab.transform.rotation);
    }

    public void GetDamage(int damage)
    {
        currentHP -= damage;
        if(currentHP <= 0)
        {
            Die();
        }
    }

    protected void Attack()
    {

    }

    protected void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerController>().GetDamage(contactDamage);
        }
    }
}
