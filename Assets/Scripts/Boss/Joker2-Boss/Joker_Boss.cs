using System.Collections;
using UnityEngine;

public class Joker_Boss : BossBase
{
    private bool isSkill4Used = false;

    protected override void Awake()
    {
        base.Awake();
    }

    void Update()
    {
        if (targetGameObject == null)
        {
            return;
        }

        
        BossSkillBase skill1 = bossSkillBases[0];
        if (Vector3.Distance(transform.position, targetGameObject.transform.position) < skill1.skillRange)
        {
            skill1.UseSkill();
        }

        
        BossSkillBase skill2 = bossSkillBases[1];
        bool skill1Done = true;
        JSkill1 skill1Cast = skill1 as JSkill1;
        if (skill1Cast != null)
        {
            skill1Done = !skill1Cast.IsRunning;
        }
        if (skill1Done && Vector3.Distance(transform.position, targetGameObject.transform.position) < skill2.skillRange)
        {
            skill2.UseSkill();
        }

        
        BossSkillBase skill3 = bossSkillBases[2];
        if (Vector3.Distance(transform.position, targetGameObject.transform.position) < skill3.skillRange)
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
