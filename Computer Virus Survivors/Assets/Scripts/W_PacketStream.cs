using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class W_PacketStream : WeaponBehaviour
{
    protected override IEnumerator Attack()
    {
        // TODO
        yield return new WaitForSeconds(CalcAttackPeriod());
        Instantiate(projectile, player.transform.position, player.transform.rotation);
    }

    protected override void LevelUp()
    {
        level++;

        // TODO: Upgrade stats
    }
}
