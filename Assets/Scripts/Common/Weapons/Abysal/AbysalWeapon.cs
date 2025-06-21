using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbysalWeapon : MonoBehaviour
{

    public GameObject projectilePrefab;  // Prefab của đạn
    public float projectileSpeed = 10f;
    public float delayBeforeShoot = 2f;
    public string enemyTag = "Enemy";

    void Start()
    {
        StartCoroutine(ShootNearestEnemyAfterDelay());
    }

    IEnumerator ShootNearestEnemyAfterDelay()
    {
        GameObject nearestEnemy = FindNearestEnemy();
        if (nearestEnemy != null)
        {
            yield return new WaitForSeconds(delayBeforeShoot);
            ShootAt(nearestEnemy);
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
        if (projectilePrefab != null && target != null)
        {
            GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                Vector2 direction = (target.transform.position - transform.position).normalized;
                rb.velocity = direction * projectileSpeed;
            }
        }
    }




}
