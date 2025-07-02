using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BossStageEvent", menuName = "Stage/BossStageEvent")]
public class BossStageEvent : StageEventBase
{
    [Header("Boss Settings")]
    public GameObject bossPrefab;
}
