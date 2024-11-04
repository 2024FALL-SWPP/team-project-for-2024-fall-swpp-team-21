using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

sealed public class W_PacketStream : WeaponBehaviour
{

    [SerializeField] private GameObject muzzle;
    [SerializeField] private float fluctuationRadius;

    protected override IEnumerator Attack()
    {
        while (true)
        {
            StartCoroutine(Shoot());
            yield return new WaitForSeconds(finalAttackPeriod);
        }
    }


    protected override void LevelUpEffect(int level)
    {
        switch (level)
        {
            case 1:
                // Do nothing
                break;
            case 2:
                BasicAttackPeriod -= 0.07f;
                break;
            case 3:
                BasicDamage += 5;
                break;
            case 4:
                BasicMultiProjectile += 1;
                break;
            case 5:
                BasicDamage += 5;
                break;
            case 6:
                BasicAttackPeriod -= 0.08f;
                break;
            case 7:
                BasicAttackPeriod -= 0.1f;
                break;
            case 8:
                BasicDamage += 10;
                break;
            case 9:
                BasicAttackPeriod -= 0.1f;
                BasicDamage += 10;
                break;
            default:
                break;
        }
    }

    private IEnumerator Shoot()
    {
        GameObject proj;
        for (int i = 0; i < finalMultiProjectile; i++)
        {
            Vector3 finalPosition = muzzle.transform.position + FireFluctuation();
            proj = PoolManager.instance.GetObject(projectilePool, finalPosition, muzzle.transform.rotation);
            proj.GetComponent<ProjectileBehaviour>().Initialize(finalDamage * (IsCritical() ? finalCritPoint : 100) / 100);
            yield return new WaitForSeconds(0.03f);
        }
    }

    private Vector3 FireFluctuation()
    {
        int x = Random.Range(int.MinValue, int.MaxValue);
        int y = Random.Range(int.MinValue, int.MaxValue);
        return new Vector3(x, y, 0) / int.MaxValue * fluctuationRadius;
    }

}
