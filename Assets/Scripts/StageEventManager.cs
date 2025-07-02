using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageEventManager : MonoBehaviour
{
    [SerializeField] StageData stageData;
    [SerializeField] EnemiesManager enemiesManager;
    [SerializeField] float spawnInterval = 2f;
    [SerializeField] float stageDuration = 120f; // mỗi stage 3 phút

    CommonUI commonUI;
    int currentStageIndex = -1;
    float stageStartTime;
    float nextSpawnTime;

    StageEvent currentStage;
    Dictionary<EnemyData, int> currentEnemyCounts = new Dictionary<EnemyData, int>();

    private void Awake()
    {
        commonUI = FindObjectOfType<CommonUI>();
    }

    private void Update()
    {
        // Bắt đầu stage mới nếu chưa có hoặc hết thời gian
        if (currentStageIndex == -1 || Time.time - stageStartTime >= stageDuration)
        {
            currentStageIndex++;
            if (currentStageIndex >= stageData.stageEvents.Count)
            {
                Debug.Log("Tất cả các stage đã hoàn thành.");
                enabled = false;
                return;
            }

            StartNewStage();
        }

        if (Time.time >= nextSpawnTime)
        {
            foreach (var enemyConfig in currentStage.enemiesToSpawn)
            {
                if (!currentEnemyCounts.ContainsKey(enemyConfig.enemyData))
                    currentEnemyCounts[enemyConfig.enemyData] = 0;

                if (currentEnemyCounts[enemyConfig.enemyData] < enemyConfig.maxCount)
                {
                    enemiesManager.SpawnEnemy(enemyConfig.enemyData);
                    currentEnemyCounts[enemyConfig.enemyData]++;
                }
            }
            nextSpawnTime = Time.time + spawnInterval;
        }
    }

    private void StartNewStage()
    {
        currentStage = stageData.stageEvents[currentStageIndex];
        stageStartTime = Time.time;
        nextSpawnTime = Time.time + spawnInterval;

        currentEnemyCounts.Clear();

        Debug.Log($"🎯 Bắt đầu Stage {currentStageIndex + 1}: {currentStage.message}");
    }

    public void OnEnemyDeath(EnemyData data)
    {
        if (currentEnemyCounts.ContainsKey(data))
            currentEnemyCounts[data]--;
    }
}

