using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill3 : SkillBase
{
    public GameObject effect;
    private GameObject effectInstance;
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
        if (archer.isSkill3Active)
        {
            time += Time.deltaTime;
            if (effectInstance != null)
            {
                effectInstance.transform.position = archer.transform.position;
            }
            if (time >= skillDuration)
            {
                archer.isSkill3Active = false;
                time = 0f; 
            }
            
        }
    }
    protected override void Activate()
    {
        Debug.Log("Skill3 Activated");
        archer.isSkill3Active = true;
        time = 0f; 

        if (effect != null && effectInstance == null)
        {
            effectInstance = Instantiate(effect, archer.transform.position, Quaternion.identity);
        }
    }
}
