using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageSkill3 : SkillBase
{
    public GameObject explosionEffectPrefab;
    public int numberOfExplosions = 3;

    public float randomSpawnRadius = 10f;
    public float minSpawnRadius = 5f;

    public float explosionSize = 1f; // Bán kính tương đối so với bán kính gốc của collider trên prefab
    public float damageTickInterval = 0.5f;

    public override void Awake()
    {
        base.Awake();
        skillType = SkillType.Passive; // Đây là kỹ năng kích hoạt thụ động? Hay là Active? Kiểm tra lại
        // Nếu là Active, cần setup cooldown, duration,...
        // Nếu là Passive, cần logic để nó tự kích hoạt (ví dụ: sau mỗi X giây, hoặc khi đạt Y điều kiện)
        // Hiện tại bạn đang StartCoroutine(SummonRandomExplosions()); trong Activate, nên nó hoạt động như Active.
        // Đặt skillDuration = thời gian mà mỗi vụ nổ tồn tại và gây sát thương
        skillDuration = 2f; // Ví dụ: mỗi vụ nổ tồn tại 2 giây
    }

    protected override void Activate()
    {
        StartCoroutine(SummonRandomExplosions());

        if (skillSound != null && GetAudio() != null)
        {
            GetAudio()?.PlaySoundClip(skillSound);
        }
    }

    private IEnumerator SummonRandomExplosions()
    {
        for (int i = 0; i < numberOfExplosions; i++)
        {
            Vector3 explosionPosition;

            float actualMinSpawnRadius = Mathf.Min(minSpawnRadius, randomSpawnRadius);
            float actualMaxSpawnRadius = Mathf.Max(minSpawnRadius, randomSpawnRadius);

            Vector2 randomDirection = Random.insideUnitCircle.normalized;
            float distance = Random.Range(actualMinSpawnRadius, actualMaxSpawnRadius);

            explosionPosition = transform.position + new Vector3(randomDirection.x, randomDirection.y, 0) * distance;

            if (explosionEffectPrefab != null)
            {
                GameObject explosionInstance = Instantiate(explosionEffectPrefab, explosionPosition, Quaternion.identity);

                CircleCollider2D explosionCollider = explosionInstance.GetComponent<CircleCollider2D>();
                if (explosionCollider != null)
                {
                    // Thay đổi bán kính của collider trực tiếp
                    // Không nhân với scale, mà thay đổi radius sau đó scale GameObject nếu muốn
                    // Hoặc scale GameObject và sau đó OverlapCircleAll sẽ tính đúng
                    // Ở đây tôi giữ nguyên logic của bạn: thay đổi radius.
                    // Nếu bạn muốn scale GameObject, bạn sẽ cần một biến basePrefabColliderRadius như MageSkill2
                    explosionCollider.radius *= explosionSize;
                    // Đảm bảo collider là trigger
                    explosionCollider.isTrigger = true;
                }
                // Điều chỉnh scale của GameObject nếu muốn thay đổi kích thước hiển thị
                // explosionInstance.transform.localScale = Vector3.one * someVisualScaleFactor;

                StartCoroutine(DealContinuousExplosionDamage(explosionInstance, skillDuration));

                Destroy(explosionInstance, skillDuration); // Đúng
            }

            yield return new WaitForSeconds(0.2f); // Khoảng thời gian giữa các vụ nổ
        }
    }

    private IEnumerator DealContinuousExplosionDamage(GameObject explosionGameObject, float duration)
    {
        float elapsed = 0f;
        float tickRate = damageTickInterval; // Sử dụng damageTickInterval đã định nghĩa

        // HashSet để theo dõi kẻ địch đã bị trúng trong mỗi tick
        HashSet<Enemy> enemiesHitInCurrentTick = new HashSet<Enemy>();
        HashSet<EnemyBase> enemyBasesHitInCurrentTick = new HashSet<EnemyBase>();

        CircleCollider2D explosionCollider = explosionGameObject.GetComponent<CircleCollider2D>();

        if (explosionCollider == null)
        {
            Debug.LogWarning("Explosion effect prefab is missing a CircleCollider2D!", explosionGameObject);
            yield break;
        }

        while (elapsed < duration)
        {
            if (explosionGameObject == null) yield break; // Dừng nếu vụ nổ đã bị phá hủy

            Vector3 currentExplosionPosition = explosionGameObject.transform.position + (Vector3)explosionCollider.offset;
            // Quan trọng: Nhân với scale hiện tại của GameObject để lấy bán kính thực tế nếu GameObject có scale khác 1
            float currentExplosionRadius = explosionCollider.radius * explosionGameObject.transform.localScale.x;

            enemiesHitInCurrentTick.Clear(); // Xóa cho tick mới
            enemyBasesHitInCurrentTick.Clear(); // Xóa cho tick mới

            Collider2D[] hitColliders = Physics2D.OverlapCircleAll(currentExplosionPosition, currentExplosionRadius);

            foreach (Collider2D hitCollider in hitColliders)
            {
                if (hitCollider.CompareTag("Enemy")) // Vẫn kiểm tra tag chung "Enemy"
                {
                    // Ưu tiên xử lý Enemy
                    Enemy enemy = hitCollider.GetComponent<Enemy>();
                    if (enemy != null)
                    {
                        if (!enemiesHitInCurrentTick.Contains(enemy))
                        {
                            enemy.TakeDamage(skillDamage); // Gây sát thương
                            enemiesHitInCurrentTick.Add(enemy);
                        }
                        continue; // Đã xử lý GameObject này là Enemy, bỏ qua kiểm tra EnemyBase cho nó
                    }

                    // Nếu không phải Enemy, thử kiểm tra EnemyBase
                    EnemyBase enemyBase = hitCollider.GetComponent<EnemyBase>();
                    if (enemyBase != null)
                    {
                        if (!enemyBasesHitInCurrentTick.Contains(enemyBase))
                        {
                            enemyBase.TakeDamage(skillDamage); // Gây sát thương
                            enemyBasesHitInCurrentTick.Add(enemyBase);
                        }
                    }
                    else
                    {
                        Debug.LogWarning($"GameObject with tag 'Enemy' on {hitCollider.name} does not have an Enemy or EnemyBase component!");
                    }
                }
            }
            yield return new WaitForSeconds(tickRate);
            elapsed += tickRate;
        }
    }
}