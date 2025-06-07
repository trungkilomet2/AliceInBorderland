using UnityEngine;

public abstract class SkillBase : MonoBehaviour
{
    public string skillName;
    public float cooldown = 5f;

    protected float currentCooldown = 0f;
    protected bool isCoolingDown = false;

    public virtual void HandleSkillInput(KeyCode key)
    {
        // Update cooldown timer
        if (isCoolingDown)
        {
            currentCooldown -= Time.deltaTime;
            if (currentCooldown <= 0f)
            {
                isCoolingDown = false;
                currentCooldown = 0f;
            }
        }

        // Only allow skill use if not cooling down
        if (!isCoolingDown && Input.GetKeyDown(key))
        {
            UseSkill();
        }
    }

    public void UseSkill()
    {
        if (isCoolingDown)
            return;

        Activate();

        isCoolingDown = true;
        currentCooldown = cooldown;
    }

    protected abstract void Activate();
}
