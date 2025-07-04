using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Sword4TargetEnemy : MonoBehaviour
{
    public GameObject damagePrefab;
    public float searchRadius = 10f;
    private GameObject player;
    private const string PLAYER_TAG = "Player";
    private const string ENEMY_TAG = "Enemy";
    public GameObject hoverObject;

    public float attackCooldown = 1f;
    private float lastAttackTime;
    private float swordDamage = 15f;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag(PLAYER_TAG);
        hoverObject.transform.position = player.transform.position;
        hoverObject.transform.SetParent(player.transform);
    }

    void Update()
    {
        GameObject nearestEnemy = FindNearestEnemy();
        if (nearestEnemy != null)
        {
            damagePrefab.SetActive(true);
            damagePrefab.transform.position = nearestEnemy.transform.position;

            if (Time.time - lastAttackTime >= attackCooldown)
            {
                nearestEnemy.GetComponent<Enemy>()?.TakeDamage(swordDamage);
                nearestEnemy.GetComponent<EnemyBase>()?.TakeDamage(swordDamage);
                lastAttackTime = Time.time;
            }
        }
        else
        {
            damagePrefab.SetActive(false);
        }
    }

    GameObject FindNearestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(ENEMY_TAG);
        GameObject nearest = null;
        float shortestDistance = Mathf.Infinity;
        Vector3 currentPosition = transform.position;

        foreach (GameObject enemy in enemies)
        {
            float distance = Vector3.Distance(currentPosition, enemy.transform.position);
            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                nearest = enemy;
            }
        }
        return nearest;
    }
}
