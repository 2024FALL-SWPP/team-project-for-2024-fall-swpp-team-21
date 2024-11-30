using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirusBehaviour : MonoBehaviour
{

    public event Action<VirusBehaviour> OnDie;

    [SerializeField] protected VirusData virusData;
    protected GameObject player;
    protected PlayerController playerController;
    protected Rigidbody rb;


    protected int currentHP;

    protected virtual void Start()
    {
        player = GameManager.instance.Player;
        playerController = player.GetComponent<PlayerController>();
        rb = GetComponent<Rigidbody>();
    }

    protected virtual void OnEnable()
    {
        currentHP = virusData.maxHP;
    }


    protected void Move()
    {
#if !WEAPON_LAB
        Vector3 moveDirection = Vector3.ProjectOnPlane(
            (player.transform.position - transform.position).normalized,
            Vector3.up);
        //transform.Translate(virusData.moveSpeed * Time.deltaTime * moveDirection, Space.World);
        rb.MovePosition(transform.position + virusData.moveSpeed * Time.deltaTime * moveDirection);
        //transform.rotation = Quaternion.LookRotation(moveDirection);
        rb.MoveRotation(Quaternion.LookRotation(moveDirection));
#endif
    }

    protected virtual void Die()
    {
        OnDie?.Invoke(this);

        if (virusData.dropExp > 0)
        {
            GameObject expGem = PoolManager.instance.GetObject(PoolType.ExpGem, transform.position, transform.rotation);
            expGem.GetComponent<ExpGem>().Initialize(virusData.dropExp);
        }

        if (!virusData.dropTable.IsEmpty())
        {
            PoolManager.instance.GetObject(virusData.dropTable.GetDropItem(), GetRandomPosition(), transform.rotation);
        }

        PoolManager.instance.ReturnObject(virusData.poolType, gameObject);
    }

    private Vector3 GetRandomPosition()
    {
        float randomRadius = UnityEngine.Random.Range(0f, 1f);
        Vector2 circlePoint = UnityEngine.Random.insideUnitCircle;
        return transform.position + new Vector3(circlePoint.x, 0, circlePoint.y).normalized * randomRadius;
    }

    public void GetDamage(int damage)
    {
        if (damage != 0)
        {

            currentHP -= damage;
            PoolManager.instance.GetObject(PoolType.DamageIndicator)
                .GetComponent<DamageIndicator>().Initialize(damage, transform.position);
            if (currentHP <= 0)
            {
                Die();
            }

        }
    }

    protected void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerController.GetDamage(virusData.contactDamage);
        }
    }

    protected void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerController.GetDamage(virusData.contactDamage);
        }
    }
}

[Serializable]
public class DropTable
{
    [Serializable]
    public class DropElement
    {
        public PoolType dropItem;
        public int dropRate;
    }

    public DropElement[] dropElements;

    public PoolType GetDropItem()
    {
        int totalRate = 0;
        foreach (DropElement element in dropElements)
        {
            totalRate += element.dropRate;
        }

        int randomValue = UnityEngine.Random.Range(0, totalRate);
        int accumulatedRate = 0;
        foreach (DropElement element in dropElements)
        {
            accumulatedRate += element.dropRate;
            if (randomValue < accumulatedRate)
            {
                return element.dropItem;
            }
        }

        throw new Exception("DropTable Error");
    }

    public bool IsEmpty()
    {
        return dropElements.Length == 0;
    }
}