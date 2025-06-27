using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill4Controller : MonoBehaviour
{
    public GameObject Skill4Projectile;             // Prefab quả cầu tím (đã gắn UltimateOrb.cs)
    public Transform firePoint;              // Vị trí bắt đầu bắn orb (thường là vị trí player hoặc tay)
    public float delayBetweenOrbs = 0.5f;    // Độ trễ giữa các lượt bắn
    public float maxDistance = 4f;           // Khoảng cách ban đầu để orb bay ra
    int stepSize = 2;
    public SkillCooldownUI skill4UI;
 


    public float cooldownTime = 30f;
    private float nextAvailableTime = 0f;

    private List<Skill4Call> activeOrbs = new List<Skill4Call>();

    void Update()
    {
        if ((Input.GetKeyDown(KeyCode.Alpha4) || Input.GetKeyDown(KeyCode.Keypad4)) && Time.time >= nextAvailableTime)
        {
            StartCoroutine(ActivateUltimate());
            skill4UI.TriggerCooldown(cooldownTime);
            nextAvailableTime = Time.time + cooldownTime;
        }
    }

    private IEnumerator ActivateUltimate()
    {
        activeOrbs.Clear(); // Clear nếu có
        Vector3 firePosition = firePoint.position;
        int totalOrbs = 12;

        for (int i = 0; i < totalOrbs; i++)
        {
            float angle = i * (360f / totalOrbs); // đều 360 độ
            Vector2 dir = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));

            GameObject orbObj = Instantiate(Skill4Projectile, firePoint.position, Quaternion.identity);
            Skill4Call orb = orbObj.GetComponent<Skill4Call>();
            if (orb != null)
            {
                orb.Initialize(dir, transform);
                orb.maxDistance = maxDistance;
                activeOrbs.Add(orb);
            }

            yield return new WaitForSeconds(delayBetweenOrbs);
        }

            // Đợi 1 chút cho orb bắt đầu quay
            yield return new WaitForSeconds(1f);

        // Gọi tất cả orb chuyển sang Explode
        foreach (Skill4Call orb in activeOrbs)
        {
            orb.ExplodeLaunch();
        }
    }
}
