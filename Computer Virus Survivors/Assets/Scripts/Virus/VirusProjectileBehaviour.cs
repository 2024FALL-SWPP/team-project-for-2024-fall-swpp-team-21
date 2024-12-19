using System;
using UnityEngine;

public abstract class VirusProjectileBehaviour : ProjectileBehaviour
{
    protected int damage;

    public virtual void Initialize(int damage)
    {
        this.damage = damage;
    }
}
