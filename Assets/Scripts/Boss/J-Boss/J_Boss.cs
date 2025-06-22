using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class J_Boss : BossBase
{
    public bool isClone = false;
    private bool isSkill4Used = false;
    protected override void Awake()
    {
        base.Awake();
        isSkill4Used = false;
        if (rgb2d != null)
        {
            rgb2d.velocity = Vector2.zero; // Reset velocity khi phân thân
        }
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

        if (!isClone)
        {
            BossSkillBase skill2 = bossSkillBases[1];
            if (Vector3.Distance(transform.position, targetCharacter.transform.position) < skill2.skillRange)
            {
                skill2.UseSkill();
            }

            BossSkillBase skill3 = bossSkillBases[2];
            if (Vector3.Distance(transform.position, targetCharacter.transform.position) < skill3.skillRange)
            {
                skill3.UseSkill();
            }

            BossSkillBase skill4 = bossSkillBases[3];
            if (hp == 100f && !isSkill4Used)
            {
                skill4.UseSkill();
                isSkill4Used = true;

            }
        }
    }
}
