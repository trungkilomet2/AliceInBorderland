using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillCooldownUI : MonoBehaviour
{
    public Image cooldownMaskImage;      // phần mask quay
    public TextMeshProUGUI cooldownText; // số đếm thời gian

    private float cooldownTime = 30f;
    private float currentCooldown = 0f;
    private bool isOnCooldown = false;

    public void TriggerCooldown(float time)
    {
        cooldownTime = time;
        currentCooldown = time;
        isOnCooldown = true;

        // Hiện mask và text khi bắt đầu hồi chiêu
        cooldownMaskImage.gameObject.SetActive(true);
        cooldownText.gameObject.SetActive(true);

        cooldownMaskImage.fillAmount = 1f;
    }

    void Update()
    {
        if (!isOnCooldown) return;

        currentCooldown -= Time.deltaTime;
        if (currentCooldown <= 0f)
        {
            // Chiêu hồi xong
            isOnCooldown = false;
            cooldownMaskImage.fillAmount = 0f;

            // Ẩn mask và text
            cooldownMaskImage.gameObject.SetActive(false);
            cooldownText.gameObject.SetActive(false);
            return;
        }

        // Đang hồi chiêu: cập nhật mask và số
        cooldownMaskImage.fillAmount = currentCooldown / cooldownTime;
        cooldownText.text = Mathf.CeilToInt(currentCooldown).ToString();
    }
}
