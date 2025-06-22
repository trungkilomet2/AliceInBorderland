using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillJ : MonoBehaviour
{
    [Header("Dragon Skill Settings")]
    public GameObject DraSkillJ;
    public GameObject SkillJProjectile;
    private float dragonTimer = 0f;
    private GameObject currentDragon;
    private float dragonDuration = 5f;

    [Header("Skill Timing")]
    public float cooldownTime = 20f;
    private float cooldownTimer = 0f;
    public SkillCooldownUI skillJUI;

    public float bulletForce = 10f;
    public float delayBetweenWaves = 2f;

    [Header("Wave Settings")]
    public int[] waveCounts = { 5, 10, 15 };
    public float[] waveDistances = { 5f, 10f, 15f };

    private bool isCasting = false;

    // Danh sách projectiles được tạo để ignore collision giữa chúng
    private List<GameObject> recentProjectiles = new List<GameObject>();

    void Update()
    {
        if (cooldownTimer > 0f)
            cooldownTimer -= Time.deltaTime;

        if (currentDragon != null)
        {
            currentDragon.transform.position = transform.position + Vector3.up * 2f;

            dragonTimer -= Time.deltaTime;
            if (dragonTimer <= 0f)
            {
                Destroy(currentDragon);
                currentDragon = null;
            }
        }

        if ((Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3)) && cooldownTimer <= 0 && !isCasting)
        {
            StartCoroutine(CastDragonSkill());
        }
    }

    private IEnumerator CastDragonSkill()
    {
        isCasting = true;

        cooldownTimer = cooldownTime;
        skillJUI?.TriggerCooldown(cooldownTime);
        dragonTimer = dragonDuration;

        currentDragon = Instantiate(DraSkillJ, transform.position + Vector3.up * 2f, Quaternion.identity);

        Animator anim = currentDragon.GetComponent<Animator>();
        if (anim != null)
        {
            anim.SetTrigger("Appear");
        }

        for (int i = 0; i < waveCounts.Length; i++)
        {
            Animator animS = GetComponent<Animator>();
            if (animS != null)
            {
                animS.SetTrigger("SkillJ");
            }

            // Bắn projectiles và thêm ignore va chạm
            FireInCircle(waveCounts[i], waveDistances[i]);

            yield return new WaitForSeconds(delayBetweenWaves);
        }

        isCasting = false;
    }

    private void FireInCircle(int count, float maxDistance)
    {
        float angleStep = 360f / count;
        Vector3 origin = transform.position;

        recentProjectiles.Clear();

        for (int i = 0; i < count; i++)
        {
            float angle = i * angleStep * Mathf.Deg2Rad;
            Vector2 direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));

            Vector3 spawnPosition = origin + (Vector3)(direction * 1f);

            GameObject fireball = Instantiate(SkillJProjectile, spawnPosition, Quaternion.identity);
            Rigidbody2D rb = fireball.GetComponent<Rigidbody2D>();

            // Ignore với người chơi
            Collider2D projCol = fireball.GetComponent<Collider2D>();
            Collider2D playerCol = GetComponent<Collider2D>();
            if (projCol != null && playerCol != null)
            {
                Physics2D.IgnoreCollision(projCol, playerCol);
            }

            // Ignore với các projectile cùng skill
            foreach (var existing in recentProjectiles)
            {
                Collider2D c1 = fireball.GetComponent<Collider2D>();
                Collider2D c2 = existing.GetComponent<Collider2D>();
                if (c1 != null && c2 != null)
                {
                    Physics2D.IgnoreCollision(c1, c2);
                }
            }

            recentProjectiles.Add(fireball);

            if (rb != null)
            {
                rb.AddForce(direction * bulletForce, ForceMode2D.Impulse);
            }

            ProjectileSkillJ fb = fireball.GetComponent<ProjectileSkillJ>();
            if (fb != null)
            {
                fb.SetDestroyAfterDistance(origin, maxDistance);
            }
        }
    }
}
