using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VP_DataBurst : MonoBehaviour
{

    private int damage;
    private float speed;
    private float acceleration;

    public void Initialize(int damage, float initialSpeed, float acceleration)
    {
        this.damage = damage;
        speed = initialSpeed;
        this.acceleration = acceleration;

        SphereCollider collider = GetComponent<SphereCollider>();
        float offsetY = -transform.position.y / transform.lossyScale.y;
        collider.center = new Vector3(collider.center.x, offsetY, collider.center.z);
    }

    private void Update()
    {
        transform.Translate(speed * Time.deltaTime * Vector3.forward, Space.Self);
        speed += acceleration * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerController>().GetDamage(damage);
            Destroy(gameObject);
        }
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
