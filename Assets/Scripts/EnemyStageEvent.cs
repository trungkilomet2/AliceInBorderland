using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyStageEvent", menuName = "Stage/EnemyStageEvent")]
public class EnemyStageEvent : StageEventBase
{
    [Header("Enemy Spawn Settings")]
    public List<EnemySpawnConfig> enemiesToSpawn;
}
