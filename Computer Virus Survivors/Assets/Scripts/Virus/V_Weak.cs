using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class V_Weak : VirusBehaviour
{
    // Temp
    // private void Start()
    // {
    //     maxHP = 10;
    //     currentHP = maxHP;
    //     moveSpeed = 1.5f;
    //     dropExp = 100;
    //     contactDamage = 5;
    // }

    private void FixedUpdate()
    {
        Move();
        rb.velocity = Vector3.zero;
    }
}
