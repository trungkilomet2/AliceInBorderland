using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Q_Boss_Skill1 : BossSkillBase
{
    public float dashSpeed = 40f; // Đổi tên để dễ hiểu hơn, giống Player
    public float dashDistance = 50f; // Thay thế skillRange nếu bạn muốn dash một khoảng cố định
                                     // Hoặc vẫn dùng skillRange nếu bạn muốn dash đến gần người chơi

    private Vector3 dashTarget; // Điểm đến của cú dash
    private bool isDashingInternal = false; // Biến cờ nội bộ cho trạng thái dash của skill này
    // isDashing static vẫn giữ nguyên để các script khác kiểm tra trạng thái của boss
    public static bool isDashing = false;

    // Không cần Rigidbody2D nữa nếu chỉ dùng transform.position
    // private Rigidbody2D rb;

    protected override void Awake()
    {
        base.Awake();
        // Không cần lấy Rigidbody2D nếu không sử dụng nó cho dash
        // rb = GetComponent<Rigidbody2D>();
    }

    // Thêm hàm Update để xử lý di chuyển liên tục khi đang dash
    public void Update() // SkillBase có thể đã có Update, nếu không thì thêm override
    {
        base.Update(); // Quan trọng để gọi Update của BossSkillBase

        if (isDashingInternal)
        {
            // Di chuyển boss về phía dashTarget
            transform.position = Vector3.MoveTowards(transform.position, dashTarget, dashSpeed * Time.deltaTime);

            // Kiểm tra nếu đã gần đến đích
            if (Vector3.Distance(transform.position, dashTarget) < 0.1f)
            {
                StopDash(); // Dừng dash khi đến đích
            }
        }
    }

    protected override void Activate()
    {
        if (isDashingInternal) return; // Ngăn không cho dash nếu đang dash

        if (target == null)
        {
            Debug.LogWarning("Dash thất bại: thiếu target.");
            return;
        }

        Debug.Log("BOSS DASH!");

        Vector3 direction = (target.transform.position - transform.position).normalized;

        // Tính toán điểm đích của cú dash
        // Dash tới một điểm cách vị trí hiện tại một khoảng skillRange theo hướng về người chơi
        dashTarget = transform.position + (direction * dashDistance); // Hoặc (direction * skillRange)

        // Cập nhật biến trạng thái
        isDashingInternal = true;
        isDashing = true; // Cập nhật biến static để các script khác biết boss đang dash

        // Nếu có Animator, bạn có thể kích hoạt animation dash ở đây
        // if (animator != null)
        //     animator.SetTrigger("dashTrigger");
    }

    // Hàm riêng để dừng dash, có thể gọi khi đến đích hoặc khi va chạm
    private void StopDash()
    {
        if (isDashingInternal)
        {
            Debug.Log("Boss Dash Ended.");
            isDashingInternal = false;
            isDashing = false; // Tắt biến static
            // Đặt lại vận tốc về 0 nếu boss có Rigidbody2D và nó đang bị ảnh hưởng bởi vận tốc từ nơi khác
            // if (rb != null) rb.velocity = Vector2.zero;
        }
    }
}
