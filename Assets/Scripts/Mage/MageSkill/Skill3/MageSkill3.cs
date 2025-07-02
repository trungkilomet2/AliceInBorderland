using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageSkill3 : SkillBase
{
    public GameObject explosionEffectPrefab;
    public int numberOfExplosions = 3;

    public float randomSpawnRadius = 10f;
    public float minSpawnRadius = 5f;

    public float explosionSize = 1f;

    public float damageTickInterval = 0.5f;

    public override void Awake()
    {
        base.Awake();
        skillType = SkillType.Passive;
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
                    explosionCollider.radius *= explosionSize;
                }
                explosionInstance.transform.localScale = Vector3.one;

                StartCoroutine(DealContinuousExplosionDamage(explosionInstance, skillDuration));

                // --- DÒNG ĐÃ SỬA LỖI ---
                Destroy(explosionInstance, skillDuration); // Đổi 'explosion' thành 'explosionInstance'
                // ---------------------
            }

            yield return new WaitForSeconds(0.2f);
        }
    }

    private IEnumerator DealContinuousExplosionDamage(GameObject explosionGameObject, float duration)
    {
        float elapsed = 0f;
        List<Enemy> enemiesHitThisTick = new List<Enemy>();

        CircleCollider2D explosionCollider = explosionGameObject.GetComponent<CircleCollider2D>();

        if (explosionCollider == null)
        {
            Debug.LogWarning("Explosion effect prefab is missing a CircleCollider2D!", explosionGameObject);
            yield break;
        }

        while (elapsed < duration)
        {
            if (explosionGameObject == null) yield break;

            Vector3 currentExplosionPosition = explosionGameObject.transform.position + (Vector3)explosionCollider.offset;
            // Quan trọng: Nhân với scale hiện tại của GameObject để lấy bán kính thực tế nếu GameObject có scale khác 1
            float currentExplosionRadius = explosionCollider.radius * explosionGameObject.transform.localScale.x;

            enemiesHitThisTick.Clear();

            Collider2D[] hitColliders = Physics2D.OverlapCircleAll(currentExplosionPosition, currentExplosionRadius);

            foreach (Collider2D hitCollider in hitColliders)
            {
                if (hitCollider.CompareTag("Enemy"))
                {
                    Enemy enemy = hitCollider.GetComponent<Enemy>();
                    if (enemy != null && !enemiesHitThisTick.Contains(enemy))
                    {
                        enemy.TakeDamage(skillDamage);
                        enemiesHitThisTick.Add(enemy);
                    }
                }
            }
            yield return new WaitForSeconds(damageTickInterval);
            elapsed += damageTickInterval;
        }
    }
}