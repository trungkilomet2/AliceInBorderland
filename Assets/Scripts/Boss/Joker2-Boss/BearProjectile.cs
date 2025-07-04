using UnityEngine;

public class BearProjectile : MonoBehaviour
{
    public float homingTime = 7f;
    public float moveSpeed = 4f;
    private Transform player;
    private Vector3 direction;
    private float timer;

    public void Init()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
        timer = homingTime;
    }

    void Update()
    {
        
        if (player != null)
        {
            Vector3 bearPos = transform.position;
            Vector3 playerPos = player.position;
            bool facingLeft = bearPos.x > playerPos.x;
            GetComponent<SpriteRenderer>().flipX = facingLeft;

            direction = (player.position - transform.position).normalized;
            transform.position += direction * moveSpeed * Time.deltaTime;
        }

        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            
            var behav = other.GetComponent<CharacterCommonBehavior>();
            if (behav != null)
            {
                behav.TakeDamage(6);
            }
            Destroy(gameObject); 
        }
    }
}
