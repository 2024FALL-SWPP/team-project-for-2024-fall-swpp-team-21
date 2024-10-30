using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirusWeak : VirusBehaviour
{
    // // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        maxHP = 1;
        currentHP = maxHP;
        moveSpeed = 1.5f;
        dropExp = 100;
        contactDamage = 5;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }
}
