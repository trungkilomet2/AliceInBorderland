using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageSkill2 : SkillBase
{
    public GameObject slowZonePrefab;
    public float slowZoneRadius = 5f; // <<<<<< Khôi phục lại bán kính mong muốn lớn hơn, ví dụ 5f
    public float slowPercentage = 0.5f; // Giảm 50% tốc độ
    public float damagePerSecond = 1f;

    // Bán kính "chuẩn" của Collider trên slowZonePrefab
    // Đảm bảo CircleCollider2D trên slowZonePrefab có Radius là giá trị này.
    // Ví dụ: Nếu visual của prefab có đường kính 1 đơn vị Unity, radius là 0.5f.
    // Nếu visual có đường kính 2 đơn vị Unity, radius là 1f.
    public float basePrefabColliderRadius = 0.5f; // <<<<<<<<<<< THÊM BIẾN NÀY VÀ CẤU HÌNH TRONG INSPECTOR

    private GameObject currentSlowZoneInstance;
    private HashSet<Enemy> enemiesInSlowZone = new HashSet<Enemy>();
    private HashSet<EnemyBase> enemyBasesInSlowZone = new HashSet<EnemyBase>(); // Thêm HashSet cho EnemyBase

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
        skillDamage = damagePerSecond; // damagePerSecond sẽ là skillDamage cho kỹ năng này
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
        // Đảm bảo bán kính của collider trên prefab được đặt đúng basePrefabColliderRadius ban đầu
        // Nếu prefab đã có collider với radius khác, hãy đảm bảo rằng basePrefabColliderRadius phản ánh nó
        // hoặc đặt lại zoneCollider.radius = basePrefabColliderRadius; ở đây.
        // Tuy nhiên, việc scale transform là cách linh hoạt hơn.

        // Bắt đầu coroutine xử lý hiệu ứng và thời gian tồn tại
        StartCoroutine(SlowZoneDurationCoroutine(skillDuration, currentSlowZoneInstance));
        StartCoroutine(ProcessSlowZoneEffects(currentSlowZoneInstance, skillDuration));
    }

    private IEnumerator SlowZoneDurationCoroutine(float duration, GameObject zoneInstance)
    {
        yield return new WaitForSeconds(duration);

        if (zoneInstance != null)
        {
            // Khôi phục tốc độ cho tất cả Enemy đang bị làm chậm
            foreach (Enemy enemy in enemiesInSlowZone)
            {
                if (enemy != null)
                {
                    if (Mathf.Abs(1f - slowPercentage) > float.Epsilon) // Đảm bảo không chia cho 0
                        enemy.stats.speed /= (1f - slowPercentage);
                }
            }
            enemiesInSlowZone.Clear(); // Xóa sạch danh sách

            // Nếu bạn cũng làm chậm EnemyBase, bạn cũng cần một logic tương tự ở đây
            // (Hiện tại code bạn chỉ làm chậm Enemy, không phải EnemyBase)
            // Nếu EnemyBase cũng có stats.speed, bạn sẽ cần:
            // foreach (EnemyBase eb in enemyBasesInSlowZone) { /* Restore speed */ }
            // enemyBasesInSlowZone.Clear();

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
            if (zoneInstance == null) yield break; // Dừng nếu vùng làm chậm đã bị phá hủy

            Vector2 zoneCenter = (Vector2)zoneInstance.transform.position + zoneCollider.offset;
            // Bán kính thực tế của collider sau khi GameObject đã được scale
            float actualZoneRadius = zoneCollider.radius * zoneInstance.transform.localScale.x;

            HashSet<Enemy> currentTickEnemies = new HashSet<Enemy>();
            HashSet<EnemyBase> currentTickEnemyBases = new HashSet<EnemyBase>(); // HashSet cho EnemyBase trong tick hiện tại

            Collider2D[] hitColliders = Physics2D.OverlapCircleAll(zoneCenter, actualZoneRadius);

            foreach (Collider2D hit in hitColliders)
            {
                if (hit.CompareTag("Enemy"))
                {
                    // Ưu tiên kiểm tra và xử lý Enemy
                    Enemy enemy = hit.GetComponent<Enemy>();
                    if (enemy != null)
                    {
                        currentTickEnemies.Add(enemy); // Thêm vào danh sách kẻ địch đang trong vùng này

                        // Áp dụng hiệu ứng làm chậm nếu chưa bị làm chậm
                        if (!enemiesInSlowZone.Contains(enemy))
                        {
                            enemy.stats.speed *= (1f - slowPercentage);
                            enemiesInSlowZone.Add(enemy);
                            Debug.Log($"Enemy {enemy.name} entered slow zone and speed reduced to {enemy.stats.speed}");
                        }

                        // Gây sát thương cho Enemy
                        if (damagePerSecond > 0f)
                            enemy.TakeDamage(damagePerSecond * tickRate);

                        continue; // Đã xử lý GameObject này là Enemy, bỏ qua kiểm tra EnemyBase cho GameObject này
                    }

                    // Nếu không phải Enemy, thử kiểm tra EnemyBase (áp dụng sát thương thôi, không làm chậm nếu EnemyBase không có stats.speed)
                    EnemyBase enemyBase = hit.GetComponent<EnemyBase>();
                    if (enemyBase != null)
                    {
                        currentTickEnemyBases.Add(enemyBase); // Thêm vào danh sách EnemyBase đang trong vùng này

                        // Gây sát thương cho EnemyBase
                        if (damagePerSecond > 0f)
                            enemyBase.TakeDamage(damagePerSecond * tickRate);

                        // Không có continue; ở đây vì chúng ta đã kiểm tra Enemy trước.
                        // Nếu Enemy là null, chúng ta mới đến đây.
                    }
                    else
                    {
                        // Debug nếu có tag Enemy nhưng không có Enemy hay EnemyBase component
                        Debug.LogWarning($"GameObject with tag 'Enemy' on {hit.name} does not have an Enemy or EnemyBase component!");
                    }
                }
            }

            // Xử lý kẻ địch rời khỏi vùng làm chậm (chỉ cho Enemy vì logic làm chậm đang ở đó)
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

            // Tương tự, nếu bạn làm chậm EnemyBase, bạn sẽ cần logic loại bỏ tương tự
            // List<EnemyBase> enemyBasesToRemove = new List<EnemyBase>();
            // foreach (EnemyBase eb in enemyBasesInSlowZone) { /* Check and remove */ }
            // foreach (EnemyBase ebToRemove in enemyBasesToRemove) { /* Remove */ }


            yield return new WaitForSeconds(tickRate);
            elapsed += tickRate;
        }

        // Đảm bảo khôi phục tốc độ cho bất kỳ Enemy nào còn lại khi kỹ năng kết thúc
        foreach (Enemy enemy in enemiesInSlowZone)
        {
            if (enemy != null)
            {
                if (Mathf.Abs(1f - slowPercentage) > float.Epsilon)
                    enemy.stats.speed /= (1f - slowPercentage);
            }
        }
        enemiesInSlowZone.Clear(); // Xóa sạch sau khi kết thúc
        // Nếu EnemyBase cũng bị làm chậm, cũng cần clear enemyBasesInSlowZone
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
                Vector2 zoneCenter = (Vector2)currentSlowZoneInstance.transform.position + zoneCollider.offset;
                float actualZoneRadius = zoneCollider.radius * currentSlowZoneInstance.transform.localScale.x;
                Gizmos.DrawWireSphere(zoneCenter, actualZoneRadius);
            }
        }
        else if (!Application.isPlaying) // Khi không ở chế độ Play, vẽ dựa trên slowZoneRadius của skill
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, slowZoneRadius);
        }
    }
}