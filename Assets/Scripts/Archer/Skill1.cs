using UnityEngine;

public class Skill1 : SkillBase
{

    //Skill 1:
    public float rollDistance = 5f;
    public float rollSpeed = 20f;
    public GameObject arrowIndicatorPrefab;

    private bool isChoosingDirection = false;
    private Vector2 rollDirection;
    private bool isRolling = false;
    private Vector3 rollTarget;
    private GameObject arrowInstance;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        HandleSkillInput(KeyCode.Alpha1);

        if (isRolling)
        {
            transform.position = Vector3.MoveTowards(transform.position, rollTarget, rollSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.position, rollTarget) < 0.1f)
            {
                isRolling = false;
            }
            return;
        }

        if (isChoosingDirection)
        {
            UpdateArrowDirection();

            if (Input.GetMouseButtonDown(0))
            {
                rollDirection = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized;
                HideArrowIndicator();
                StartRoll();
            }

            if (Input.GetMouseButtonDown(1))
            {
                HideArrowIndicator();
            }
        }
    }



    protected override void Activate()
    {
        if (isChoosingDirection || isRolling) return;
        isChoosingDirection = true;
        ShowArrowIndicator();
    }


    void ShowArrowIndicator()
    {
        arrowInstance = Instantiate(arrowIndicatorPrefab, transform.position, Quaternion.identity);
        arrowInstance.transform.localScale = new Vector3(rollDistance, 1f, 1f);
    }



    void UpdateArrowDirection()
    {
        if (arrowInstance != null)
        {
            Vector3 dir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            dir.z = 0;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            arrowInstance.transform.position = transform.position;
            arrowInstance.transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }

    void HideArrowIndicator()
    {
        if (arrowInstance != null)
        {
            Destroy(arrowInstance);
        }
        isChoosingDirection = false;
    }

    void StartRoll()
    {
        if (isRolling) return;
        animator.SetTrigger("roll");

        Vector2 rollDir = arrowInstance != null
            ? arrowInstance.transform.right.normalized
            : rollDirection;

        rollTarget = transform.position + (Vector3)(rollDir * rollDistance);
        isRolling = true;
    }

}