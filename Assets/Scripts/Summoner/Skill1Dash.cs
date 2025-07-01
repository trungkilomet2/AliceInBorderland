using System.Collections;
using UnityEngine;

public class Skill1Dash : SkillBase
{
    public float dashForce = 15f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 4f;

    private Rigidbody2D rb;
    private bool isDashing = false;
    private float cooldownTimer = 0f;

    public SkillCooldownUI skill1UI;

    private Vector2 lastMoveDir;

    [Header("Ghost Effect")]
    public GhostImage ghostEffect;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (cooldownTimer > 0f)
            cooldownTimer -= Time.deltaTime;


        // Lưu hướng di chuyển cuối cùng
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        if (input != Vector2.zero)
        {
            lastMoveDir = input.normalized;
        }

        // Kích hoạt dash
        if ((Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1)) && !isDashing && cooldownTimer <= 0f)
        {
            StartCoroutine(Dash());
            skill1UI.TriggerCooldown(dashCooldown);
        }
    }

    IEnumerator Dash()
    {
        isDashing = true;
        cooldownTimer = dashCooldown;

        if (ghostEffect != null)
        {
            StartCoroutine(ghostEffect.SpawnGhosts(dashDuration)); // Gọi hiệu ứng ghost song song
        }

        rb.velocity = lastMoveDir * dashForce;

        yield return new WaitForSeconds(dashDuration);

        rb.velocity = Vector2.zero; // Reset tốc độ sau khi dash

        isDashing = false;
    }
}
