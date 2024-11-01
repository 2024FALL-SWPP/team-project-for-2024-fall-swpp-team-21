using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class WeaponBehaviour : SelectableBehaviour
{

    protected PlayerController playerController;
    protected PlayerStat playerStat;
    protected ProjectileBehaviour projectile;
    protected int basicDamage;
    protected float basicAttackPeriod;
    protected float basicAttackRange;

    protected abstract IEnumerator Attack();
    protected int CalcProjectileDamage()
    {
        return basicDamage * playerStat.AttackPoint * (IsCritical() ? playerStat.CritAttackPoint : 100) / 10000;
    }
    protected float CalcAttackPeriod()
    {

        return basicAttackPeriod;
    }
    protected float CalcAttackRange()
    {
        // TODO
        return basicAttackRange;
    }


    protected bool IsCritical()
    {
        return Random.Range(0, 100) < playerStat.CritAttackProbability;
    }


}
