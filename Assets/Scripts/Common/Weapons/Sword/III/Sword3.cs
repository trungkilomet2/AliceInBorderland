using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword3 : MonoBehaviour
{
    public GameObject projectilePrefab;
    public float spawnInterval = 3f;
    public float projectileSpeed = 5f;
    public float spawnDistance = 1.5f;

    private Transform player;
    private float timer;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        timer = spawnInterval;
    }

    void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            SpawnProjectiles();
            timer = spawnInterval;
        }
    }

    void SpawnProjectiles()
    {
        Vector2[] directions = new Vector2[]
        {
            Vector2.up,
            Vector2.down,
            Vector2.left,
            Vector2.right
        };

        foreach (Vector2 dir in directions)
        {
            Vector3 spawnPos = player.position + (Vector3)(dir * spawnDistance);
            GameObject proj = Instantiate(projectilePrefab, spawnPos, Quaternion.identity);

            Rigidbody2D rb = proj.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = dir.normalized * projectileSpeed;
            }
        }
    }
}