using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class P_VaccineRing : PlayerProjectileBehaviour
{
    [SerializeField] private float spinSpeed;
    private Transform rotationCenter;
    private float angle;

    // Angle은 0 이상일 경우만 초기화
    public void Initialize(FinalWeaponData finalWeaponData, Transform rotationCenter, float angle)
    {
        base.Initialize(finalWeaponData);

        if (this.rotationCenter == null)
        {
            this.rotationCenter = rotationCenter;

            SphereCollider collider = GetComponent<SphereCollider>();
            collider.center = new Vector3(collider.center.x, -rotationCenter.position.y, collider.center.z);
        }
        this.angle = angle;
        transform.rotation = Quaternion.identity;
    }

    private void Update()
    {
        Vector2 circlePoint = finalWeaponData.attackRange * new Vector2(MathF.Cos(angle), MathF.Sin(angle));
        transform.position = rotationCenter.position + new Vector3(circlePoint.x, 0, circlePoint.y);
        transform.Rotate(Vector3.up, spinSpeed * Time.deltaTime);

        angle += 2f * MathF.PI / finalWeaponData.attackPeriod * Time.deltaTime;
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Virus"))
        {
            // other.GetComponent<VirusBehaviour>().GetDamage(finalWeaponData.GetFinalDamage(), finalWeaponData.knockbackTime);
            other.GetComponent<VirusBehaviour>().GetDamage(finalWeaponData.GetDamageData());
        }
    }
}
