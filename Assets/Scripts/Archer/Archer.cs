using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer : CharacterCommonBehavior
{
    public GameObject arrow;
    public Transform arrowSpawnPoint;
    public float timeBtwArrow = 0.2f;
    public float bulletForce;

    private float _timeBtwArrow = 0.2f;

    private Rigidbody2D rb;

    protected override void Update()
    {

        base.Update();

        _timeBtwArrow -= Time.deltaTime;
        if (Input.GetMouseButton(0) && _timeBtwArrow < 0)
        {
            Debug.Log("attack");
            Attack();
        }
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
        // Implement Skill1 logic here
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
}
