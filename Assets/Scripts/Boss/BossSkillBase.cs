using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BossSkillBase : MonoBehaviour
{
    public string skillName;
    public float cooldown = 5f;
    public float skillDuration = 2f;
    public float skillRange = 10f;
    public AudioClip skillSound;

    protected GameObject target;

    private AudioManager audioManager;
    private float lastUsedTime = -Mathf.Infinity;
    public bool IsRunning { get; protected set; } = false;

    protected void Update()
    {
        if(target == null)
        {
            BossBase bossBase = GetComponent<BossBase>();
            target = bossBase.targetGameObject;
        }
    }

    protected virtual void Awake()
    {
        BossBase bossBase = GetComponent<BossBase>();
        if (bossBase != null)
        {
            target = bossBase.targetGameObject;
        }
        else
        {
            // Có thể log warning tại đây để biết đang gắn skill sai object
            Debug.LogWarning("BossSkillBase không tìm thấy BossBase trên object " + gameObject.name);
        }
        target = bossBase.targetGameObject;
        audioManager = FindAnyObjectByType<AudioManager>();
    }

    protected abstract void Activate();

    private bool IsReady()
    {
        return Time.time >= lastUsedTime + cooldown;
    }

    public bool UseSkill()
    {
        if (IsReady() && !IsRunning)
        {
            Activate();
            lastUsedTime = Time.time;
            if (skillSound != null && audioManager != null)
            {
                audioManager.PlaySoundClip(skillSound);
            }
            return true;
        }
        else
        {
            return false;
        }
    }


}
