using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;


public class CoolDownSkill1 : CoolDownBase
{
    GameObject player;
    SkillBase[] skillBase;


    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }

    public override void StartCooldown()
    {
        base.StartCooldown();
    }

    public override void CheckSkillActive()
    {
        player = GameObject.FindWithTag("Player");
        skillBase = player?.GetComponents<SkillBase>();
        SkillNum numberOfSkill = SkillNum.Skill1;
        skillBase = skillBase?.OrderBy(skill => skill.skillNum).ToArray();

        foreach (SkillBase skill in skillBase)
        {
            if (numberOfSkill == skill.skillNum)
            {
                skill.UnlockSkillBySkillNum(numberOfSkill);
                float skillCooldown = skill.GetCurrentCooldown();
                bool skillIsCoolingDown = skill.GetIsCoolingDown();

                if (skillIsCoolingDown && !isCoolingDown)
                {
                    cooldownTime = skillCooldown;
                    StartCooldown();
                }
            }
        }
    }

}
