using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class ProjectileBehaviour : MonoBehaviour
{
    protected Animator animator;
    protected int damage;

    protected Action<Collider> ProjEffect;
}
