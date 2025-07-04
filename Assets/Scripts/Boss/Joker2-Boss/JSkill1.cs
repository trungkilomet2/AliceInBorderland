using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JSkill1 : BossSkillBase
{
    [Header("Fire Circle")]
    public float circleRadius = 5f;
    public int circleSegments = 100;
    public float circleDrawSpeed = 1f; // vòng lửa vẽ nhanh hay chậm
    

    [Header("Fire Star")]
    public float moveSpeed = 2f;
    public GameObject fireZonePrefab;
    public int starPoints = 5;
    public float fireSpawnDistance = 0.5f;
    [Range(1, 10)]
    public float starScale = 3f;

    public bool IsSkillRunning { get; private set; } = false;


    public Collider2D playerCol;

    private Coroutine runningSkill;

    void Start()
    {
        // Tự động tìm Player nếu quên kéo vào Inspector
        if (playerCol == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null) playerCol = player.GetComponent<Collider2D>();
        }
    }

    protected override void Activate()
    {
        if (runningSkill == null)
            runningSkill = StartCoroutine(SkillRoutine());
        Debug.Log("Skill Used!");
    }

    IEnumerator SkillRoutine()
    {
        IsSkillRunning = true;
        // 1. Vẽ vòng lửa quanh target
        Vector3 center = target.transform.position;
        yield return StartCoroutine(DrawFireCircleWithSprites(center));

        // 2. Tính toán các điểm ngôi sao (scale bán kính theo starScale)
        float scaledRadius = circleRadius * starScale;
        Vector3[] points = CalculateStarPoints(center, scaledRadius, starPoints);

        // 3. Boss bay theo đường ngôi sao, tạo vùng lửa
        yield return StartCoroutine(MoveBossAndSpawnFire(center, points));
        IsSkillRunning = false;
        runningSkill = null;
    }

    IEnumerator DrawFireCircleWithSprites(Vector3 center)
    {
        float radius = circleRadius * starScale;

        for (int i = 0; i <= circleSegments; i++)
        {
            float angle = (float)i / circleSegments * Mathf.PI * 2;
            Vector3 pos = center + new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * radius;

            GameObject flame = Instantiate(fireZonePrefab, pos, Quaternion.identity);
            flame.transform.rotation = Quaternion.Euler(0, 0, angle * Mathf.Rad2Deg); // Quay lửa hướng ra ngoài
            Destroy(flame, 3f); // Tự huỷ

            yield return new WaitForSeconds(1f / (circleSegments * circleDrawSpeed)); // tạo hiệu ứng "vẽ dần"
        }
    }

    Vector3[] CalculateStarPoints(Vector3 center, float radius, int points)
    {
        Vector3[] result = new Vector3[points];
        for (int i = 0; i < points; i++)
        {
            float angle = i * 2 * Mathf.PI / points - Mathf.PI / 2;
            result[i] = center + new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * radius;
        }
        return result;
    }

    IEnumerator MoveBossAndSpawnFire(Vector3 center, Vector3[] points)
    {
        int[] starOrder = { 0, 2, 4, 1, 3, 0 }; // Vẽ ngôi sao 5 cánh

        Vector3 lastFirePos = transform.position;
        for (int i = 0; i < starOrder.Length - 1; i++)
        {
            Vector3 start = points[starOrder[i]];
            Vector3 end = points[starOrder[i + 1]];
            float t = 0;
            while (t < 1f)
            {
                t += Time.deltaTime * moveSpeed;
                Vector3 pos = Vector3.Lerp(start, end, t);
                transform.position = pos;

                // Spawn fire zone nếu di chuyển đủ xa
                if (Vector3.Distance(lastFirePos, pos) > fireSpawnDistance)
                {
                    GameObject fire = Instantiate(fireZonePrefab, pos, Quaternion.identity);
                    Destroy(fire, 3f);

                    
                    
                    lastFirePos = pos;
                }
                yield return null;
            }
        }
    }
}
