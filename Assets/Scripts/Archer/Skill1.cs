using UnityEngine;

public class Skill1 : SkillBase
{
    public float rollSpeed = 20f;

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

        // Handle rolling movement
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
        // When Activate is called (from SkillBase), choose direction and roll
        if (isRolling) return;

        // Calculate direction from player to mouse
        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorld.z = 0;
        rollDirection = (mouseWorld - transform.position).normalized;

        // Set roll target
        rollTarget = transform.position + (Vector3)(rollDirection * skillRange);

        // Play roll animation
        if (animator != null)
            animator.SetTrigger("roll");

        isRolling = true;
    }
}