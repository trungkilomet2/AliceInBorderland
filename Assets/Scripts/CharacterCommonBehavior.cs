using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterCommonBehavior : MonoBehaviour
{

    public abstract float moveSpeed { get; set; }
    public SkillBase[] skills;
    public float hp;

    private Vector3 moveInput;
    private Rigidbody2D rb;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        Move();
        UpdateAnimation();

        // Use the new skill input handling flow
        if (skills != null && skills.Length > 0 && skills[0] != null)
        {
            for (int i = 0; i < skills.Length; i++)
            {
                if (skills[i] != null)
                {
                    skills[i].HandleSkillInput();
                }
            }
        }
    }

    protected virtual void Move()
    {
        moveInput.x = Input.GetAxis("Horizontal");
        moveInput.y = Input.GetAxis("Vertical");
        transform.position += moveInput * moveSpeed * Time.deltaTime;

        if (moveInput.x != 0)
        {
            if (moveInput.x != 0)
            {
                Vector3 scale = transform.localScale;
                scale.x = moveInput.x > 0 ? Mathf.Abs(scale.x) : -Mathf.Abs(scale.x);
                transform.localScale = scale;
            }
        }
    }

    private void UpdateAnimation()
    {
        bool isRunning = Mathf.Abs(moveInput.x) > 0.01f || Mathf.Abs(moveInput.y) > 0.01f;
        animator.SetBool("isRunning", isRunning);
    }

    internal void TakeDamage(float damage)
    {
        hp -= damage;
        if (hp <= 0)
        {
            Destroy(gameObject);
            Time.timeScale = 0f;
        }
        animator.SetTrigger("takeHit");
    }

    public abstract void Attack();
}
