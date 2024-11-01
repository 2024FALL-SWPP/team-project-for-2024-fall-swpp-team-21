using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class P_PacketStream : ProjectileBehaviour
{

    private void OnEnable()
    {
        StartCoroutine(LifeTime());
    }

    private IEnumerator LifeTime()
    {
        yield return new WaitForSeconds(3);
        Destroy(gameObject);
    }

    private void Update()
    {
        transform.Translate(10 * Time.deltaTime * Vector3.forward);
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
