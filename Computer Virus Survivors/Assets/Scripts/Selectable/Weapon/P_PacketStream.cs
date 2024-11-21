using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class P_PacketStream : ProjectileBehaviour
{
    [SerializeField] private float bulletSpeed;

    private void OnEnable()
    {
        CapsuleCollider collider = GetComponent<CapsuleCollider>();

        float currentWorldY = transform.position.y;
        float offsetY = -currentWorldY / transform.lossyScale.y;

        collider.center = new Vector3(collider.center.x, offsetY, collider.center.z);
    }

    private void Update()
    {
        transform.Translate(bulletSpeed * Time.deltaTime * Vector3.forward);
        if (CheckOutOfScreen())
        {
            PoolManager.instance.ReturnObject(PoolType.Proj_PacketStream, gameObject);
        }
    }

    private bool CheckOutOfScreen()
    {
        Vector3 viewPos = Camera.main.WorldToViewportPoint(transform.position);
        return viewPos.x < 0 || viewPos.x > 1 || viewPos.y < 0 || viewPos.y > 1;
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Virus"))
        {
            other.GetComponent<VirusBehaviour>().GetDamage(damage);
            PoolManager.instance.ReturnObject(PoolType.Proj_PacketStream, gameObject);
        }
        else
        {
            PoolManager.instance.ReturnObject(PoolType.Proj_PacketStream, gameObject);
        }
    }
}
