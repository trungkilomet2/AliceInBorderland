using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Skill3_Warrior : SkillBase
{
    public float cooldownTime = 5f;
    private float lastUsedTime = -Mathf.Infinity;

    public SharedWarriorState sharedWarriorState;

    // === Quản lý các phần cần thay đổi ===
    public Sprite form1Sprite;
    public Sprite form2Sprite;

    public RuntimeAnimatorController form1Animator;
    public RuntimeAnimatorController form2Animator;

    public GameObject weaponForm1;
    public GameObject weaponForm2;

    private bool isForm1 = true;

    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private WeaponBase weapon;
    private Warrior warrior;
    public GameObject changeFormEffectPrefab;

    public override void Awake()
    {
        base.Awake();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        weapon = GetComponent<WeaponBase>();
        warrior = GetComponent<Warrior>();
    }

    protected override void Activate()
    {
        if (sharedWarriorState != null && sharedWarriorState.hasUsedSkill4)
            return;

        if (Time.time < sharedWarriorState.skill3LastUsedTime + cooldownTime)
            return;

        sharedWarriorState.skill3LastUsedTime = Time.time;
        Instantiate(changeFormEffectPrefab, transform.position, Quaternion.identity);

        // === Chuyển đổi giữa form 1 và form 2 ===
        if (isForm1)
        {
            // Sang form 2
            spriteRenderer.sprite = form2Sprite;
            animator.runtimeAnimatorController = form2Animator;

            weaponForm1.SetActive(false);
            weaponForm2.SetActive(true);
            warrior.axe = weaponForm2;
            warrior.moveSpeed = 3f;
            warrior.damageReductionMultiplier = 0.7f;
        }
        else
        {
            // Quay về form 1
            spriteRenderer.sprite = form1Sprite;
            animator.runtimeAnimatorController = form1Animator;

            weaponForm2.SetActive(false);
            weaponForm1.SetActive(true);
            warrior.axe = weaponForm1;
            warrior.moveSpeed = 4f;
            warrior.damageReductionMultiplier = 1f;
        }

        isForm1 = !isForm1;
    }
}

