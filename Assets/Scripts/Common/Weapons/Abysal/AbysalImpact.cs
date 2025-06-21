using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbysalImpact : MonoBehaviour
{
    public float destroyDelay = 0.5f;
    private Animator animator;
    private bool hasExploded = false;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (hasExploded) return;

        if (other.CompareTag("Enemy"))
        {
            hasExploded = true;

            animator.SetTrigger("Impact");

            GetComponent<Collider2D>().enabled = false;
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;

            if (other.CompareTag("Enemy"))
            {
                other.GetComponent<Enemy>()?.TakeDamage(10);
            }

            Destroy(gameObject, destroyDelay);
        }
    }
}
