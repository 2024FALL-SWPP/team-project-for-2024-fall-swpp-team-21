using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cst = GameConstants;

public class PlayerController : MonoBehaviour
{
    public Action onPlayerEvade;

    public PlayerStatData playerStatData;
    public PlayerStatEventCaller statEventCaller;
    public PlayerStat playerStat = new PlayerStat();

    public SphereCollider sphereCollider;

    [SerializeField] private SFXPreset hitSFX;
    [SerializeField] private CanvasSoundPreset deathSFX;
    [SerializeField] private SFXPreset evadeSFX;

    private Rigidbody rb;
    private Animator animator;
    private bool isInvincible = false;

    private int debuffNum = 0;
    private float debuffDegree = 1.0f;

    private PlayerHitEffect playerHitEffect;
    private bool isGameOver = false;

    public void Initialize()
    {
        playerStat.Initialize(playerStatData, statEventCaller);
        statEventCaller.StatChangedHandler += OnStatChanged;

        playerStat.TakeSelectable(SelectableManager.instance.GetSelectableBehaviour("패킷 스트림"));
        // 경험치 획득 범위 초기화
        //sphereCollider = GetComponent<SphereCollider>();
        sphereCollider.radius = playerStat.ExpGainRange;

        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

        playerHitEffect = new PlayerHitEffect(gameObject);
    }


    private void FixedUpdate()
    {
        Move();
        rb.velocity = Vector3.zero;
    }

    private void Move()
    {
        if (isGameOver)
        {
            return;
        }

        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        float horizontalAbs = Mathf.Abs(horizontalInput);
        float verticalAbs = Mathf.Abs(verticalInput);

        // 두 방향키를 모두 뗀 후 3프레임이 지난 시점에 중립 판정
        if (horizontalAbs < Cst.DeadZoneSec
            && verticalAbs < Cst.DeadZoneSec)
        {
            animator.SetBool("b_IsMoving", false);
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

        animator.SetBool("b_IsMoving", moveDirection != Vector3.zero);

        //transform.Translate(playerStat.MoveSpeed * Time.deltaTime * moveDirection, Space.World);
        rb.MovePosition(transform.position + playerStat.MoveSpeed * debuffDegree * Time.deltaTime * moveDirection);
        //transform.rotation = Quaternion.LookRotation(moveDirection, Vector3.up);
        if (playerStat.MoveSpeed >= 0)
        {
            rb.MoveRotation(Quaternion.LookRotation(moveDirection, Vector3.up));
        }
        else
        {
            rb.MoveRotation(Quaternion.LookRotation(-moveDirection, Vector3.up));
        }
    }

    private void Die()
    {
        isGameOver = true;
        UISoundManager.instance.PlaySound(deathSFX.EnterSound);
        animator.updateMode = AnimatorUpdateMode.UnscaledTime;
        animator.Play("Dead");
        GameManager.instance.GameOver();
    }

    public void GetDamage(int damage)
    {
        // 만약 무적 프레임이 남아있다면 데미지를 받지 않음
        if (isInvincible || damage <= 0)
        {
            return;
        }

        StartCoroutine(BeInvincible());

        // 공격 회피
        // 무적을 만들어 주지 않으면 몬스터랑 비비면서 다음 프레임에 다시 공격 받음
        if (IsEvade())
        {
            // 회피 이펙트 추가용
            onPlayerEvade?.Invoke();
            return;
        }
        playerStat.CurrentHP -= ReduceDamage(damage);
        hitSFX.Play();

        Debug.Log("Player HP: " + playerStat.CurrentHP);
        playerHitEffect.PlayGetDamageEffect();
        if (playerStat.CurrentHP <= 0)
        {
            Die();
        }
    }

    private int ReduceDamage(int damage)
    {
        float reduceRate = 1f - (7 / (7 + playerStat.DefencePoint + 0.1f * Mathf.Pow(playerStat.DefencePoint, 2)));
        float reducingDamage = damage * reduceRate;
        int reducingDamageInt = (int) reducingDamage;
        float remain = reducingDamage - reducingDamageInt;
        if (UnityEngine.Random.Range(0f, 1f) < remain)
        {
            reducingDamageInt++;
        }
        return damage - reducingDamageInt;
    }

    private bool IsEvade()
    {
        float evadeDice = UnityEngine.Random.Range(0f, 1f);
        Debug.Log("Evade dice: " + evadeDice);
        return evadeDice < playerStat.EvadeProbability / 100f;
    }

    public void GetHeal(int heal)
    {
        playerStat.CurrentHP += heal;
        playerHitEffect.PlayGetHealEffect();
    }

    public void GetSelectable()
    {

    }

    public void GetExp(int exp)
    {
        Debug.Log("EXP gained: " + exp);
        playerStat.CurrentExp += exp * playerStat.ExpGainRatio / 100;
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
        else if (e.StatName == nameof(PlayerStat.HealthRezenPer10))
        {
            StartCoroutine(HPrezen());
        }
    }

    public void DebuffMoveSpeed(float value)
    {
        debuffNum++;
        debuffDegree = value;
    }

    public void RestoreMoveSpeed()
    {
        debuffNum--;
        if (debuffNum <= 0)
        {
            debuffNum = 0;
            debuffDegree = 1.0f;
        }
    }

    public void ReverseSpeed(float duration)
    {
        StartCoroutine(BuffMoveSpeedCoroutine(duration));
    }

    private IEnumerator BuffMoveSpeedCoroutine(float duration)
    {
        playerStat.MoveSpeed *= -1;
        yield return new WaitForSeconds(duration);
        playerStat.MoveSpeed *= -1;
    }

    private IEnumerator HPrezen()
    {
        float remainHP = 0;
        while (true)
        {
            yield return new WaitForSeconds(1);
            float hpRezenPerSecond = playerStat.HealthRezenPer10 / 10f;
            int hpZenInt = (int) hpRezenPerSecond;
            remainHP += hpRezenPerSecond - hpZenInt;
            if (remainHP > 1f)
            {
                hpZenInt++;
                remainHP -= 1f;
            }
            playerStat.CurrentHP += hpZenInt;
        }
    }
}
