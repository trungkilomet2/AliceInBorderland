using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class AbysalWeapon : MonoBehaviour
{

    public GameObject abysalAttackPrefab;  // Prefab của đạn
  //  public GameObject abysalImpactPrefab;
    public float projectileSpeed = 10f;
    public string enemyTag = "Enemy";
    float timeToAttack = 4f;
    float timer;


    void Start()
    {
        timer = timeToAttack;
    }

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            GameObject enemy = FindNearestEnemy();
            if (enemy != null)
            {
                ShootAt(enemy);
                timer = timeToAttack; // reset lại timer tại đây để không bắn liên tục
            }
        }
    }


    GameObject FindNearestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        GameObject nearest = null;
        float shortestDistance = Mathf.Infinity;
        Vector3 currentPosition = transform.position;
        foreach (GameObject enemy in enemies)
        {
            float distance = Vector3.Distance(currentPosition, enemy.transform.position);
            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                nearest = enemy;
            }
        }
        return nearest;
    }

    void ShootAt(GameObject target)
    {
        timer = timeToAttack;
        if (abysalAttackPrefab != null && target != null)
        {
            Vector2 direction = (target.transform.position - transform.position).normalized;
            // Tính góc quay
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            // Khởi tạo đầu đạn và xoay về hướng enemy
            GameObject projectile = Instantiate(abysalAttackPrefab, transform.position, Quaternion.Euler(0, 0, angle));

            Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = direction * projectileSpeed;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Attack enemy");
    }




}
