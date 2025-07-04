using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Skill2_Warrior : SkillBase
{
    public GameObject warriorClonePrefab;
    public float cloneSpeed = 10f;
    public float dashSpeed = 15f;
    public float damageOnPath = 20f;
    public float explosionDamage = 50f;
    public GameObject explosionEffect;

    private bool isDashing = false;
    private Vector3 dashTarget;
    private Collider2D col;
    private CharacterCommonBehavior characterCommonBehavior;
    private Collider2D myCollider;

    void Start()
    {
        col = GetComponent<Collider2D>();
        characterCommonBehavior = GetComponent<CharacterCommonBehavior>();
        myCollider = GetComponent<Collider2D>();
    }


    protected override void Activate()
    {
        if (isDashing) return; // Nếu đang dash thì không làm gì cả

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;

        Vector3 currentPos = transform.position;
        Vector3 direction = (mousePos - currentPos).normalized;
        float distanceToMouse = Vector3.Distance(currentPos, mousePos);
        float maxDashRadius = 10f; // Bán kính dash tối đa (có thể public để chỉnh trong Unity)

        // Nếu chuột vượt quá phạm vi cho phép, giới hạn lại target
        if (distanceToMouse > maxDashRadius)
        {
            mousePos = currentPos + direction * maxDashRadius;
        }

        // Spawn clone tại vị trí hiện tại
        GameObject clone = Instantiate(warriorClonePrefab, currentPos, Quaternion.identity);

        // Bắt đầu coroutine di chuyển clone và dash theo
        StartCoroutine(MoveCloneThenDash(clone, mousePos));
    }

    private IEnumerator MoveCloneThenDash(GameObject clone, Vector3 target)
{
    const float stopDist      = 0.1f;   // Khoảng cách coi như “đã tới”
    const float maxDashTime   = 2f;     // Thoát khẩn nếu kẹt
    const float checkRadius   = 0.3f;   // Bán kính quét va chạm
    float elapsed = 0f;
    bool  blocked = false;

    // ──────────────────────────────────────────────────────────
    while (clone && Vector3.Distance(clone.transform.position, target) > stopDist && elapsed < maxDashTime)
    {
        /* 1️⃣ Kiểm tra xem clone sắp chạm tường (tag "Block") */
        Collider2D[] hits = Physics2D.OverlapCircleAll(clone.transform.position, checkRadius);
        foreach (var h in hits)
        {
            if (h.CompareTag("Block"))
            {
                blocked = true;
                break;
            }
        }
        if (blocked) break;

        /* 2️⃣ Di chuyển clone */
        Vector3 dir = (target - clone.transform.position).normalized;
        clone.transform.position += dir * cloneSpeed * Time.deltaTime;

        elapsed += Time.deltaTime;
        yield return null;
    }
    // ──────────────────────────────────────────────────────────

    /* Nếu chạm tường hoặc clone mất -> huỷ clone & KHÔNG dash */
    if (blocked || clone == null)
    {
        if (clone) Destroy(clone);
        yield break;
    }

    /* Ép clone đứng chính xác mục tiêu */
    clone.transform.position = target;
    dashTarget = clone.transform.position;
    Destroy(clone);

    /* Bắt đầu dash nhân vật */
    isDashing = true;
    characterCommonBehavior.isDashing = true;
    if (myCollider) myCollider.enabled = false;
    yield return StartCoroutine(DashToTarget());

    // (DashToTarget kết thúc sẽ bật lại collider & flag)
}




    private IEnumerator DashToTarget()
    {
        Vector3 start = transform.position;
        Vector3 dir = (dashTarget - start).normalized;
        float dashDistance = Vector3.Distance(start, dashTarget);
        float traveled = 0f;

        while (traveled < dashDistance)
        {
            Vector3 move = dir * dashSpeed * Time.deltaTime;
            transform.position += move;
            traveled += move.magnitude;

            // Gây damage cho enemy trên đường
            RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, 0.5f, Vector2.zero);
            foreach (var hit in hits)
            {
                Enemy enemy = hit.collider.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.TakeDamage(damageOnPath);
                }
            }

            yield return null;
        }

        // Explosion on merge
        if (explosionEffect != null)
        {
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
        }

        // Gây damage AOE
        Collider2D[] aoeHits = Physics2D.OverlapCircleAll(transform.position, 2f);
        foreach (var col in aoeHits)
        {
            Enemy enemy = col.GetComponent<Enemy>();
            EnemyBase enemyBase = col.GetComponent<EnemyBase>();
            BossBase boss = col.GetComponent<BossBase>();
            if (enemy != null)
            {
                enemy.TakeDamage(explosionDamage);
            }
            if (enemyBase != null)
            {
                enemyBase.TakeDamage(explosionDamage);
            }
            if (boss != null)
            {
                boss.TakeDamage(explosionDamage);
            }
                
        }

        isDashing = false;
        characterCommonBehavior.isDashing = false;
        if (col != null) col.enabled = true;
    }

    private void OnEnable()
    {
        CharacterCommonBehavior.OnBlockedCollision += StopRolling;
    }

    private void OnDisable()
    {
        CharacterCommonBehavior.OnBlockedCollision -= StopRolling;
    }

    private void StopRolling()
    {
        if (isDashing)
        {
            Debug.Log("Va chạm Block, dừng dash.");
            isDashing = false;
            CancelSkill();
            // THAY ĐỔI: Tắt isDashing khi dash bị dừng do va chạm
            if (characterCommonBehavior != null)
            {
                characterCommonBehavior.isDashing = false;
            }
        }
    }
}

