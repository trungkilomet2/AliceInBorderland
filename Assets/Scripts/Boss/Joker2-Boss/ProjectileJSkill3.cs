using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileJSkill3 : MonoBehaviour
{
    public int damage = 10;
    public float lifeTime = 5f;
    private float timer = 0f;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= lifeTime)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            var health = other.GetComponent<CharacterCommonBehavior>();
            if (health != null)
            {
                health.TakeDamage(damage);
            }
            Destroy(gameObject); // Nếu muốn biến mất khi va chạm
        }
    }
}
