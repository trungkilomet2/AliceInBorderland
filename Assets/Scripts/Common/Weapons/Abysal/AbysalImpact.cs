using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbysalImpact : MonoBehaviour
{
    public float destroyDelay = 0.5f;
    private Animator animator;
    private bool hasExploded = false;
    private const string ENEMY_TAG = "Enemy";
    private const string IMPACT_TRIGGER = "Impact";
    private const float abysalDamage = 20f;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (hasExploded) return;

        if (other.CompareTag(ENEMY_TAG))
        {
            hasExploded = true;

            animator.SetTrigger(IMPACT_TRIGGER);

            GetComponent<Collider2D>().enabled = false;
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;

            if (other.CompareTag(ENEMY_TAG))
            {
                other.GetComponent<Enemy>()?.TakeDamage(abysalDamage);
            }

            Destroy(gameObject, destroyDelay);
        }
    }
}
