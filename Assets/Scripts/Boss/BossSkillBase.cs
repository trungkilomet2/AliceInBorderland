using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BossSkillBase : MonoBehaviour
{
    public string skillName;
    public float cooldown = 5f;
    public float skillDuration = 2f;
    public float skillRange = 10f;
    protected GameObject target;

    private float lastUsedTime = -Mathf.Infinity;

    protected virtual void Awake()
    {
        BossBase bossBase = GetComponent<BossBase>();
        target = bossBase.targetGameObject;
    }

    protected abstract void Activate();

    private bool IsReady()
    {
        return Time.time >= lastUsedTime + cooldown;
    }

    public void UseSkill()
    {
        if (IsReady())
        {
            Activate();
            lastUsedTime = Time.time;
        }
    }
}
