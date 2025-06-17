using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class J_Skill2 : BossSkillBase
{
    public GameObject circleFlame;

    protected override void Activate()
    {
        Instantiate(circleFlame, target.transform.position, Quaternion.identity);
    }
}
