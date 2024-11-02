using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cst = GameConstants;

public class PlayerController : MonoBehaviour
{

    public PlayerStatData playerStatData;
    public PlayerStatEventCaller statEventCaller;

    public PlayerStat playerStat = new PlayerStat();

#if WEAPON_TEST
    public WeaponBehaviour weapon;
#endif
    private void Start()
    {
        playerStat.Initialize(playerStatData, statEventCaller);
    }

    private void Update()
    {
        Move();
#if WEAPON_TEST
        if (Input.GetKeyDown(KeyCode.Space))
        {
            weapon.GetSelectable(this);
        }
#endif
    }

    private void Move()
    {

        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        float horizontalAbs = Mathf.Abs(horizontalInput);
        float verticalAbs = Mathf.Abs(verticalInput);

        // 두 방향키를 모두 뗀 후 3프레임이 지난 시점에 중립 판정
        if (horizontalAbs < Cst.DeadZoneSec
            && verticalAbs < Cst.DeadZoneSec)
        {
            return;
        }

        // 8-axis movement
        if (horizontalAbs - verticalAbs > Cst.ThresholdSec)
        {
            // 수직 방향키를 뗀 후 3프레임 이상 지나면 수직 속력 0
            verticalInput = 0;
        }
        else if (verticalAbs - horizontalAbs > Cst.ThresholdSec)
        {
            // 수평 방향키를 뗀 후 3프레임 이상 지나면 수평 속력 0
            horizontalInput = 0;
        }
        else
        {
            // 각 축에 대해 가능한 속력을 {-1, 0, 1}로 제한하여 8축의 이산 방향 설정
            if (verticalAbs > .1f)
            {
                verticalInput /= verticalAbs;
            }

            if (horizontalAbs > .1f)
            {
                horizontalInput /= horizontalAbs;
            }
        }

        Vector3 moveDirection = new Vector3(horizontalInput, 0, verticalInput).normalized;

        transform.Translate(playerStat.MoveSpeed * Time.deltaTime * moveDirection, Space.World);
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
