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
        const int thresholdFrame = 3;       // 대각선으로 움직이다 한 쪽 방향키를 뗐을 때 대각선을 유지하는 유예 프레임
        const float inputUnitPerSec = .2f;  // 방향키를 뗐을 때 axis input 감소량 (input manager -> gravity / 60)


        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        float horizontalAbs = Mathf.Abs(horizontalInput);
        float verticalAbs = Mathf.Abs(verticalInput);

        // 두 방향키를 모두 뗀 후 3프레임이 지난 시점에 중립 판정
        if(horizontalAbs < .6f && verticalAbs < .6f) {return;}

        // 8-axis movement
        if(horizontalAbs - verticalAbs > thresholdFrame * inputUnitPerSec + .1f)
        {
            // 수직 방향키를 뗀 후 3프레임 이상 지나면 수직 속력 0
            verticalInput = 0;
        }
        else if(verticalAbs - horizontalAbs > thresholdFrame * inputUnitPerSec + .1f)
        {
            // 수평 방향키를 뗀 후 3프레임 이상 지나면 수평 속력 0
            horizontalInput = 0;
        }
        else
        {
            // 각 축에 대해 가능한 속력을 {-1, 0, 1}로 제한하여 8축의 이산 방향 설정
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

    public void GetDamage(int damage)
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
