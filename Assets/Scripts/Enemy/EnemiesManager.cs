using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesManager : MonoBehaviour
{
    [SerializeField] GameObject enemy;
    [SerializeField] GameObject enemyAnimation;
    [SerializeField] EnemyData currentEnemyData;
    [SerializeField] Vector2 spawnArea;
    [SerializeField] GameObject player;

    private void Start()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
    }

    public void SetPlayer(GameObject newPlayer)
    {
        player = newPlayer;
    }

    public void SetEnemyPrefab(GameObject newEnemyPrefab)
    {
        enemy = newEnemyPrefab;
    }

    public void SetEnemyData(EnemyData enemyData)
    {
        currentEnemyData = enemyData;
    }

    public void SpawnEnemy()
    {
        Vector3 position = GenerateRandomPosition();

        position += player.transform.position;

        // spawn main object
        GameObject newEnemy = Instantiate(enemy);
        newEnemy.transform.position = position;
        Enemy enemyComponent = newEnemy.GetComponent<Enemy>();
        if (enemyComponent != null)
        {
            enemyComponent.SetTarget(player);
        }
        newEnemy.transform.parent = transform;

        // spawn sprite object
        GameObject spriteObject = Instantiate(enemyAnimation);
        spriteObject.transform.parent = newEnemy.transform;
        spriteObject.transform.localPosition = Vector3.zero; 
    }

    public void SpawnEnemy(EnemyData data)
    {
        Vector3 position = GenerateRandomPosition();

        position += player.transform.position;

        // spawn main object
        GameObject newEnemy = Instantiate(enemy);
        newEnemy.transform.position = position;
        Enemy enemyComponent = newEnemy.GetComponent<Enemy>();
        if (enemyComponent != null)
        {
            enemyComponent.SetTarget(player);
        }
        newEnemy.transform.parent = transform;

        // spawn sprite object
        GameObject spriteObject = Instantiate(data.animatedPrefab);
        spriteObject.transform.parent = newEnemy.transform;
        spriteObject.transform.localPosition = Vector3.zero;
    }

    private Vector3 GenerateRandomPosition()
    {
        Vector3 position = new Vector3();
        float f = UnityEngine.Random.value > 0.5f ? -1f : 1f;
        if (UnityEngine.Random.value > 0.5f)
        {
            position.x = UnityEngine.Random.Range(-spawnArea.x, spawnArea.x);
            position.y = f * spawnArea.y;
        }
        else
        {
            position.y = UnityEngine.Random.Range(-spawnArea.y, spawnArea.y);
            position.x = f * spawnArea.x;
        }

        position.z = 0;

        return position;
    }
}