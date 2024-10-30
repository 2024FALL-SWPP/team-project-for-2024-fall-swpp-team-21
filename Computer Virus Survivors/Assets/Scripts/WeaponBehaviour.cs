using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class WeaponBehaviour : SelectableBehaviour
{
    protected GameObject player;
    protected ProjectileBehaviour projectile;
    protected int damage;
    protected float attackPeriod;
    protected float attackRange;

    protected abstract IEnumerator Attack();
    protected int CalcProjectileDamage()
    {
        // TODO:
        return damage;
    }
    protected float CalcAttackPeriod()
    {
        // TODO
        return attackPeriod;
    }
    protected float CalcAttackRange()
    {
        // TODO
        return attackRange;
    }
    protected bool IsCritical()
    {
        // TODO
        return false;
    }
}
