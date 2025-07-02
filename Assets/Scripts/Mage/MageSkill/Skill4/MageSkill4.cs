using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageSkill4 : SkillBase
{
    public float rollSpeed = 40f;

    private bool isRolling = false;
    private Vector3 rollTarget;
    private Vector2 rollDirection;
    private Animator animator;

    public override void Awake()
    {
        animator = GetComponent<Animator>();
        base.Awake();
    }

    public override void Update()
    {
        base.Update();

        if (isRolling)
        {
            transform.position = Vector3.MoveTowards(transform.position, rollTarget, rollSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.position, rollTarget) < 0.1f)
            {
                isRolling = false;
            }
        }
    }

    protected override void Activate()
    {
        if (isRolling) return;

        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorld.z = 0;
        rollDirection = (mouseWorld - transform.position).normalized;

        rollTarget = transform.position + (Vector3)(rollDirection * skillRange);

        if (animator != null)
            animator.SetTrigger("roll");

        isRolling = true;
    }


    private void OnEnable()
    {
        CharacterCommonBehavior.OnBlockedCollision += StopRolling;
    }

    private void OnDisable()
    {
        CharacterCommonBehavior.OnBlockedCollision -= StopRolling;
    }
    private void StopRolling()
    {
        if (isRolling)
        {
            Debug.Log("Va chạm Block, dừng dash.");
            isRolling = false;
            CancelSkill();
        }
    }
}