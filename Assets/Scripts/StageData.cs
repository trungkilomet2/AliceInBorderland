using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class StageEvent
{
    [Header("Event Settings")]
    public float time;
    public string message;

    [Header("Enemy Spawn Settings")]
    public GameObject enemyToSpawn;
    [Range(1, 100)]
    public int count = 1;
}

[CreateAssetMenu]
public class StageData : ScriptableObject
{
    public List<StageEvent> stageEvents;
}
