using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageSkill1 : SkillBase
{
    public GameObject laserAnimationPrefab;
    public float laserLength = 20f;
    public float laserVisualWidth = 0.5f;

    private GameObject currentLaserInstance;
    private Vector3 laserOrigin;
    private Vector2 directionOnSkillActivation; 

    public override void Awake()
    {
        base.Awake();

        skillNum = SkillNum.Skill1;
        skillType = SkillType.Active;
        indicatorType = IndicatorType.Arrow;

        cooldown = 8f;
        skillDuration = 6f;
        skillRange = laserLength;
        this.skillWidth = laserVisualWidth;
    }

    protected override void Activate()
    {
        laserOrigin = transform.position;

        // Lấy hướng chuột THỰC TẾ tại thời điểm skill được kích hoạt (nhấn chuột trái)
        // Đây là hướng mà damage box sẽ sử dụng
        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        directionOnSkillActivation = (mouseWorld - transform.position).normalized;

        currentLaserInstance = Instantiate(laserAnimationPrefab, laserOrigin, Quaternion.identity);
        float scaleFactor = 4f; // hoặc laserLength / chiều_dài_thực_tế_của_prefab
        currentLaserInstance.transform.localScale = new Vector3(scaleFactor, scaleFactor, 1f);

        // Xoay visual của laser để nó nhìn đúng hướng chuột (có thêm 180 độ nếu prefab cần)
        float visualAngle = Mathf.Atan2(directionOnSkillActivation.y, directionOnSkillActivation.x) * Mathf.Rad2Deg;
        visualAngle += 180f; // Vẫn cần thêm 180 độ cho visual nếu prefab của bạn hướng ngược
        currentLaserInstance.transform.rotation = Quaternion.Euler(0f, 0f, visualAngle);

        // Bắt đầu Coroutine để quản lý thời gian tồn tại của laser
        StartCoroutine(LaserDurationCoroutine(skillDuration));
        // Bắt đầu Coroutine để quản lý sát thương liên tục của laser
        StartCoroutine(LaserDamageCoroutine(skillDuration));
    }

    public override void Update()
    {
        base.Update(); // Quan trọng: Gọi Update của SkillBase để xử lý input và trạng thái isPreparingSkill

        // Logic để visual laser theo chuột CHỈ KHI đang chuẩn bị skill (indicator đang hiện)
        if (IsPreparingSkill() && skillType == SkillType.Active) // <<< Thay đổi ở đây: gọi hàm IsPreparingSkill()
        {
            if (currentLaserInstance != null)
            {
                currentLaserInstance.transform.position = transform.position;

                Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector3 direction = mouseWorld - transform.position;
                direction.z = 0f;

                if (direction.sqrMagnitude > 0.001f)
                {
                    float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                    angle += 180f; // Vẫn cần thêm 180 độ cho visual
                    currentLaserInstance.transform.rotation = Quaternion.Euler(0f, 0f, angle);
                }
            }
        }
        else if (currentLaserInstance != null && !IsPreparingSkill())
        {
            currentLaserInstance.transform.position = transform.position;
 
        }
    }

    private IEnumerator LaserDurationCoroutine(float duration)
    {
        yield return new WaitForSeconds(duration);

        if (currentLaserInstance != null)
        {
            Destroy(currentLaserInstance);
            currentLaserInstance = null;
        }
    }

    private IEnumerator LaserDamageCoroutine(float duration)
    {
        float elapsed = 0f;
        float damageTickRate = 0.1f;

        // Sử dụng hai HashSet riêng biệt để theo dõi hai loại địch đã bị trúng trong tick này
        HashSet<Enemy> enemiesHitInCurrentTick = new HashSet<Enemy>();
        HashSet<EnemyBase> enemyBasesHitInCurrentTick = new HashSet<EnemyBase>(); // Đổi tên cho rõ ràng hơn

        while (elapsed < duration)
        {
            if (currentLaserInstance == null) yield break;

            // Xóa danh sách kẻ địch đã bị trúng trong tick này cho tick mới
            enemiesHitInCurrentTick.Clear();
            enemyBasesHitInCurrentTick.Clear();

            Vector2 boxDirection = directionOnSkillActivation;
            float boxAngle = Mathf.Atan2(boxDirection.y, boxDirection.x) * Mathf.Rad2Deg;

            Vector2 boxCenter = (Vector2)transform.position + boxDirection * (skillRange * 0.5f);
            Vector2 boxSize = new Vector2(skillRange, skillWidth);

            Collider2D[] hits = Physics2D.OverlapBoxAll(boxCenter, boxSize, boxAngle);

            foreach (Collider2D hit in hits)
            {
                if (hit.gameObject == gameObject) continue; // Bỏ qua chính người chơi

                if (hit.CompareTag("Enemy")) // Vẫn sử dụng tag "Enemy" cho cả hai loại
                {
                    // Cố gắng lấy component Enemy
                    Enemy enemy = hit.GetComponent<Enemy>();
                    if (enemy != null)
                    {
                        if (!enemiesHitInCurrentTick.Contains(enemy))
                        {
                            // Gây sát thương nếu đây là Enemy và chưa bị trúng trong tick này
                            enemy.TakeDamage(skillDamage);
                            enemiesHitInCurrentTick.Add(enemy);
                        }
                        // Bỏ qua phần kiểm tra EnemyBase nếu đã xử lý như một Enemy
                        continue; // Rất quan trọng để tránh xử lý một GameObject 2 lần
                    }

                    // Nếu không phải Enemy, thử lấy component EnemyBase
                    EnemyBase enemyBase = hit.GetComponent<EnemyBase>();
                    if (enemyBase != null)
                    {
                        if (!enemyBasesHitInCurrentTick.Contains(enemyBase))
                        {
                            // Gây sát thương nếu đây là EnemyBase và chưa bị trúng trong tick này
                            enemyBase.TakeDamage(skillDamage);
                            enemyBasesHitInCurrentTick.Add(enemyBase);
                        }
                    }
                    else
                    {
                        // Trường hợp GameObject có tag "Enemy" nhưng không có Enemy hoặc EnemyBase component
                        Debug.LogWarning($"GameObject with tag 'Enemy' on {hit.name} does not have an Enemy or EnemyBase component!");
                    }
                }
            }

            yield return new WaitForSeconds(damageTickRate);
            elapsed += damageTickRate;
        }
    }

    public override void CancelSkill()
    {
        base.CancelSkill();
        if (currentLaserInstance != null)
        {
            Destroy(currentLaserInstance);
            currentLaserInstance = null;
        }
    }

    // (Tùy chọn) Hàm Debug để trực quan hóa vùng OverlapBox trong Scene view
    void OnDrawGizmos()
    {
        if (Application.isPlaying && directionOnSkillActivation != Vector2.zero) // Chỉ vẽ khi có hướng đã lưu
        {
            Vector2 boxDirection = directionOnSkillActivation;
            float boxAngle = Mathf.Atan2(boxDirection.y, boxDirection.x) * Mathf.Rad2Deg;

            Vector2 boxCenter = (Vector2)transform.position + boxDirection * (skillRange * 0.5f);
            Vector2 boxSize = new Vector2(skillRange, skillWidth);

            Gizmos.color = Color.red;
            Matrix4x4 originalMatrix = Gizmos.matrix;
            Gizmos.matrix = Matrix4x4.TRS(boxCenter, Quaternion.Euler(0, 0, boxAngle), Vector3.one);
            Gizmos.DrawWireCube(Vector3.zero, new Vector3(boxSize.x, boxSize.y, 0.1f));
            Gizmos.matrix = originalMatrix;
        }
    }
}