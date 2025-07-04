using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public abstract class CoolDownBase : MonoBehaviour
{
    [Header("Cooldown Settings")]
    public float cooldownTime = 999f; // Thời gian hồi chiêu
    protected float currentCooldown = 0f;
    protected bool isCoolingDown = false;
    
    [Header("UI")]
    public Image cooldownImage; // Image có fill 

    protected virtual void Start()
    {
        if (cooldownImage != null)
        {
            cooldownImage.fillAmount = 0f;
        }
    }

    protected virtual void Update()
    {
        CheckSkillActive();
       // CheckingPlayer();
        if (isCoolingDown)
        {
            currentCooldown -= Time.deltaTime;
         //  Debug.Log("Current Cooldown: " + currentCooldown);
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
        Debug.Log(currentCooldown);
        if (cooldownImage != null)
            cooldownImage.fillAmount = 1f;
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


    public abstract void CheckSkillActive();


}
