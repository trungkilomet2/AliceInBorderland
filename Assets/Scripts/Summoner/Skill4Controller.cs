using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill4Controller : SkillBase
{
    public GameObject Skill4Projectile;             // Prefab quả cầu tím (gắn UltimateOrb.cs/Skill4Call)
    public Transform firePoint;                     // Vị trí bắn orb (thường là player hoặc tay)
    public float delayBetweenOrbs = 0.5f;           // Độ trễ giữa các lượt bắn
    public float maxDistance = 4f;                  // Khoảng cách orb bay ra
    public int totalOrbs = 12;
    public float explodeDelay = 1f;                 // Đợi trước khi explode tất cả orb

    private List<Skill4Call> activeOrbs = new List<Skill4Call>();

    public override void Awake()
    {
        base.Awake();
        skillNum = SkillNum.Skill4;
        skillType = SkillType.Passive;
        skillName = "Skill4 - Ultimate Orb";
        cooldown = 30f;
        skillRange = maxDistance;                   // Tùy chỉnh nếu muốn
        skillWidth = 2f;
        skillDuration = (delayBetweenOrbs * totalOrbs) + explodeDelay;
        skillDamage = 0f;                           // Tùy chỉnh nếu orb gây damage
    }

    public override void Update()
    {
        base.Update();
        // Không xử lý phím, mọi trigger đã nằm trong SkillBase
    }

    public override void HandleSkillInput()
    {
        base.HandleSkillInput();
        // Mọi input trigger nằm trong hệ thống skill
    }

    protected override void Activate()
    {
        StartCoroutine(ActivateUltimate());
    }

    private IEnumerator ActivateUltimate()
    {
        activeOrbs.Clear();

        // SỬA: Lấy vị trí player mới từng lần spawn orb
        for (int i = 0; i < totalOrbs; i++)
        {
            Vector3 currentPos = firePoint ? firePoint.position : transform.position; // <-- luôn lấy vị trí hiện tại
            float angle = i * (360f / totalOrbs);
            Vector2 dir = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));

            GameObject orbObj = Instantiate(Skill4Projectile, currentPos, Quaternion.identity);
            Skill4Call orb = orbObj.GetComponent<Skill4Call>();
            if (orb != null)
            {
                orb.Initialize(dir, transform); // Hàm này nhận hướng & player
                orb.maxDistance = maxDistance;
                activeOrbs.Add(orb);
            }

            yield return new WaitForSeconds(delayBetweenOrbs);
        }

        // Đợi trước khi explode tất cả orb
        yield return new WaitForSeconds(explodeDelay);

        foreach (Skill4Call orb in activeOrbs)
        {
            if (orb != null) orb.ExplodeLaunch();
        }
    }
}
