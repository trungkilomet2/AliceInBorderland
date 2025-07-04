using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] public float speed = 10f;
    [SerializeField] public float damage = 5f;
    public Vector3 direction;
    public float lifetime = 7f; 

    private float timer;

    private void Update()
    {
        transform.position += direction.normalized * speed * Time.deltaTime;

        timer += Time.deltaTime;
        if (timer >= lifetime)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            CharacterCommonBehavior target = other.GetComponent<CharacterCommonBehavior>();
            if (target != null)
            {
                target.TakeDamage(damage);
            }
            Destroy(gameObject);
        }
    }
}
