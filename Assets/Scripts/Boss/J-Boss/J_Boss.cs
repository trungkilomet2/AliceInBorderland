using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class J_Boss : BossBase
{
    public bool isClone = false;
    private bool isSkill4Used = false;
    protected Animator animator;

    protected override void Awake()
    {
        base.Awake();
        animator = transform.GetChild(0).GetComponent<Animator>();
        isSkill4Used = false;
        if (rgb2d != null)
        {
            rgb2d.velocity = Vector2.zero; // Reset velocity khi phân thân
        }
        animator.SetBool("isRunning", true);
    }

    void Update()
    {
        if (targetCharacter == null)
        {
            return;
        }


        // Set điều kiện để sử dụng kỹ năng
        BossSkillBase skill1 = bossSkillBases[0];
        if (Vector3.Distance(transform.position, targetCharacter.transform.position) < skill1.skillRange)
        {
            skill1.UseSkill();
        }

        if (!isClone)
        {
            BossSkillBase skill2 = bossSkillBases[1];
            if (Vector3.Distance(transform.position, targetCharacter.transform.position) < skill2.skillRange)
            {
                if (skill2.UseSkill())
                {
                    animator.SetBool("isAtk", true);
                    StartCoroutine(ResetAtk2Bool());
                }
            }

            BossSkillBase skill3 = bossSkillBases[2];
            if (Vector3.Distance(transform.position, targetCharacter.transform.position) < skill3.skillRange)
            {
                if (skill3.UseSkill())
                {
                    animator.SetBool("isAtk", true);
                    StartCoroutine(ResetAtk2Bool());
                }
            }

            BossSkillBase skill4 = bossSkillBases[3];
            if (hp < 500f && !isSkill4Used)
            {
                if (skill4.UseSkill())
                {
                    animator.SetBool("isAtk", true);
                    StartCoroutine(ResetAtk2Bool());
                    isSkill4Used = true;
                }
            }
        }
    }
    private IEnumerator ResetAtk2Bool()
    {
        yield return new WaitForSeconds(0.5f);
        animator.SetBool("isAtk", false);
    }
}
