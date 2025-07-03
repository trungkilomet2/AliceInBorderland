using UnityEngine;

public class BearProjectile : Joker_Boss
{
    public float homingTime = 7f;
    public float moveSpeed = 4f;
    private Transform player;
    private Vector3 direction;
    private float timer;

    public void Init()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        timer = homingTime;
    }

    void Update()
    {


        Vector3 bearPos = transform.position;
        Vector3 playerPos = targetGameObject.transform.position;
        bool facingLeft = bearPos.x > playerPos.x;
        GetComponent<SpriteRenderer>().flipX = facingLeft;
        timer -= Time.deltaTime;
        if (player != null)
        {
            direction = (player.position - transform.position).normalized;
            transform.position += direction * moveSpeed * Time.deltaTime;
        }
        if (timer <= 0f)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<CharacterCommonBehavior>().TakeDamage(6);
        }
    }
}
