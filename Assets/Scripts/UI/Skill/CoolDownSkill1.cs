using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoolDownSkill1 : CoolDownBase
{
    // Ghi đè hoặc mở rộng nếu cần

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
        Debug.Log("Skill 1 cooldown started.");
    }

}
