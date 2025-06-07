using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer : CharacterCommonBehavior
{
    public override float moveSpeed { get; set; } = 5f; // Set a default move speed

    public GameObject arrow;
    public Transform arrowSpawnPoint;
    public float timeBtwArrow = 0.2f;
    public float bulletForce;

    private float _timeBtwArrow = 0.2f;
    private Animator animator;

    public SkillBase[] skills;



    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    protected override void Update()
    {
        base.Update();
        UpdateAnimation();

        _timeBtwArrow -= Time.deltaTime;
        if (Input.GetMouseButton(0) && _timeBtwArrow < 0)
        {
            Attack();
        }

        // Use the new skill input handling flow
        if (skills != null && skills.Length > 0 && skills[0] != null)
        {
            skills[0].HandleSkillInput();
        }
    }

    private void UpdateAnimation()
    {
        bool isRunning = Mathf.Abs(moveInput.x) > 0.01f || Mathf.Abs(moveInput.y) > 0.01f;
        animator.SetBool("isRunning", isRunning);
    }

    public override void Attack()
    {
        _timeBtwArrow = timeBtwArrow;

        // Lấy vị trí chuột trên màn hình và chuyển sang tọa độ thế giới
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0f;

        // Tính hướng từ điểm bắn đến vị trí chuột
        Vector2 direction = (mouseWorldPos - arrowSpawnPoint.position).normalized;

        // Tạo tên
        GameObject newArrow = Instantiate(arrow, arrowSpawnPoint.position, Quaternion.identity);

        // Xoay tên theo hướng bắn (nếu muốn)
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        newArrow.transform.rotation = Quaternion.Euler(0f, 0f, angle);

        // Thêm lực
        Rigidbody2D arrowRb = newArrow.GetComponent<Rigidbody2D>();
        arrowRb.AddForce(direction * bulletForce, ForceMode2D.Impulse);
    }
}
