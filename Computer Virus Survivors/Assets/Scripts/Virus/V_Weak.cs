using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class V_Weak : VirusBehaviour
{
    private void FixedUpdate()
    {
        Move();
        rb.velocity = Vector3.zero;
    }
}
