using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSkillJ : MonoBehaviour
{
    private Vector3 startPos;
    public float maxDistance = 10f;
    public float damage = 20f;

    public void SetDestroyAfterDistance(Vector3 origin, float maxDist)
    {
        startPos = origin;
        maxDistance = maxDist;
    }

    void Update()
    {
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
                
            }

            if (enemyB != null)
            {
                enemyB.TakeDamage(damage);
                
            }
        }
    }
}


