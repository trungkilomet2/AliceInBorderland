using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BossBase : EnemyBase
{
    public BossSkillBase[] bossSkillBases;

    protected override void Awake()
    {
        base.Awake();
        SetTarget(GameObject.FindGameObjectWithTag("Player"));
    }
}
