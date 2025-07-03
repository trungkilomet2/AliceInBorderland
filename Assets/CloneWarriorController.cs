using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloneWarriorController : MonoBehaviour
{
    public float dashSpeed = 10f;
    public float dashTime = 0.5f;
    public float explosionDamage = 30f;
    public GameObject explosionEffect;

    private Vector2 dashDirection;
    private float timer = 0f;
    private Warrior originalWarrior;

    public void StartDash(Vector2 dir, Warrior origin)
    {
        dashDirection = dir.normalized;
        originalWarrior = origin;
    }

    void Update()
    {
        transform.Translate(dashDirection * dashSpeed * Time.deltaTime);

        timer += Time.deltaTime;
        if (timer >= dashTime)
        {
            ExplodeAndMerge();
        }
    }

    void ExplodeAndMerge()
    {
        if (explosionEffect != null)
            Instantiate(explosionEffect, transform.position, Quaternion.identity);

        if (originalWarrior != null)
            originalWarrior.transform.position = transform.position;

        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Enemy"))
        {
            // Gây damage ở đây nếu cần
            col.GetComponent<Enemy>()?.TakeDamage(explosionDamage * 0.5f);
        }
    }
}

