using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword3Damage : MonoBehaviour
{
    float swordDamage = 30f;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();
        Enemy enemy = collision.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy?.TakeDamage(swordDamage);
        }
        EnemyBase enemy2 = collision.GetComponent<EnemyBase>();
        enemy2?.TakeDamage(swordDamage);
    }
}
