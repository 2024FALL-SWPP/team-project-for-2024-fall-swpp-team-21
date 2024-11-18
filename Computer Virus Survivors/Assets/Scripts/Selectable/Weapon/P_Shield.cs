using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class P_Shield : ProjectileBehaviour
{
    private float currentAngle;
    private float rotateSpeed;

    public void Initialize(int damage, float angle, float rotateSpeed)
    {
        base.Initialize(damage);

        if (angle > 0)
        {
            currentAngle = angle;
        }
        this.rotateSpeed = rotateSpeed;
    }

    public float GetCurrentAngle()
    {
        return currentAngle;
    }

    private void Update()
    {
        currentAngle += rotateSpeed * Time.deltaTime;
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Virus"))
        {
            other.GetComponent<VirusBehaviour>().GetDamage(damage);
        }
    }
}
