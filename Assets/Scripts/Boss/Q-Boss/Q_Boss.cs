using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Q_Boss : BossBase
{
    protected override void Start()
    {
        base.Start();
        SetTarget(GameObject.FindGameObjectWithTag("Player"));
    }

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

        // Ngăn boss làm gì khác nếu đang dash
        if (Q_Boss_Skill1.isDashing)
        {
            return;
        }

        // Set điều kiện để sử dụng kỹ năng
        BossSkillBase skill1 = bossSkillBases[0];
        if (Vector3.Distance(transform.position, targetCharacter.transform.position) < skill1.skillRange)
        {
            skill1.UseSkill();
        }

    }
}