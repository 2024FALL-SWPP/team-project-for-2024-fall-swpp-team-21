using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class P_Shield : ProjectileBehaviour
{
    [SerializeField] private float tick = 0.5f;
    [SerializeField] private float spinSpeed = 90.0f;
    private float currentAngle;
    private float rotateSpeed;
    private bool canDamage = true;

    // Angle은 0 이상일 경우만 초기화
    public void Initialize(int damage, float angle, float rotateSpeed)
    {
        base.Initialize(damage);

        if (angle >= 0)
        {
            currentAngle = angle;
        }
        this.rotateSpeed = rotateSpeed;
        transform.rotation = Quaternion.identity;
    }

    public float GetCurrentAngle()
    {
        return currentAngle;
    }

    private void Update()
    {
        currentAngle += rotateSpeed * Time.deltaTime;
        transform.Rotate(Vector3.up, spinSpeed * Time.deltaTime);

    }

    protected override void OnTriggerEnter(Collider other)
    {
        if (canDamage && other.CompareTag("Virus"))
        {
            other.GetComponent<VirusBehaviour>().GetDamage(damage);
            StartCoroutine(CoolDown());
        }
    }

    private IEnumerator CoolDown()
    {
        canDamage = false;
        yield return new WaitForSeconds(tick);
        canDamage = true;
    }
}
