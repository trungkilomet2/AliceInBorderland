using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Q_Boss_Skill1 : BossSkillBase
{
    public float dashSpeed = 40f; // Tốc độ dash mặc định
    private float originalDashSpeed; // Để lưu lại tốc độ gốc
    public float dashDistance = 50f;

    private Vector3 dashTarget;
    private bool isDashingInternal = false;
    public static bool isDashing = false;

    private int dashCount = 0; // Đếm số lần dash liên tiếp

    protected override void Awake()
    {
        base.Awake();
        originalDashSpeed = dashSpeed; // Lưu tốc độ ban đầu
    }

    public void Update()
    {
        base.Update();

        if (isDashingInternal)
        {
            transform.position = Vector3.MoveTowards(transform.position, dashTarget, dashSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, dashTarget) < 0.1f)
            {
                StopDash();
            }
        }
    }

    protected override void Activate()
    {
        if (isDashingInternal) return;

        if (target == null)
        {
            Debug.LogWarning("Dash thất bại: thiếu target.");
            return;
        }

        Debug.Log("BOSS DASH!");

        dashCount++;

        if (dashCount == 4)
        {
            dashSpeed *= 4;
            Debug.Log("🔥 Dash tăng tốc gấp đôi!");
        }

        Vector3 direction = (target.transform.position - transform.position).normalized;
        dashTarget = transform.position + (direction * dashDistance);

        isDashingInternal = true;
        isDashing = true;
    }

    private void StopDash()
    {
        if (isDashingInternal)
        {
            Debug.Log("Boss Dash Ended.");

            isDashingInternal = false;
            isDashing = false;

            // Nếu vừa dash gấp đôi thì reset lại tốc độ và đếm
            if (dashCount >= 4)
            {
                dashSpeed = originalDashSpeed;
                dashCount = 0; // Reset đếm sau khi dash đặc biệt
                Debug.Log("🔁 DashSpeed đã được reset.");
            }
        }
    }
}
