using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_Beam : PlayerProjectileBehaviour
{
    [SerializeField] private float speed = 10f;
    private GameObject lightBeam;
    private Vector3 targetPos;

    public void Initialize(FinalWeaponData finalWeaponData, Vector3 target)
    {
        base.Initialize(finalWeaponData);

        if (lightBeam == null)
        {
            lightBeam = GetComponentInChildren<MeshRenderer>().gameObject;
        }
        this.targetPos = target;
        lightBeam.SetActive(true);
    }

    private void Update()
    {
        if (targetPos != null)
        {
            transform.LookAt(targetPos);
        }

        transform.Translate(speed * Time.deltaTime * Vector3.forward);

        if (CheckOutOfScreen())
        {
            PoolManager.instance.ReturnObject(PoolType.Proj_Beam, gameObject);
        }
    }

    protected override void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Virus"))
        {
            // other.GetComponent<VirusBehaviour>().GetDamage(finalWeaponData.GetFinalDamage(), finalWeaponData.knockbackTime);
            other.GetComponent<VirusBehaviour>().GetDamage(finalWeaponData.GetDamageData(out bool isCritical));
            PlayAttackEffect(other.ClosestPoint(transform.position) + new Vector3(0, transform.position.y, 0), Quaternion.identity, isCritical);
        }

        PoolManager.instance.ReturnObject(PoolType.Proj_Beam, gameObject);
    }

}
