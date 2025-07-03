using UnityEngine;
using UnityEngine.UI;

public abstract class CoolDownBase : MonoBehaviour
{
    [Header("Cooldown Settings")]
    public float cooldownTime = 5f; // Thời gian hồi chiêu
    protected float currentCooldown = 0f;
    protected bool isCoolingDown = false;

    [Header("UI")]
    public Image cooldownImage; // Image có fill

    GameObject player;
    SkillBase[] skillBase;

    protected virtual void Start()
    {
        if (cooldownImage != null)
        {
            cooldownImage.fillAmount = 0f;
            Debug.Log("Da fill Amount");
        }
    }

    protected virtual void Update()
    {
        CheckSkillActive();
        if (isCoolingDown)
        {
            currentCooldown -= Time.deltaTime;
            Debug.Log("Current Cooldown: " + currentCooldown);
            UpdateCooldownUI();

            if (currentCooldown <= 0f)
            {
                EndCooldown();
            }
        }
    }

    public virtual void StartCooldown()
    {
        isCoolingDown = true;
        currentCooldown = cooldownTime;
        if (cooldownImage != null)
            cooldownImage.fillAmount = 0f;
    }

    protected virtual void UpdateCooldownUI()
    {
        if (cooldownImage != null)
        {
            cooldownImage.fillAmount = currentCooldown / cooldownTime;
        }
    }

    protected virtual void EndCooldown()
    {
        isCoolingDown = false;
        currentCooldown = 0f;
        if (cooldownImage != null)
            cooldownImage.fillAmount = 0f;
    }

    public bool IsCooldownActive()
    {
        return isCoolingDown;
    }
    public void CheckSkillActive()
    {
        player = GameObject.FindWithTag("Player");
        skillBase = player?.GetComponents<SkillBase>();
        SkillNum numberOfSkill = SkillNum.Skill1;
        foreach (SkillBase skill in skillBase)
        {
            if (numberOfSkill == skill.skillNum)
            {
                float cooldownTime;
                skill.UnlockSkillBySkillNum(numberOfSkill);
                cooldownTime = skill.GetCurrentCooldown();
                if (skill.GetIsCoolingDown())
                {
                    StartCooldown();
                }
            }
        }
    }

}
