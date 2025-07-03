using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SumSkill2 : SkillBase
{
    [Header("Skill 2 Settings")]
    public GameObject ProjectileSkill2;
    public int projectileCount = 7;
    public float radius = 4f;
    public float duration = 10f;

    // Danh sách để quản lý các projectile đang active
    private List<GameObject> activeProjectiles = new List<GameObject>();

    // Gán các thuộc tính kế thừa (gán đúng nút/phím & loại)
    public override void Awake()
    {
        base.Awake();
        skillNum = SkillNum.Skill2;            // Kích hoạt phím 2
        skillType = SkillType.Active;           // Chủ động
        skillName = "SumSkill2 - Vòng đạn";     // Tên
        cooldown = 15f;
        skillRange = radius;                    // Có thể tùy chỉnh
        skillWidth = 2f;
        skillDuration = duration;
        skillDamage = 10f;                      // Nếu có damage riêng, thay tại đây
    }

    public override void Update()
    {
        base.Update();
        // Không cần xử lý gì thêm, hệ thống base đã xử lý cooldown và trigger
    }

    public override void HandleSkillInput()
    {
        base.HandleSkillInput();
        // Không cần xử lý gì thêm, hệ thống base đã xử lý phím
    }

    // Hàm Activate được hệ thống gọi khi xác nhận dùng skill (bấm phím & click chuột trái)
    protected override void Activate()
    {
        // Lấy vị trí triển khai skill theo chỉ thị chuột (vòng tròn)
        Vector3 castPos = transform.position;
        if (indicatorInstance != null)
        {
            castPos = indicatorInstance.transform.position;
        }

        activeProjectiles.Clear();

        for (int i = 0; i < projectileCount; i++)
        {
            float angle = i * Mathf.PI * 2 / projectileCount;
            Vector3 offset = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * radius;

            GameObject proj = Instantiate(ProjectileSkill2, castPos + offset, Quaternion.identity);

            // Ignore va chạm với player
            Collider2D projCol = proj.GetComponent<Collider2D>();
            Collider2D playerCol = GetComponent<Collider2D>();
            if (projCol != null && playerCol != null)
            {
                Physics2D.IgnoreCollision(projCol, playerCol);
            }
            // Ignore va chạm với các projectile khác cùng skill
            foreach (var existing in activeProjectiles)
            {
                Collider2D c1 = existing.GetComponent<Collider2D>();
                if (projCol != null && c1 != null)
                {
                    Physics2D.IgnoreCollision(projCol, c1);
                }
            }

            // Khởi tạo projectile (nếu có logic riêng)
            ProjectileSkill2 projScript = proj.GetComponent<ProjectileSkill2>();
            if (projScript != null)
            {
                projScript.Initialize(this.transform, angle, radius);
                // Nếu có damage kế thừa
                if (projScript.GetType().GetProperty("damage") != null)
                    projScript.damage = skillDamage;
            }

            activeProjectiles.Add(proj);
        }

        // Kết thúc skill sau duration
        StartCoroutine(EndSkillAfterDuration());
    }

    IEnumerator EndSkillAfterDuration()
    {
        yield return new WaitForSeconds(skillDuration);

        foreach (var proj in activeProjectiles)
        {
            if (proj != null) Destroy(proj);
        }

        activeProjectiles.Clear();
    }
}
