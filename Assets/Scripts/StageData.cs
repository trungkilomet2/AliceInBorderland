using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EnemySpawnConfig
{
    public EnemyData enemyData;
    [Range(1, 100)]
    public int maxCount;
}

[Serializable]
public class StageEvent
{
    [Header("Event Settings")]
    public string message;

    [Header("Enemy Spawn Settings")]
    public List<EnemySpawnConfig> enemiesToSpawn;
}

[CreateAssetMenu]
public class StageData : ScriptableObject
{
    public List<StageEvent> stageEvents;
}
