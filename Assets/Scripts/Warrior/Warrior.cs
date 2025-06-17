using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Warrior : CharacterCommonBehavior
{
    public override float moveSpeed { get; set; } = 4f;
    private Animator animator;
    public GameObject axe;
    public Transform axeSpawnPoint;
    public float timeBtwaxe = 0.2f;
    public float bulletForce;
    private bool isActived = false;

    // Start is called before the first frame update

    // Update is called once per frame
    private float _timeBtwaxe = 0.2f;
    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    protected override void Update()
    {
        base.Update();
        _timeBtwaxe -= Time.deltaTime;
        if (Input.GetMouseButtonDown(0) && _timeBtwaxe < 0)
        {
            animator.SetTrigger("isAttack");
            Attack();
        }
    }
    public override void Attack()
    {
        _timeBtwaxe = timeBtwaxe;

        // Lấy vị trí chuột trên màn hình và chuyển sang tọa độ thế giới
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0f;
        // Tính hướng từ điểm bắn đến vị trí chuột
        Vector2 direction = (mouseWorldPos - axeSpawnPoint.position).normalized;

        // Tạo tên
        GameObject newaxe = Instantiate(axe, axeSpawnPoint.position, Quaternion.identity);

        // Xoay tên theo hướng bắn (nếu muốn)
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        newaxe.transform.rotation = Quaternion.Euler(0f, 0f, angle);

        // Thêm lực
        Rigidbody2D arrowRb = newaxe.GetComponent<Rigidbody2D>();
        arrowRb.AddForce(direction * bulletForce, ForceMode2D.Impulse);
    }
}
