using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class W_PacketStream : WeaponBehaviour
{

    [SerializeField] private GameObject muzzle;
    [SerializeField] private float fluctuationRadius;

    // TODO : 투사체 배율
    protected override IEnumerator Attack()
    {
        ProjectileBehaviour projectile;
        while (true)
        {
            Vector3 finalPosition = muzzle.transform.position + FireFluctuation();
            projectile = Instantiate(this.projectile, finalPosition, muzzle.transform.rotation);
            projectile.Initialize(finalDamage * (IsCritical() ? finalCritPoint : 100) / 100);
            yield return new WaitForSeconds(finalAttackPeriod);
        }
    }


    protected override void LevelUp()
    {
        level++;
        // TODO: Upgrade stats
    }


    private Vector3 FireFluctuation()
    {
        int x = Random.Range(int.MinValue, int.MaxValue);
        int y = Random.Range(int.MinValue, int.MaxValue);
        return new Vector3(x, y, 0) / int.MaxValue * fluctuationRadius;
    }

}
