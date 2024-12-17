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
    private float knockbackTime = 0f;
    private HitEffect hitEffect;
    private DissolveEffect dissolveEffect;
    private bool isDead;

    protected virtual void Awake()
    {
        hitEffect = new HitEffect(gameObject, virusData.knockbackColor);
        dissolveEffect = new DissolveEffect(gameObject);
    }

    protected virtual void Start()
    {
        player = GameManager.instance.Player;
        playerController = player.GetComponent<PlayerController>();
        rb = GetComponent<Rigidbody>();
    }

    protected virtual void OnEnable()
    {
        currentHP = virusData.maxHP;
        knockbackTime = 0f;
        isDead = false;
        dissolveEffect.Reset();
        gameObject.layer = LayerMask.NameToLayer("Virus");
    }


    protected void Move()
    {
#if !WEAPON_LAB
        if (knockbackTime <= 0)
        {
            if (!isDead)
            {
                Vector3 moveDirection = Vector3.ProjectOnPlane(
                (player.transform.position - transform.position).normalized,
                Vector3.up);
                //transform.Translate(virusData.moveSpeed * Time.deltaTime * moveDirection, Space.World);
                rb.MovePosition(transform.position + virusData.moveSpeed * Time.fixedDeltaTime * moveDirection);
                //transform.rotation = Quaternion.LookRotation(moveDirection);
                rb.MoveRotation(Quaternion.LookRotation(moveDirection));
            }
        }
        else // Knockback
        {
            knockbackTime = Math.Max(knockbackTime - Time.fixedDeltaTime, 0);
            if (Physics.Raycast(transform.position, -transform.forward, out RaycastHit hit,
                virusData.knockbackSpeed * Time.fixedDeltaTime, LayerMask.GetMask("Obstacle")))
            {
                return;
            }
            rb.MovePosition(transform.position + virusData.knockbackSpeed * Time.fixedDeltaTime * -transform.forward);
        }
#endif
    }

    public PoolType GetPoolType()
    {
        if (virusData != null)
        {
            return virusData.poolType;
        }
        return PoolType.None;
    }

    public virtual float GetVirusSize()
    {
        Collider coll = GetComponent<Collider>();

        if (coll is SphereCollider)
        {
            return ((SphereCollider) coll).radius * transform.localScale.x * 2;
        }
        else if (coll is CapsuleCollider)
        {
            CapsuleCollider capsule = (CapsuleCollider) coll;
            return Mathf.Max(capsule.radius, capsule.height / 2) * transform.localScale.x * 2;
        }
        else if (coll is BoxCollider)
        {
            BoxCollider box = (BoxCollider) coll;
            return Mathf.Max(box.size.x, box.size.z) * transform.localScale.x;
        }
        else
        {
            throw new Exception("Collider Type Error");
        }
    }

    protected virtual void Die()
    {
        isDead = true;
        gameObject.layer = LayerMask.NameToLayer("Ghost");
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

        OnDie = null;

        dissolveEffect.Play(() =>
        {
            PoolManager.instance.ReturnObject(virusData.poolType, gameObject);
        });

    }

    private Vector3 GetRandomPosition()
    {
        float randomRadius = UnityEngine.Random.Range(0f, 1f);
        Vector2 circlePoint = UnityEngine.Random.insideUnitCircle;
        return transform.position + new Vector3(circlePoint.x, 0, circlePoint.y).normalized * randomRadius;
    }

    // public void GetDamage(DamageData damageData)
    // {
    //     GetDamage(damageData.finalDamage, damageData.knockbackTime, damageData.weaponName);
    // }

    public void GetDamage(DamageData damageData)
    {
        if (damageData.finalDamage != 0)
        {

            currentHP -= damageData.finalDamage;
            PoolManager.instance.GetObject(PoolType.DamageIndicator)
                .GetComponent<DamageIndicator>().Initialize(damageData.finalDamage, transform.position, damageData.isCritical);
            hitEffect.Play();
            if (currentHP <= 0)
            {
                damageData.incrementKillCount();
                Die();
            }
            if (virusData.knockbackSpeed > 0)
            {
                this.knockbackTime += damageData.knockbackTime;
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
