using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class P_PacketStream : ProjectileBehaviour
{
    [SerializeField] private float bulletSpeed;

    private void OnBecameInvisible()
    {
        //enabled = false;
    }

    private void Update()
    {
        transform.Translate(bulletSpeed * Time.deltaTime * Vector3.forward);
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Virus"))
        {
            other.GetComponent<VirusBehaviour>().GetDamage(damage);
            Destroy(gameObject);
        }
    }
}
