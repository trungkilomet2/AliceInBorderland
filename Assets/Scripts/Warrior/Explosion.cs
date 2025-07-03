using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public float damage = 999f;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy enemy = collision.GetComponent<Enemy>();
        EnemyBase enemy1 = collision.GetComponent<EnemyBase>();
        if (collision.CompareTag("Enemy"))
        {
            enemy.TakeDamage(damage);
            enemy1.TakeDamage(damage);
        }
    }
    public void DestroyExplosion()
    {
        Destroy(gameObject);
    }
}
