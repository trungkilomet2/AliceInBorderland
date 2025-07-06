using UnityEngine;

public class BunnyProjectile : Joker_Boss
{
    public GameObject bearPrefab;
    public float moveSpeed = 3f;
    public float lifeTime = 2f;
    private Vector3 direction;
    private float timeAlive = 0f;
    public bool IsRunning { get; private set; } = false;

    public void Init(Vector3 dir)
    {
        direction = (dir + new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f), 0)).normalized;
    }

    void Update()
    {

        
        transform.position += direction * moveSpeed * Time.deltaTime;
        timeAlive += Time.deltaTime;
        if (timeAlive >= lifeTime)
        {
            Explode();
        }
    }

    void Explode()
    {
        GameObject bear = Instantiate(bearPrefab, transform.position, Quaternion.identity);
        bear.GetComponent<BearProjectile>().Init();
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<CharacterCommonBehavior>().TakeDamage(3);
        }
    }
}