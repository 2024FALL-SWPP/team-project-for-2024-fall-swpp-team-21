using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f; // Movement speed
    public float inputBufferTime = 0.05f; // Time buffer to detect simultaneous input

    private Vector3 moveDirection;

    // Input buffers for key timing
    private float buffer = 0f;
    private bool buffered = false;

    void Start()
    {

    }

    void Update()
    {
        HandleBufferedMovement();
    }

    void HandleBufferedMovement()
    {
        if (buffered && buffer > 0)
        {
            buffer -= Time.deltaTime;
            return;
        }

        float moveX = Input.GetAxisRaw("Horizontal"); // A/D or Left/Right keys
        float moveZ = Input.GetAxisRaw("Vertical");   // W/S or Up/Down keys

        if (!buffered)
        {
            // If there is horizontal input, start or reset the buffer timer for horizontal movement
            if (moveX != 0 && moveZ == 0)
            {
                buffer = inputBufferTime;
                buffered = true;
                return;
            }

            // If there is vertical input, start or reset the buffer timer for vertical movement
            if (moveZ != 0 && moveX == 0)
            {
                buffer = inputBufferTime;
                buffered = true;
                return;
            }
        }

        if (buffered && buffer <= 0)
        {
            buffered = false;
        }

        // Move only in the direction of the active input
        moveDirection = new Vector3(moveX, 0, moveZ).normalized;

        // Move the character smoothly using CharacterController
        if (moveDirection != Vector3.zero)
        {
            transform.Translate(moveDirection * moveSpeed * Time.deltaTime, Space.World);
            transform.rotation = Quaternion.LookRotation(moveDirection);
        }
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
