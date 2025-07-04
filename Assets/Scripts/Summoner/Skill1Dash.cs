using UnityEngine;
using System.Collections;

public class Skill1Dash : SkillBase
{
    public float dashSpeed = 20f;
    public float dashDistance = 7f;

    private bool isDashing = false;

    private Vector3 dashTarget;
    private Vector2 dashDirection;
    private Animator animator;
    private CharacterCommonBehavior characterCommonBehavior;

    [Header("Ghost Effect")]
    public GhostImage ghostEffect;

    public override void Awake()
    {
        characterCommonBehavior = GetComponent<CharacterCommonBehavior>();
        animator = GetComponent<Animator>();
        base.Awake();
        skillNum = SkillNum.Skill1;
        skillType = SkillType.Active;
        indicatorType = IndicatorType.Arrow;
        skillName = "Dash";
        cooldown = 4f;
        skillRange = dashDistance;
        skillWidth = 1f;
        skillDuration = dashDistance / dashSpeed;
        skillDamage = 0f;
    }

    public override void Update()
    {
        base.Update();

        if (isDashing)
        {
            transform.position = Vector3.MoveTowards(transform.position, dashTarget, dashSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.position, dashTarget) < 0.1f)
            {
                isDashing = false;
                if (characterCommonBehavior != null)
                {
                    characterCommonBehavior.isDashing = false;
                }
            }

        }
    }

    protected override void Activate()
    {

        if (ghostEffect != null)
        {
            StartCoroutine(ghostEffect.SpawnGhosts(skillDuration));
        }
        if (isDashing) return;

      
        Vector2 dashDir = Vector2.right;

        if (indicatorInstance != null)
        {
            
            dashDir = (indicatorInstance.transform.position - transform.position).normalized;
            if (dashDir.sqrMagnitude < 0.01f)
                dashDir = indicatorInstance.transform.right.normalized;
        }
        else
        {
            
            Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseWorld.z = 0;
            dashDir = (mouseWorld - transform.position).normalized;
        }

        dashDirection = dashDir;
        dashTarget = transform.position + (Vector3)(dashDirection * skillRange);

        
        if (animator != null)
            animator.SetTrigger("roll");

        isDashing = true;
        if (characterCommonBehavior != null)
        {
            characterCommonBehavior.isDashing = true;
        }
    }

    private void OnEnable()
    {
        CharacterCommonBehavior.OnBlockedCollision += StopDash;
    }

    private void OnDisable()
    {
        CharacterCommonBehavior.OnBlockedCollision -= StopDash;
    }

    private void StopDash()
    {
        if (isDashing)
        {
            Debug.Log("Va chạm Block, dừng dash.");
            isDashing = false;
            CancelSkill();
            if (characterCommonBehavior != null)
            {
                characterCommonBehavior.isDashing = false;
            }
        }
    }
    }

