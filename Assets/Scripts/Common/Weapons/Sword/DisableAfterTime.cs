using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableAfterTime : MonoBehaviour
{
    float swordDamage = 50f;
    float timeToDisable = 0.3f;
    float timer;
    private const string ENEMY_TAG = "Enemy";
    private void OnEnable()
    {
        timer = timeToDisable;
    }
    private void LateUpdate()
    {
        timer -= Time.deltaTime;
        if (timer < 0f)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();

        Enemy enemy = collision.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy?.TakeDamage(swordDamage);
        }
    }
}
