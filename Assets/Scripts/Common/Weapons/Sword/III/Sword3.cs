using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword3 : MonoBehaviour
{
    public GameObject swordWeapon;
    public float spawnInterval = 3.5f;
    public float weaponSpeed = 5f;
    public float spawnDistance = 1.5f;

    private Transform player;
    private float timer;
    private const string PLAYER_TAG = "Player";

    void Start()
    {
        player = GameObject.FindGameObjectWithTag(PLAYER_TAG).transform;
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
            GameObject proj = Instantiate(swordWeapon, spawnPos, Quaternion.identity);
            proj.SetActive(true);
            Rigidbody2D rb = proj.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = dir.normalized * weaponSpeed;
            }

            Destroy(proj, 3f);

        }
    }
}