using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill2 : SkillBase
{
    public GameObject skillEffect;

    private GameObject skillEffectInstant;

    public override void Awake()
    {
        base.Awake();
    }

    public override void Update()
    {
        base.Update();
    }

    protected override void Activate()
    {
        skillEffectInstant = Instantiate(skillEffect, skillTransform.position, Quaternion.identity);
        skillEffectInstant.transform.localScale = new Vector3(skillRange, skillRange, 1f);
        Destroy(skillEffectInstant, skillDuration);
    }
}
