using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterCommonBehavior : MonoBehaviour
{

    public abstract float moveSpeed { get; set; }
    public SkillBase[] skills;
    public float hp;
    private Vector3 moveInput;
    private Rigidbody2D rb;
    private Animator animator;
    private GameObject damageTextPrefab;
    private const string COIN_TAG = "Coin";
    private const string EXP_TAG = "EXP";
    public CommonUI commonUI;


    private void Awake()
    {
        damageTextPrefab = Resources.Load<GameObject>("Prefabs/DamageText"); // Load the damage text prefab from Resources folder
        animator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {   
        rb = GetComponent<Rigidbody2D>();
        commonUI = FindAnyObjectByType<CommonUI>();
        commonUI.SetExp(0, 100f);
        commonUI.levelText.text = "Level: " + commonUI.currentLevel.ToString();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        Move();
        UpdateAnimation();
        commonUI.levelText.text = "Level: " + commonUI.currentLevel.ToString();

        // Use the new skill input handling flow
        if (skills != null && skills.Length > 0 && skills[0] != null)
        {
            for (int i = 0; i < skills.Length; i++)
            {
                if (skills[i] != null)
                {
                    skills[i].HandleSkillInput();
                }
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == COIN_TAG)
        {
            Destroy(collision.gameObject);
            // Xu ly add them playprefabs
        }
        if (collision.tag == EXP_TAG)
        {
            Destroy(collision.gameObject);
            commonUI.AddExp(10f);
        }
    }

    protected virtual void Move()
    {
        moveInput.x = Input.GetAxis("Horizontal");
        moveInput.y = Input.GetAxis("Vertical");
        transform.position += moveInput * moveSpeed * Time.deltaTime;

        if (moveInput.x != 0)
        {
            if (moveInput.x != 0)
            {
                Vector3 scale = transform.localScale;
                scale.x = moveInput.x > 0 ? Mathf.Abs(scale.x) : -Mathf.Abs(scale.x);
                transform.localScale = scale;
            }
        }
    }

    private void UpdateAnimation()
    {
        bool isRunning = Mathf.Abs(moveInput.x) > 0.01f || Mathf.Abs(moveInput.y) > 0.01f;
        animator.SetBool("isRunning", isRunning);
    }

    internal void TakeDamage(float damage)
    {
        ShowDamageText(damage);
        hp -= damage;
        if (hp <= 0)
        {
            Destroy(gameObject);
            Time.timeScale = 0f;
        }
        animator.SetTrigger("takeHit");
    }


    private void ShowDamageText(float damage)
    {
        Vector3 spawnPos = transform.position + new Vector3(0, 1f, 0); // bay lên đầu enemy

        GameObject dmgTextObj = Instantiate(damageTextPrefab, spawnPos, Quaternion.identity);

        DamageText dmgText = dmgTextObj.GetComponent<DamageText>();
        dmgText.SetDamage(damage);
    }

    public abstract void Attack();
}
