using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill1_Warrior : SkillBase
{
    public float shieldDuration = 5f;              // Thời gian tồn tại của giáp
    public float maxAbsorbDamage = 100f;           // Lượng sát thương tối đa hấp thụ
    public float reflectRadius = 3f;               // Bán kính phản sát thương
    public float reflectDamageMultiplier = 1f;     // Tỉ lệ sát thương phản lại
    public GameObject shieldEffectPrefab;          // Hiệu ứng khi có giáp
    public GameObject reflectExplosionPrefab;      // Hiệu ứng khi phản dame

    private float shieldTimer = 0f;
    private float absorbedDamage = 0f;
    private bool isShieldActive = false;

    private GameObject shieldEffectInstance;
    private CharacterCommonBehavior character;

    public override void Awake()
    {
        base.Awake();
        character = GetComponent<CharacterCommonBehavior>();
    }

    protected override void Activate()
    {
        if (isShieldActive) return;

        isShieldActive = true;
        shieldTimer = shieldDuration;
        absorbedDamage = 0f;

        if (shieldEffectPrefab != null)
        {
            shieldEffectInstance = Instantiate(shieldEffectPrefab, transform);
        }

        // Debug.Log("Shield activated!");
    }

    void Update()
    {
        if (!isShieldActive) return;

        shieldTimer -= Time.deltaTime;

        if (shieldTimer <= 0f || absorbedDamage >= maxAbsorbDamage)
        {
            ReflectDamage();
            EndShield();
        }
    }

    public float OnAbsorbDamage(float incomingDamage)
    {
        if (!isShieldActive) return incomingDamage;

        float absorbable = Mathf.Min(incomingDamage, maxAbsorbDamage - absorbedDamage);
        absorbedDamage += absorbable;

        float leftover = incomingDamage - absorbable;

        if (absorbedDamage >= maxAbsorbDamage)
        {
            ReflectDamage();
            EndShield();
        }

        return leftover;
    }

    private void ReflectDamage()
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, reflectRadius);
        foreach (Collider2D col in enemies)
        {
            Enemy enemy = col.GetComponent<Enemy>();
            EnemyBase enemyBase = col.GetComponent<EnemyBase>();
            if (enemy != null)
            {
                enemy.TakeDamage(absorbedDamage * reflectDamageMultiplier);
                
            }
            if (enemyBase != null)
            {
                enemyBase.TakeDamage(absorbedDamage * reflectDamageMultiplier);
            }
        }

        TriggerExplosion();

        // Debug.Log("Reflected damage: " + absorbedDamage);
    }

    private void EndShield()
    {
        isShieldActive = false;
        absorbedDamage = 0f;

        if (shieldEffectInstance != null)
        {
            Destroy(shieldEffectInstance);
        }
    }

private void TriggerExplosion()
{
    if (reflectExplosionPrefab != null)
    {
        Instantiate(reflectExplosionPrefab, transform.position, Quaternion.identity);
    }
}

}
