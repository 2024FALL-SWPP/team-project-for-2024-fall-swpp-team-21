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

    private bool isInvincible = false;
    private SphereCollider sphereCollider;

    public GameObject spawnManager; // Temp: 나중에 삭제

#if WEAPON_TEST
    public WeaponBehaviour weapon;
#endif
    private void Start()
    {
        playerStat.Initialize(playerStatData, statEventCaller);
        statEventCaller.StatChanged += OnStatChanged;

        // 경험치 획득 범위 초기화
        sphereCollider = GetComponent<SphereCollider>();
        sphereCollider.radius = playerStat.ExpGainRange;
    }

    private void Update()
    {
        Move();

        // Temp: 스폰 임시로 구현
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            spawnManager.GetComponent<SpawnManager>().Spawn(PoolType.Virus_Weak);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            spawnManager.GetComponent<SpawnManager>().Spawn(PoolType.Virus_Trojan);
        }
#if WEAPON_TEST
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Weapon Test");
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
        // TODO: Game Over
    }

    public void GetDamage(int damage)
    {
        // 만약 무적 프레임이 남아있다면 데미지를 받지 않음
        if (isInvincible)
        {
            return;
        }

        StartCoroutine(BeInvincible());
        playerStat.CurrentHP -= damage;
        Debug.Log("Player HP: " + playerStat.CurrentHP);
        if (playerStat.CurrentHP <= 0)
        {
            Die();
        }
    }

    public void GetSelectable()
    {

    }

    public void GetExp(int exp)
    {
        playerStat.CurrentExp += exp * playerStat.ExpGainRatio / 100;
        Debug.Log("Player EXP: " + playerStat.CurrentExp);
        // TODO: Level up
    }

    private void OnTriggerEnter(Collider other)
    {

    }

    private IEnumerator BeInvincible()
    {
        isInvincible = true;
        yield return new WaitForSeconds(playerStat.InvincibleFrame / 60.0f);
        isInvincible = false;
    }

    public void OnStatChanged(object sender, StatChangedEventArgs e)
    {
        if (e.StatName == nameof(PlayerStat.ExpGainRange))
        {
            sphereCollider.radius = playerStat.ExpGainRange;
        }
    }
}
