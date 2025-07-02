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

    protected override void Start()
    {
        base.Start();
        StartCoroutine(WaitForPlayerAndSetTarget());
    }
    private IEnumerator WaitForPlayerAndSetTarget()
    {
        while (GameObject.FindGameObjectWithTag("Player") == null)
        {
            yield return null; // chờ 1 frame
        }

        SetTarget(GameObject.FindGameObjectWithTag("Player"));
    }
}
