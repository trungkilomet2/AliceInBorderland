
using System.Collections;
using UnityEngine;

public class JSkill3 : BossSkillBase
{
    public GameObject ProJSkill3;
    public float projectileSpeed = 8f;
    public float projectileLifetime = 5f;
    public float spreadAngle = 90f;    // Độ mở hình quạt
    public int[] projectilesPerWave = { 5, 10, 15 };
    public float intervalBetweenWaves = 2f;

    private void Awake()
    {
        cooldown = 10f; // Cooldown của skill
    }

    protected override void Activate()
    {
        StartCoroutine(FireBarrage());
    }

    IEnumerator FireBarrage()
    {
        for (int w = 0; w < projectilesPerWave.Length; w++)
        {
            int count = projectilesPerWave[w];
            Vector3 toPlayer = (target.transform.position - transform.position).normalized;
            float baseAngle = Mathf.Atan2(toPlayer.y, toPlayer.x) * Mathf.Rad2Deg;
            float angleStep = (spreadAngle) / (count - 1);
            float startAngle = baseAngle - spreadAngle / 2f;

            for (int i = 0; i < count; i++)
            {
                float angle = startAngle + i * angleStep;
                Vector3 dir = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad), 0);

                // Lấy vị trí boss hiện tại mỗi lần bắn
                GameObject proj = Instantiate(ProJSkill3, transform.position, Quaternion.identity);
                Rigidbody2D rb = proj.GetComponent<Rigidbody2D>();
                if (rb != null)
                    rb.velocity = dir * projectileSpeed;

                ProjectileJSkill3 damageScript = proj.GetComponent<ProjectileJSkill3>();
                if (damageScript != null)
                {
                    damageScript.lifeTime = projectileLifetime;
                    damageScript.damage = 10;
                }
            }

            if (w < projectilesPerWave.Length - 1)
                yield return new WaitForSeconds(intervalBetweenWaves);
        }
    }
}
