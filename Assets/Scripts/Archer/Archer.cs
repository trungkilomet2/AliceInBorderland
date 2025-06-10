using System;
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

    public bool isSkill3Active = false; // Flag to check if Skill3 is active
    private float _timeBtwArrow = 0.2f;

    protected override void Update()
    {
        base.Update();

        _timeBtwArrow -= Time.deltaTime;
        if (Input.GetMouseButton(0) && _timeBtwArrow < 0)
        {
            Attack();
        }
    }


    public override void Attack()
    {
        _timeBtwArrow = timeBtwArrow;

        // Lấy vị trí chuột trên màn hình và chuyển sang tọa độ thế giới
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0f;

        if (isSkill3Active)
        {
            //bắn 3 mũi tên 3 hướng khác nhau
            Vector2 direction1 = (mouseWorldPos - arrowSpawnPoint.position).normalized;
            Vector2 direction2 = Quaternion.Euler(0, 0, 10) * direction1; // Xoay 10 độ
            Vector2 direction3 = Quaternion.Euler(0, 0, -10) * direction1; // Xoay -10 độ
            // Tạo tên và thêm lực cho từng tên
            for (int i = 0; i < 3; i++)
            {
                Vector2 direction = i == 0 ? direction1 : (i == 1 ? direction2 : direction3);
                GameObject newArrow = Instantiate(arrow, arrowSpawnPoint.position, Quaternion.identity);
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                newArrow.transform.rotation = Quaternion.Euler(0f, 0f, angle);
                Rigidbody2D arrowRb = newArrow.GetComponent<Rigidbody2D>();
                arrowRb.AddForce(direction * bulletForce, ForceMode2D.Impulse);
            }
        }
        else
        {
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

}
