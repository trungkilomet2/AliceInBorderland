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

    //Skill 1:
    private bool isChoosingDirection = false;
    private Vector2 rollDirection;
    public float rollDistance = 5f;
    public float rollSpeed = 20f;
    private bool isRolling = false;
    private Vector3 rollTarget;

    public GameObject arrowIndicatorPrefab;
    private GameObject arrowInstance;


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

        Skill1();
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

    public override void Skill1()
    {
        // Skill 1: Roll - Character rolls in the direction of movement
        if (isRolling)
        {
            // Di chuyển tới target
            transform.position = Vector3.MoveTowards(transform.position, rollTarget, rollSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.position, rollTarget) < 0.1f)
            {
                isRolling = false;
            }
            return;
        }

        if (Input.GetKeyDown(KeyCode.Alpha1) && !isChoosingDirection)
        {
            isChoosingDirection = true;
            ShowArrowIndicator();
        }

        if (isChoosingDirection)
        {
            UpdateArrowDirection();

            if (Input.GetMouseButtonDown(0))
            {
                rollDirection = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized;
                HideArrowIndicator();
                StartRoll();
            }
        }
    }
    public override void Skill2()
    {
        // Implement Skill2 logic here
    }
    public override void Skill3()
    {
        // Implement Skill3 logic here
    }
    public override void Skill4()
    {
        // Implement Skill4 logic here

    }

    void ShowArrowIndicator()
    {
        arrowInstance = Instantiate(arrowIndicatorPrefab, transform.position, Quaternion.identity);
    }

    void UpdateArrowDirection()
    {
        if (arrowInstance != null)
        {
            Vector3 dir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            //dir.z = 0;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            arrowInstance.transform.position = transform.position;
            arrowInstance.transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }

    void HideArrowIndicator()
    {
        if (arrowInstance != null)
        {
            Destroy(arrowInstance);
        }
        isChoosingDirection = false;
    }

    void StartRoll()
    {
        Vector2 rollDir = arrowInstance.transform.right.normalized;

        // Lấy chiều dài từ scale của body (child)
        float rollLength = arrowInstance.transform.GetChild(0).localScale.x;

        // Tính vị trí đích
        rollTarget = transform.position + (Vector3)(rollDir * rollLength);

        isRolling = true;
        animator.SetTrigger("roll"); // Optional
    }
}
