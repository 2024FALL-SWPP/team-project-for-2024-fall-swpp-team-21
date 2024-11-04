using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VP_EncryptionSpike : MonoBehaviour
{
    private int damage = 10;
    private float speed = 10.0f;
    private float debuffDegree = 0.5f;
    private float debuffDuration = 3.0f;

    private Vector3 direction;

    public void Initialize(Vector3 direction, int damage, float speed, float debuffDegree, float debuffDuration)
    {
        this.direction = direction;
        this.damage = damage;
        this.speed = speed;
        this.debuffDegree = debuffDegree;
        this.debuffDuration = debuffDuration;
    }

    // Update is called once per frame
    private void Update()
    {
        transform.Translate(speed * Time.deltaTime * Vector3.forward, Space.Self);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerController>().GetDamage(damage);
            other.GetComponent<PlayerController>().BuffMoveSpeed(debuffDegree, debuffDuration);
        }
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
