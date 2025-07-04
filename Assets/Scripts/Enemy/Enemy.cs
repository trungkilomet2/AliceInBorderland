using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EnemyStats
{
    public float hp = 50f;
    public float damage = 10f;
    public float speed = 1f;

    public EnemyStats(EnemyStats stats)
    {
        this.hp = stats.hp;
        this.damage = stats.damage;
        this.speed = stats.speed;
    }

    internal void ApplyProgress(float progress)
    {
        this.hp *= progress;
        this.damage *= progress;
    }
}

public class Enemy : MonoBehaviour
{
    Transform targetDestination;
    public GameObject targetGameObject;
    CharacterCommonBehavior targetCharacter;
    private GameObject damageTextPrefab;
    Rigidbody2D rgb2d;

    public EnemyStats stats;
    [SerializeField] public EnemyData enemyData;

    // Insert By Trung
    public GameObject coin;
    public GameObject exp;
    private const float MAX_RATTING_DROPCOIN = 10f;
    private const float MAX_RATTING_DROPEXP = 100f;
    private bool isKnockedBack = false;
    private float knockbackTime = 0.2f;
    private float knockbackTimer = 0f;

    [SerializeField] private bool isRanged;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float fireRate = 2f;
    private float fireCooldown = 0f;



    private void Awake()
    {
        damageTextPrefab = Resources.Load<GameObject>("Prefabs/DamageText"); // Load the damage text prefab from Resources folder
        rgb2d = GetComponent<Rigidbody2D>();
    }

    public void SetTarget(GameObject target)
    {
        targetGameObject = target;
        if (targetGameObject != null)
        {
            targetDestination = targetGameObject.transform;
        }
    }

    public void KnockbackEnemy(Vector2 force)
    {
        isKnockedBack = true;
        knockbackTimer = knockbackTime;
        rgb2d.AddForce(force, ForceMode2D.Impulse);
    }

    private void FixedUpdate()
    {
        if (isKnockedBack)
        {
            knockbackTimer -= Time.fixedDeltaTime;
            if (knockbackTimer <= 0f)
            {
                isKnockedBack = false;
            }
            return;
        }
        if (targetDestination != null)
        {
            Vector3 direction1 = (targetDestination.position - transform.position).normalized;
            rgb2d.velocity = direction1 * stats.speed;
        }

        Vector3 direction = (targetDestination.position - transform.position).normalized;
        rgb2d.velocity = direction * stats.speed;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject == targetGameObject)
        {
            Attack();
        }
    }

    private void Attack()
    {
        Debug.Log("Enemy attacks!");
    }

    public void TakeDamage(float damage)
    {
        ShowDamageText(damage);
        stats.hp -= damage;
        if (stats.hp <= 0)
        {
            OnDeath();
        }
    }

    private void OnDeath()
    {
        StageEventManager stageManager = FindObjectOfType<StageEventManager>();
        if (stageManager != null && enemyData != null)
        {
            stageManager.OnEnemyDeath(enemyData);
        }

        DropCoin();
        DropEXP();

        Destroy(gameObject);
    }

    public void SetEnemyData(EnemyData data)
    {
        this.enemyData = data;
        this.isRanged = data.isRanged;
        this.bulletPrefab = data.bulletPrefab;
        this.fireRate = data.fireRate;
    }

    // insert by Trung
    public void DropCoin()
    {
        float randomDropCoin = UnityEngine.Random.Range(0, 100);
        if (randomDropCoin <= MAX_RATTING_DROPCOIN)
        {
            Vector3 localDie = rgb2d.transform.position;
            Instantiate(coin).transform.position = localDie;
        }
    }

    public void DropEXP()
    {
        float randomDropCoin = UnityEngine.Random.Range(0, 100);
        if (randomDropCoin <= MAX_RATTING_DROPEXP)
        {
            Vector3 localDie = rgb2d.transform.position;
            Instantiate(exp).transform.position = localDie;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Weapon"))
        {
            WeaponBase weapon = other.GetComponent<WeaponBase>();
            if (weapon != null)
            {
                TakeDamage(weapon.damage);
                if (!weapon.isThought)
                {
                    Destroy(other.gameObject);
                }
            }
        }
        else if (other.CompareTag("Player"))
        {
            targetCharacter = other.GetComponent<CharacterCommonBehavior>();
            if (targetCharacter != null)
            {
                targetCharacter.TakeDamage(stats.damage);
            }
        }
    }

    private void ShowDamageText(float damage)
    {
        Vector3 spawnPos = transform.position + new Vector3(0, 1f, 0); // bay lên đầu enemy
        GameObject dmgTextObj = Instantiate(damageTextPrefab, spawnPos, Quaternion.identity);
        DamageText dmgText = dmgTextObj.GetComponent<DamageText>();
        dmgText.SetDamage(damage);
    }

    internal void SetStats(EnemyStats stats)
    {
        this.stats = new EnemyStats(stats);
    }

    internal void UpdateStatsForProgress(float progress)
    {
        stats.ApplyProgress(progress);
    }

    private void Update()
    {
        if (isRanged && targetGameObject != null)
        {
            fireCooldown -= Time.deltaTime;
            if (fireCooldown <= 0f)
            {
                Fire();
                fireCooldown = fireRate;
            }
        }
    }

    private void Fire()
    {
        if (bulletPrefab == null || targetGameObject == null) return;

        Vector3 dir = (targetGameObject.transform.position - transform.position).normalized;
        GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);

        Bullet bulletScript = bullet.GetComponent<Bullet>();
        if (bulletScript != null)
        {
            bulletScript.direction = dir;
            bulletScript.damage = stats.damage;
        }
    }

}