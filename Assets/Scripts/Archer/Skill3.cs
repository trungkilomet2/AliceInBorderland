using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill3 : SkillBase
{
    private Archer archer; // Reference to the Archer component
    private float time = 0f;
    public override void Awake()
    {
        archer = GetComponent<Archer>();
        base.Awake();
    }

    public override void Update()
    {
        base.Update();
        if(archer.isSkill3Active)
        {
            time += Time.deltaTime;
            if (time >= skillDuration)
            {
                // Sau khi hết thời gian, tắt cờ isSkill3Active
                archer.isSkill3Active = false;
                time = 0f; // Reset thời gian
            }
        }
    }
    protected override void Activate()
    {
        Debug.Log("Skill3 Activated");
        archer.isSkill3Active = true;
        time = 0f; // Reset thời gian
    }
}
