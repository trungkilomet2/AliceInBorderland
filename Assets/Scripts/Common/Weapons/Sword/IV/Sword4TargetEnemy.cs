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
            nearestEnemy.GetComponent<Enemy>().TakeDamage(10f);
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
