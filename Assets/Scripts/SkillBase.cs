using UnityEngine;

public enum IndicatorType { Circle, Arrow }
public enum SkillNum { Skill1, Skill2, Skill3, Skill4 }
public enum SkillType { Active, Passive }
public abstract class SkillBase : MonoBehaviour
{
    public SkillNum skillNum;
    public SkillType skillType = SkillType.Active;
    public IndicatorType indicatorType = IndicatorType.Circle; 
    public string skillName;
    public float cooldown = 5f;
    public float skillRange = 10f;
    public float skillWidth = 1f;
    public float skillDuration = 2f;

    protected float currentCooldown = 0f;
    protected bool isCoolingDown = false;
    protected Transform skillTransform;
    protected GameObject indicatorInstance;
    protected GameObject rangeIndicatorInstance;

    private GameObject indicatorPrefab;
    private GameObject rangeIndicatorPrefab;
    private KeyCode key;
    private bool isPreparingSkill = false;

    // Static field to track if any skill is being prepared or executed
    private static SkillBase currentActiveSkill = null;

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

        switch(skillNum)
        {
            case SkillNum.Skill1:
                key = KeyCode.Alpha1;
                break;
            case SkillNum.Skill2:
                key = KeyCode.Alpha2;
                break;
            case SkillNum.Skill3:
                key = KeyCode.Alpha3;
                break;
            case SkillNum.Skill4:
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

        // Prevent preparing/executing another skill if one is already being prepared or executed
        if (currentActiveSkill != null && currentActiveSkill != this)
        {
            return;
        }

        // Passive skill: activate immediately, no indicator
        if (skillType == SkillType.Passive && !isCoolingDown && Input.GetKeyDown(key))
        {
            currentActiveSkill = this;
            Activate();
            isCoolingDown = true;
            currentCooldown = cooldown;
            currentActiveSkill = null;
            return;
        }

        // Active skill: show indicator and prepare
        if (skillType == SkillType.Active && !isCoolingDown && !isPreparingSkill && Input.GetKeyDown(key))
        {
            ShowIndicators();
            isPreparingSkill = true;
            currentActiveSkill = this;
        }
    }

    public virtual void Update()
    {
        // Only handle input if preparing the skill (active only)
        if (isPreparingSkill && skillType == SkillType.Active)
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
        }
    }

    private void ShowIndicators()
    {
        if (indicatorPrefab != null && indicatorInstance == null)
        {
            indicatorInstance = Instantiate(indicatorPrefab, transform.position, Quaternion.identity);
            if (indicatorType == IndicatorType.Arrow)
            {
                indicatorInstance.transform.localScale = new Vector3(skillRange, skillWidth, 1f);
            }
            else if (indicatorType == IndicatorType.Circle)
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
            if (indicatorType == IndicatorType.Arrow)
            {
                indicatorInstance.transform.position = transform.position;
                Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector3 direction = mouseWorld - transform.position;
                direction.z = 0f;
                if (direction.sqrMagnitude > 0.001f)
                {
                    float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                    indicatorInstance.transform.rotation = Quaternion.Euler(0f, 0f, angle);
                }
            }
            else if (indicatorType == IndicatorType.Circle)
            {
                Vector3 mouseScreen = Input.mousePosition;
                mouseScreen.z = Mathf.Abs(Camera.main.transform.position.z);
                Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(mouseScreen);
                mouseWorld.z = 0;
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
        skillTransform = indicatorInstance != null ? indicatorInstance.transform : null;
        HideIndicators();
        Activate();
        isCoolingDown = true;
        currentCooldown = cooldown;
        isPreparingSkill = false;
        currentActiveSkill = null;
    }

    public virtual void CancelSkill()
    {
        HideIndicators();
        isPreparingSkill = false;
        if (currentActiveSkill == this)
            currentActiveSkill = null;
    }

    protected abstract void Activate();
}
