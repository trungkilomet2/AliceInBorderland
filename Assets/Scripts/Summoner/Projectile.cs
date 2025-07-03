using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float maxDistance = 15f;
    private Vector3 startPos;
    public float damage = 10f;
    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        Debug.Log("Projectile running...");
        if (Vector3.Distance(transform.position, startPos) > maxDistance)
        {
            Destroy(gameObject);
        }
    }
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
                Destroy(gameObject);
            }
            
            if (enemyB != null)
            {
                enemyB.TakeDamage(damage);
                Destroy(gameObject);
            }
        }
    }
}
