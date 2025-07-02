using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Q_Boss : BossBase
{
    protected override void Awake()
    {
        base.Awake();
        SetTarget(GameObject.FindGameObjectWithTag("Player"));
    }
    void Update()
    {
        if (targetCharacter == null)
        {
            return;
        }
        // Set điều kiện để sử dụng kỹ năng
        BossSkillBase skill1 = bossSkillBases[0];
        if (Vector3.Distance(transform.position, targetCharacter.transform.position) < skill1.skillRange) 
        {
            skill1.UseSkill();
        }
        BossSkillBase skill2 = bossSkillBases[1];
        if (Vector3.Distance(transform.position, targetCharacter.transform.position) < skill2.skillRange)
        {
            skill2.UseSkill();
        }

    }
}