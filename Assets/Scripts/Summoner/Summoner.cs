using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Summoner : CharacterCommonBehavior
{
    public override float moveSpeed { get; set; } = 5f; // Tốc độ mặc định

    public GameObject projectile;
    public Transform projectileSpawnPoint;
    public float timeBtwProjectile = 0.2f;
    public float bulletForce = 5f;

    private float _timeBtwProjectile = 0.2f;

    protected override void Update()
    {
        base.Update();

        _timeBtwProjectile -= Time.deltaTime;
        if (Input.GetMouseButton(0) && _timeBtwProjectile <= 0f)
        {
            Attack();
        }
    }

    public override void Attack()
    {
        _timeBtwProjectile = timeBtwProjectile;

        // Gọi animation bắn
        Animator anim = GetComponent<Animator>();
        if (anim != null)
        {
            anim.SetTrigger("Shoot");
        }

        // Lấy vị trí chuột
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0f;

        Vector2 direction = (mouseWorldPos - transform.position).normalized;
        Vector3 spawnPos = transform.position + (Vector3)(direction * 0.5f);

        GameObject newProjectile = Instantiate(projectile, spawnPos, Quaternion.identity);

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        newProjectile.transform.rotation = Quaternion.Euler(0f, 0f, angle);

        Rigidbody2D projectileRb = newProjectile.GetComponent<Rigidbody2D>();
        if (projectileRb != null)
        {
            projectileRb.AddForce(direction * bulletForce, ForceMode2D.Impulse);
        }

        // Tránh va chạm với chính mình
        Collider2D projCol = newProjectile.GetComponent<Collider2D>();
        Collider2D playerCol = GetComponent<Collider2D>();
        if (projCol != null && playerCol != null)
        {
            Physics2D.IgnoreCollision(projCol, playerCol);
        }
    }
}
