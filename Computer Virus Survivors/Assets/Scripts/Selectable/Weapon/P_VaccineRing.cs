using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class P_VaccineRing : ProjectileBehaviour
{
    [SerializeField] private float spinSpeed;
    private float currentAngle;

    // Angle은 0 이상일 경우만 초기화
    public void Initialize(FinalWeaponData finalWeaponData, float angle)
    {
        base.Initialize(finalWeaponData);

        if (angle >= 0)
        {
            currentAngle = angle;
        }
        transform.rotation = Quaternion.identity;
    }

    public float GetCurrentAngle()
    {
        return currentAngle;
    }

    private void Update()
    {
        currentAngle += 2f * MathF.PI / finalWeaponData.attackPeriod * Time.deltaTime;
        transform.Rotate(Vector3.up, spinSpeed * Time.deltaTime);
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Virus"))
        {
            other.GetComponent<VirusBehaviour>().GetDamage(finalWeaponData.GetFinalDamage());
        }
    }
}
