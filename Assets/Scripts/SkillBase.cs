using UnityEngine;

public enum IndicatorType { Circle, Arrow }
public enum SkillType { Skill1, Skill2, Skill3, Skill4 }
public abstract class SkillBase : MonoBehaviour
{
    public string skillName;
    public float cooldown = 5f;
    public float skillRange = 10f;
    public float skillWidth = 1f;
    public SkillType skillType;
    public IndicatorType indicatorType = IndicatorType.Circle; 

    protected float currentCooldown = 0f;
    protected bool isCoolingDown = false;

    private GameObject indicatorPrefab;
    private GameObject indicatorInstance;
    private GameObject rangeIndicatorPrefab;
    private GameObject rangeIndicatorInstance;
    private KeyCode key;
    private bool isPreparingSkill = false;

    public virtual void Awake()
    {
        rangeIndicatorPrefab = Resources.Load<GameObject>("Prefabs/RangeIndicator");
        if (indicatorType == IndicatorType.Arrow)
        {
            indicatorPrefab = Resources.Load<GameObject>("Prefabs/ArrowIndicator");
        }
        else if (indicatorType == IndicatorType.Circle)
        {
            indicatorPrefab = Resources.Load<GameObject>("Prefabs/CircleIndicator");
        }

        switch(skillType)
        {
            case SkillType.Skill1:
                key = KeyCode.Alpha1;
                break;
            case SkillType.Skill2:
                key = KeyCode.Alpha2;
                break;
            case SkillType.Skill3:
                key = KeyCode.Alpha3;
                break;
            case SkillType.Skill4:
                key = KeyCode.Alpha4;
                break;
            default:
                key = KeyCode.None; // Default to no key
                break;
        }
    }

    public virtual void HandleSkillInput()
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

        // Start preparing skill if not cooling down and key pressed
        if (!isCoolingDown && !isPreparingSkill && Input.GetKeyDown(key))
        {
            ShowIndicators();
            isPreparingSkill = true;
        }
    }

    public virtual void Update()
    {
        // Only handle input if preparing the skill
        if (isPreparingSkill)
        {
            // Cancel skill with right mouse button
            if (Input.GetMouseButtonDown(1))
            {
                CancelSkill();
            }
            // Execute skill with left mouse button
            else if (Input.GetMouseButtonDown(0))
            {
                ExecuteSkill();
            }
            UpdateIndicators();
            UpdateIndicatorDirection();
        }
    }

    private void ShowIndicators()
    {
        if (indicatorPrefab != null && indicatorInstance == null)
        {
            indicatorInstance = Instantiate(indicatorPrefab, transform.position, Quaternion.identity);
            if (indicatorType == IndicatorType.Arrow) // Fixed enum reference
            {
                indicatorInstance.transform.localScale = new Vector3(skillRange, skillWidth, 1f);
            }
            else if (indicatorType == IndicatorType.Circle) // Fixed enum reference
            {
                indicatorInstance.transform.localScale = new Vector3(skillWidth, skillWidth, 1f);
            }
        }
        if (rangeIndicatorPrefab != null && rangeIndicatorInstance == null)
        {
            rangeIndicatorInstance = Instantiate(rangeIndicatorPrefab, transform.position, Quaternion.identity);
            rangeIndicatorInstance.transform.localScale = new Vector3(skillRange * 2, skillRange * 2, 1f);
        }
    }

    private void UpdateIndicators()
    {
        if (indicatorInstance != null)
        {
            if (indicatorType == IndicatorType.Arrow) // Fixed enum reference
            {
                indicatorInstance.transform.position = transform.position;
            }
            else if (indicatorType == IndicatorType.Circle) // Fixed enum reference
            {
                // Update circle indicator position if needed
                Vector3 mouseScreen = Input.mousePosition;
                mouseScreen.z = Mathf.Abs(Camera.main.transform.position.z); // fix z để ray ra đúng mặt phẳng

                Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(mouseScreen);
                mouseWorld.z = 0; // vì bạn đang dùng 2D

                float distance = Vector3.Distance(transform.position, mouseWorld);
                if (distance > skillRange)
                {
                    Vector3 direction = (mouseWorld - transform.position).normalized * skillRange;
                    indicatorInstance.transform.position = transform.position + direction;
                }
                else
                {
                    indicatorInstance.transform.position = mouseWorld;
                }
            }
        }
        if (rangeIndicatorInstance != null)
        {
            rangeIndicatorInstance.transform.position = transform.position;
        }
    }

    // New: Update indicator direction to face mouse
    private void UpdateIndicatorDirection()
    {
        if (indicatorInstance != null)
        {
            Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 direction = mouseWorld - transform.position;
            direction.z = 0f;
            if (direction.sqrMagnitude > 0.001f)
            {
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                indicatorInstance.transform.rotation = Quaternion.Euler(0f, 0f, angle);
            }
        }
    }

    protected void HideIndicators()
    {
        if (indicatorInstance != null)
        {
            Destroy(indicatorInstance);
            indicatorInstance = null;
        }
        if (rangeIndicatorInstance != null)
        {
            Destroy(rangeIndicatorInstance);
            rangeIndicatorInstance = null;
        }
    }

    private void ExecuteSkill()
    {
        HideIndicators();
        Activate();
        isCoolingDown = true;
        currentCooldown = cooldown;
        isPreparingSkill = false;
    }

    public virtual void CancelSkill()
    {
        HideIndicators();
        isPreparingSkill = false;
    }

    protected abstract void Activate();
}
