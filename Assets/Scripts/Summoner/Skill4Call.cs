using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill4Call : MonoBehaviour
{
    private Transform player;
    private Vector2 shootDir;
    public float maxDistance = 4f;
    private float orbitSpeed = 200f; // độ xoay mỗi giây
    private float currentAngle;
    private Vector3 targetPos;
    public float damage = 30f;

    private enum State { FlyingOut, Orbiting, Exploding, Finished }
    private State state = State.FlyingOut;

    private float moveSpeed = 20f;
    private Animator anim;
    private Vector3 orbitOffset;
    private Vector3 launchDirection;   // hướng phóng ra khi rời quỹ đạo
    private Vector3 launchStartPos;    // vị trí bắt đầu launch

    private bool readyToExplode = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Trigger entered with: " + other.gameObject.name);

        if (other.CompareTag("Enemy"))
        {
            Debug.Log("HIT ENEMY!");
            Enemy enemy = other.GetComponent<Enemy>();
            EnemyBase enemyB = other.GetComponent<EnemyBase>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
                
            }

            if (enemyB != null)
            {
                enemyB.TakeDamage(damage);
                
            }
        }
    }

    public void Initialize(Vector2 dir, Transform playerTransform)
    {
        shootDir = dir.normalized;
        player = playerTransform;
        anim = GetComponent<Animator>();
        transform.localScale = Vector3.one;
        UpdateTargetPos();

        Collider2D myCol = GetComponent<Collider2D>();
        Collider2D playerCol = player.GetComponent<Collider2D>();

        if (myCol != null && playerCol != null)
        {
            Physics2D.IgnoreCollision(myCol, playerCol);
        }
    }

    void UpdateTargetPos()
    {
        targetPos = player.position + (Vector3)(shootDir * maxDistance);
    }

    void Update()
    {
        switch (state)
        {
            case State.FlyingOut:
                UpdateTargetPos();
                transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);

                if (Vector3.Distance(transform.position, targetPos) < 0.1f)
                {
                    orbitOffset = transform.position - player.position;
                    currentAngle = Mathf.Atan2(orbitOffset.y, orbitOffset.x) * Mathf.Rad2Deg;
                    state = State.Orbiting;
                }
                break;

            case State.Orbiting:
                currentAngle += orbitSpeed * Time.deltaTime;
                float rad = currentAngle * Mathf.Deg2Rad;
                transform.position = player.position + new Vector3(Mathf.Cos(rad), Mathf.Sin(rad)) * maxDistance;
                break;

            case State.Exploding:
                float traveled = Vector3.Distance(transform.position, launchStartPos);
                transform.position += launchDirection * moveSpeed * Time.deltaTime;

                if (traveled >= maxDistance)
                {
                    StartCoroutine(Explode());
                    state = State.Finished;
                }
                break;

            case State.Finished:
                // Không làm gì
                break;
        }
    }

    public void ExplodeLaunch()
    {
        readyToExplode = true;

        // Lấy hướng đang quay để phóng ra
        orbitOffset = transform.position - player.position;
        launchDirection = orbitOffset.normalized;
        launchStartPos = transform.position;

        state = State.Exploding;
    }

    public IEnumerator Explode()
    {
        //  Chuyển sang trạng thái kết thúc để ngắt Update()
        state = State.Finished;

        //  Dừng vật lý (nếu có Rigidbody2D)
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = Vector2.zero;
            rb.isKinematic = true; // hoặc rb.simulated = false;
        }

        //  Gọi animation phát nổ
        if (anim != null)
        {
            anim.SetTrigger("Explode");
        }

        //  Phóng to quả cầu
        transform.localScale *= 10f;

        //  Chờ animation kết thúc trước khi xóa
        yield return new WaitForSeconds(3f); 

        Destroy(gameObject);
    }
}
