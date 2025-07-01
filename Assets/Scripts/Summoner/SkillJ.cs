using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillJ : SkillBase
{
    [Header("Dragon Skill Settings")]
    public GameObject DraSkillJ;
    public GameObject SkillJProjectile;
    public float dragonDuration = 5f;

    public float bulletForce = 10f;
    public float delayBetweenWaves = 2f;

    [Header("Wave Settings")]
    public int[] waveCounts = { 5, 10, 15 };
    public float[] waveDistances = { 5f, 10f, 15f };

    // Private
    private GameObject currentDragon;
    private float dragonTimer = 0f;
    private bool isCasting = false;
    private List<GameObject> recentProjectiles = new List<GameObject>();

    public override void Awake()
    {
        base.Awake();
        skillNum = SkillNum.Skill3;      // Gán đúng số phím (Alpha3)
        skillType = SkillType.Active;    // Là skill chủ động
        skillName = "Skill J Rồng Lửa";
        cooldown = 20f;
        skillRange = 10f;      // Khoảng cách chọn vị trí
        skillWidth = 2f;
        skillDuration = 5f;    // thời gian tồn tại rồng
        skillDamage = 20f;
    }

    public override void Update()
    {
        base.Update();
        // Quản lý thời gian tồn tại của dragon (nếu đang active)
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
    }

    public override void HandleSkillInput()
    {
        base.HandleSkillInput();
        // Không cần làm gì thêm ở đây, mọi logic input đã xử lý ở SkillBase
    }

    protected override void Activate()
    {
        // Lấy vị trí con trỏ hoặc vị trí chỉ định (nếu dùng Arrow thì hướng, Circle thì vị trí)
        Vector3 spawnPos = transform.position + Vector3.up * 2f;
        if (indicatorInstance != null)
        {
            spawnPos = transform.position + Vector3.up * 2f;
        }

        // Kích hoạt dragon, trigger animation
        currentDragon = Instantiate(DraSkillJ, spawnPos, Quaternion.identity);
        dragonTimer = skillDuration;
        Animator anim = currentDragon.GetComponent<Animator>();
        if (anim != null)
        {
            anim.SetTrigger("Appear");
        }

        // Gọi coroutine để thực hiện các wave bắn
        StartCoroutine(CastDragonSkillWaves());
    }

    // Đã sửa: Lấy vị trí mới mỗi wave
    private IEnumerator CastDragonSkillWaves()
    {
        isCasting = true;
        for (int i = 0; i < waveCounts.Length; i++)
        {
            // Lấy lại vị trí hiện tại của nhân vật (nếu nhân vật di chuyển)
            Vector3 currentOrigin = transform.position + Vector3.up * 2f;

            Animator animS = GetComponent<Animator>();
            if (animS != null)
            {
                animS.SetTrigger("SkillJ");
            }
            FireInCircle(waveCounts[i], waveDistances[i], currentOrigin);
            yield return new WaitForSeconds(delayBetweenWaves);
        }
        isCasting = false;
    }

    // Bắn các projectiles theo hình tròn
    private void FireInCircle(int count, float maxDistance, Vector3 origin)
    {
        float angleStep = 360f / count;
        recentProjectiles.Clear();

        for (int i = 0; i < count; i++)
        {
            float angle = i * angleStep * Mathf.Deg2Rad;
            Vector2 direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
            Vector3 spawnPosition = origin + (Vector3)(direction * 1f);

            GameObject fireball = Instantiate(SkillJProjectile, spawnPosition, Quaternion.identity);
            Rigidbody2D rb = fireball.GetComponent<Rigidbody2D>();

            // Ignore va chạm với người chơi
            Collider2D projCol = fireball.GetComponent<Collider2D>();
            Collider2D playerCol = GetComponent<Collider2D>();
            if (projCol != null && playerCol != null)
            {
                Physics2D.IgnoreCollision(projCol, playerCol);
            }

            // Ignore giữa các projectile cùng skill
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
                fb.damage = skillDamage;
            }
        }
    }
}
