using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageSkill4 : SkillBase
{
    public float teleportDistance = 10f; // Khoảng cách dịch chuyển tối đa
    public LayerMask obstaclesLayer; // Layer của các vật cản để tránh dịch chuyển vào

    public override void Awake()
    {
        base.Awake();

        skillNum = SkillNum.Skill4; // Gán SkillNum là Skill4
        skillType = SkillType.Active; // Đây là kỹ năng chủ động
        indicatorType = IndicatorType.Circle; // Sử dụng chỉ báo hình tròn để chọn vị trí

        cooldown = 10f; // Thời gian hồi chiêu
        skillRange = teleportDistance; // Tầm sử dụng skill (tầm dịch chuyển)
        skillWidth = 1f; // Độ lớn của indicator hình tròn (có thể điều chỉnh để phù hợp)
        skillDuration = 0f; // Kỹ năng tức thời, không có duration
        skillDamage = 0f; // Kỹ năng không gây sát thương
    }

    protected override void Activate()
    {
        // Lấy vị trí được chọn bởi người chơi từ skillTransform
        // Đảm bảo targetPosition luôn là Vector3
        Vector3 targetPosition = skillTransform != null ? skillTransform.position : transform.position;

        // Tính toán hướng dịch chuyển từ vị trí hiện tại đến vị trí được chọn
        // Cả hai toán hạng đều là Vector3, không có lỗi ở đây.
        Vector3 direction = (targetPosition - transform.position);

        // Giới hạn khoảng cách dịch chuyển không vượt quá skillRange
        if (direction.magnitude > skillRange)
        {
            targetPosition = transform.position + direction.normalized * skillRange;
            // Recalculate direction if targetPosition was clamped
            direction = (targetPosition - transform.position);
        }

        // Kiểm tra xem có vật cản nào trên đường dịch chuyển hoặc tại điểm đến không
        // Sử dụng Raycast để kiểm tra đường đi
        // Cast transform.position (Vector3) to Vector2 for Physics2D.Raycast if needed,
        // or ensure your scene is 2D and transform.position.z is consistently 0.
        // For 2D games, it's often safer to work with Vector2 for Physics2D.
        RaycastHit2D hit = Physics2D.Raycast((Vector2)transform.position, (Vector2)direction.normalized, direction.magnitude, obstaclesLayer);

        if (hit.collider != null)
        {
            // Nếu có vật cản, dịch chuyển đến ngay trước vật cản
            Debug.Log($"Teleport blocked by {hit.collider.name}. Teleporting to obstacle edge.");
            // hit.point is Vector2. Explicitly convert it to Vector3 for subtraction with Vector3.
            // Or convert the direction.normalized (Vector3) to Vector2 as well
            transform.position = (Vector3)hit.point - ((Vector3)direction.normalized * 0.1f);
        }
        else
        {
            // Không có vật cản, dịch chuyển đến vị trí đã chọn
            transform.position = targetPosition;
        }

        Debug.Log($"Player teleported to: {transform.position}");
    }
}