using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageSkill2 : SkillBase
{
    public GameObject slowZonePrefab;
    public float slowZoneRadius = 0.5f; // <<<<<< Khôi phục lại bán kính mong muốn lớn hơn, ví dụ 5f
    public float slowPercentage = 0.5f; // Giảm 50% tốc độ
    public float damagePerSecond = 1f;

    // Bán kính "chuẩn" của Collider trên slowZonePrefab
    // Đảm bảo CircleCollider2D trên slowZonePrefab có Radius là giá trị này.
    // Ví dụ: Nếu visual của prefab có đường kính 1 đơn vị Unity, radius là 0.5f.
    // Nếu visual có đường kính 2 đơn vị Unity, radius là 1f.
    public float basePrefabColliderRadius = 0.5f; // <<<<<<<<<<< THÊM BIẾN NÀY VÀ CẤU HÌNH TRONG INSPECTOR

    private GameObject currentSlowZoneInstance;
    private HashSet<Enemy> enemiesInSlowZone = new HashSet<Enemy>();

    public override void Awake()
    {
        base.Awake();
        skillNum = SkillNum.Skill2;
        skillType = SkillType.Active;
        indicatorType = IndicatorType.Circle;

        cooldown = 15f;
        skillDuration = 10f;
        skillRange = 15f;
        // skillWidth cho indicator nên phản ánh đường kính cuối cùng của vùng ảnh hưởng
        skillWidth = slowZoneRadius * 2;
        skillDamage = damagePerSecond;
    }

    protected override void Activate()
    {
        Vector3 targetPosition = skillTransform != null ? skillTransform.position : transform.position;

        currentSlowZoneInstance = Instantiate(slowZonePrefab, targetPosition, Quaternion.identity);

        // Tính toán tỉ lệ scale để vùng làm chậm đạt bán kính mong muốn
        float scaleFactor = slowZoneRadius / basePrefabColliderRadius;
        currentSlowZoneInstance.transform.localScale = new Vector3(scaleFactor, scaleFactor, 1f);

        // Đảm bảo collider tồn tại và là trigger
        CircleCollider2D zoneCollider = currentSlowZoneInstance.GetComponent<CircleCollider2D>();
        if (zoneCollider == null)
        {
            zoneCollider = currentSlowZoneInstance.AddComponent<CircleCollider2D>();
            Debug.LogWarning("Slow Zone Prefab was missing a CircleCollider2D. Added one automatically.");
        }
        zoneCollider.isTrigger = true;

        // Bắt đầu coroutine xử lý hiệu ứng và thời gian tồn tại
        StartCoroutine(SlowZoneDurationCoroutine(skillDuration, currentSlowZoneInstance));
        StartCoroutine(ProcessSlowZoneEffects(currentSlowZoneInstance, skillDuration));
    }

    private IEnumerator SlowZoneDurationCoroutine(float duration, GameObject zoneInstance)
    {
        yield return new WaitForSeconds(duration);

        if (zoneInstance != null)
        {
            foreach (Enemy enemy in enemiesInSlowZone)
            {
                if (enemy != null)
                {
                    // Đảm bảo không chia cho 0
                    if (Mathf.Abs(1f - slowPercentage) > float.Epsilon)
                        enemy.stats.speed /= (1f - slowPercentage);
                }
            }
            enemiesInSlowZone.Clear();
            Destroy(zoneInstance);
            currentSlowZoneInstance = null;
        }
    }

    private IEnumerator ProcessSlowZoneEffects(GameObject zoneInstance, float duration)
    {
        float elapsed = 0f;
        float tickRate = 0.5f;

        CircleCollider2D zoneCollider = zoneInstance.GetComponent<CircleCollider2D>();
        if (zoneCollider == null)
        {
            Debug.LogError("Slow Zone Prefab must have a CircleCollider2D component!");
            yield break;
        }

        while (elapsed < duration)
        {
            if (zoneInstance == null) yield break;

            Vector2 zoneCenter = zoneInstance.transform.position + (Vector3)zoneCollider.offset;
            // Bán kính thực tế của collider sau khi GameObject đã được scale
            float actualZoneRadius = zoneCollider.radius * zoneInstance.transform.localScale.x;
            // Debug để kiểm tra bán kính thực tế của vùng làm chậm
            // Debug.Log($"Slow Zone actual radius: {actualZoneRadius}");

            Collider2D[] hitColliders = Physics2D.OverlapCircleAll(zoneCenter, actualZoneRadius);
            HashSet<Enemy> currentTickEnemies = new HashSet<Enemy>();

            foreach (Collider2D hit in hitColliders)
            {
                if (hit.CompareTag("Enemy"))
                {
                    Enemy enemy = hit.GetComponent<Enemy>();
                    EnemyBase enemyBase = hit.GetComponent<EnemyBase>();
                    if (enemy != null)
                    {
                        currentTickEnemies.Add(enemy);

                        if (!enemiesInSlowZone.Contains(enemy))
                        {
                            enemy.stats.speed *= (1f - slowPercentage);
                            enemiesInSlowZone.Add(enemy);
                            Debug.Log($"Enemy {enemy.name} entered slow zone and speed reduced to {enemy.stats.speed}");
                        }

                        if (damagePerSecond > 0f)
                        enemyBase.TakeDamage(damagePerSecond * tickRate);
                        enemy.TakeDamage(damagePerSecond * tickRate);
                    }
                }
            }

            List<Enemy> enemiesToRemove = new List<Enemy>();
            foreach (Enemy enemy in enemiesInSlowZone)
            {
                if (enemy == null || !currentTickEnemies.Contains(enemy))
                {
                    if (enemy != null)
                    {
                        if (Mathf.Abs(1f - slowPercentage) > float.Epsilon)
                            enemy.stats.speed /= (1f - slowPercentage);
                        Debug.Log($"Enemy {enemy.name} left slow zone and speed restored to {enemy.stats.speed}");
                    }
                    enemiesToRemove.Add(enemy);
                }
            }

            foreach (Enemy enemyToRemove in enemiesToRemove)
                enemiesInSlowZone.Remove(enemyToRemove);

            yield return new WaitForSeconds(tickRate);
            elapsed += tickRate;
        }

        foreach (Enemy enemy in enemiesInSlowZone)
        {
            if (enemy != null)
            {
                if (Mathf.Abs(1f - slowPercentage) > float.Epsilon)
                    enemy.stats.speed /= (1f - slowPercentage);
            }
        }
        enemiesInSlowZone.Clear();
    }

    // (Tùy chọn) Hàm Debug để trực quan hóa vùng OverlapCircle trong Scene view
    void OnDrawGizmos()
    {
        if (Application.isPlaying && currentSlowZoneInstance != null)
        {
            CircleCollider2D zoneCollider = currentSlowZoneInstance.GetComponent<CircleCollider2D>();
            if (zoneCollider != null)
            {
                Gizmos.color = Color.cyan;
                Vector2 zoneCenter = currentSlowZoneInstance.transform.position + (Vector3)zoneCollider.offset;
                float actualZoneRadius = zoneCollider.radius * currentSlowZoneInstance.transform.localScale.x;
                Gizmos.DrawWireSphere(zoneCenter, actualZoneRadius);
            }
        }
        else if (!Application.isPlaying && slowZonePrefab != null)
        {
            // Vẽ Gizmo đại diện cho vùng khi đang ở chế độ Editor
            Gizmos.color = Color.yellow;
            // Tính toán scale factor để hiển thị đúng kích thước trong Editor
            float editorScaleFactor = (slowZoneRadius * 2) / (basePrefabColliderRadius * 2);
            Gizmos.DrawWireSphere(transform.position, slowZoneRadius); // Vẽ dựa trên slowZoneRadius bạn muốn
        }
    }
}