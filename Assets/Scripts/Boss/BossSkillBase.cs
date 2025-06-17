using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BossSkillBase : MonoBehaviour
{
    public string skillName;
    public float cooldown = 5f;
    public float skillRange = 10f;
    public float skillDuration = 2f;
    public float skillDamage = 10f;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected abstract void Activate();

}
