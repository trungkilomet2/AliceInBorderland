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

[CreateAssetMenu]
public class StageData : ScriptableObject
{
    public List<StageEventBase> stageEvents;
}
