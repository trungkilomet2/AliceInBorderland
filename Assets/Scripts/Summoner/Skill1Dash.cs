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

    [Header("Ghost Effect")]
    public GhostImage ghostEffect;

    public override void Awake()
    {
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

        // Lấy hướng dash từ indicator nếu có, ngược lại lấy hướng chuột (PC)
        Vector2 dashDir = Vector2.right;

        if (indicatorInstance != null)
        {
            // Có thể dùng hướng (indicatorInstance.position - player.position) nếu indicator là object di chuyển
            // Hoặc transform.right nếu indicator chỉ là mũi tên quay quanh player
            dashDir = (indicatorInstance.transform.position - transform.position).normalized;
            if (dashDir.sqrMagnitude < 0.01f)
                dashDir = indicatorInstance.transform.right.normalized;
        }
        else
        {
            // Nếu không có indicator, lấy hướng chuột (PC)
            Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseWorld.z = 0;
            dashDir = (mouseWorld - transform.position).normalized;
        }

        dashDirection = dashDir;
        dashTarget = transform.position + (Vector3)(dashDirection * skillRange);

        // Play dash animation nếu có
        if (animator != null)
            animator.SetTrigger("roll");

        isDashing = true;
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
        }
    }
}
