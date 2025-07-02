using System;
using System.Collections.Generic;
using UnityEngine;

public class StageEventManager : MonoBehaviour
{
    [SerializeField] private StageData stageData;
    [SerializeField] private EnemiesManager enemiesManager;
    [SerializeField] private float stageDuration = 120f;
    [SerializeField] private float spawnInterval = 2f;

    private CommonUI commonUI;
    private int currentStageIndex = -1;
    private float stageStartTime;
    private float nextSpawnTime;
    private float totalGameTime = 0f;

    private StageEventBase currentStage;
    private Dictionary<EnemyData, int> currentEnemyCounts = new Dictionary<EnemyData, int>();
    private bool bossSpawned = false;

    private GameObject player;

    private bool isBossStage = false;
    private bool bossDefeated = false;

    private void Awake()
    {
        commonUI = FindObjectOfType<CommonUI>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
            if (player == null) return;
        }

        totalGameTime += Time.deltaTime;

        // Thay đổi spawnInterval theo thời gian chơi
        if (totalGameTime >= 540f)
            spawnInterval = 0.2f;
        else if (totalGameTime >= 360f)
            spawnInterval = 0.5f;
        else if (totalGameTime >= 180f)
            spawnInterval = 1f;
        else
            spawnInterval = 2f;

        // Nếu boss đã bị diệt thì sang stage ngay
        if (isBossStage && bossDefeated)
        {
            AdvanceToNextStage();
            return;
        }

        // Nếu stage hết thời gian thì chuyển stage
        if (currentStageIndex == -1 || Time.time - stageStartTime >= stageDuration)
        {
            AdvanceToNextStage();
            return;
        }

        if (Time.time >= nextSpawnTime)
        {
            if (currentStage is StageEvent enemyStage)
            {
                foreach (var enemyConfig in enemyStage.enemiesToSpawn)
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
            else if (currentStage is BossStageEvent bossStage)
            {
                if (!bossSpawned && bossStage.bossPrefab != null)
                {
                    enemiesManager.SpawnBoss(bossStage.bossPrefab);
                    bossSpawned = true;
                    Debug.Log("👑 Boss đã xuất hiện!");
                }
            }
        }
    }

    private void AdvanceToNextStage()
    {
        currentStageIndex++;
        if (currentStageIndex >= stageData.stageEvents.Count)
        {
            Debug.Log("🎉 Tất cả các stage đã hoàn thành.");
            enabled = false;
            return;
        }

        StartNewStage();
    }

    private void StartNewStage()
    {
        currentStage = stageData.stageEvents[currentStageIndex];
        stageStartTime = Time.time;
        nextSpawnTime = Time.time + spawnInterval;
        bossSpawned = false;
        bossDefeated = false;
        currentEnemyCounts.Clear();

        isBossStage = currentStage is BossStageEvent;

        Debug.Log($"🚩 Bắt đầu Stage {currentStageIndex + 1}: {currentStage.message}");
    }

    public void OnEnemyDeath(EnemyData data)
    {
        if (currentEnemyCounts.ContainsKey(data))
        {
            currentEnemyCounts[data]--;
        }
    }

    internal void OnBossDeath()
    {
        if (isBossStage)
        {
            bossDefeated = true;
            Debug.Log("✅ Boss đã bị tiêu diệt!");
        }
    }
}
