using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f; // Movement speed

    void Start()
    {

    }

    void Update()
    {
        Move();
    }

    void Move()
    {
        const int thresholdFrame = 3; 

        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        float horizontalAbs = Mathf.Abs(horizontalInput);
        float verticalAbs = Mathf.Abs(verticalInput);

        if(horizontalAbs < 0.6f && verticalAbs < 0.6f) {return;}

        if(horizontalAbs - verticalAbs > thresholdFrame + .1f)
        {
            verticalInput = 0;
        }
        else if(verticalAbs - horizontalAbs > thresholdFrame + .1f)
        {
            horizontalInput = 0;
        }
        else
        {
            if(verticalAbs > .1f)
            {
                verticalInput /= verticalAbs;
            }
            
            if(horizontalAbs > .1f)
            {
                horizontalInput /= horizontalAbs;
            }
        }

        Vector3 moveDirection = new Vector3(horizontalInput, 0, verticalInput).normalized;

        transform.Translate(moveSpeed * Time.deltaTime * moveDirection, Space.World);
        transform.rotation = Quaternion.LookRotation(moveDirection, Vector3.up);

    }

    private void Die()
    {

    }

    private void GetDamage()
    {

    }

    public void GetSelectable()
    {

    }

    public void GetExp()
    {

    }

    private void OnTriggerEnter(Collider other)
    {

    }
}
