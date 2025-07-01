using Cinemachine;
using System;
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
    protected Animator animator;
    private GameObject damageTextPrefab;
    private const string COIN_TAG = "Coin";
    private const string EXP_TAG = "EXP";
    public CommonUI commonUI;
    private float onMovingCharacterHorizontal;
    private const string ENERMY_WEAPON = "Enemy_Weapon";
    public AudioClip attackSound;

    private bool isInvincible = false;
    private float invincibleEndTime = 0f;
    private bool canDie = false;
    public void BlockDeath() => canDie = false;
    public void AllowDeath() => canDie = true;
    public bool CanDie() => canDie;
    private Skill1_Warrior skill1;

    // Add 28.06/2025 |Quang Anh|  Lưu vị trí an toàn để tránh bị kẹt trong Block
    private Vector3 lastSafePosition;
    private float positionRecordInterval = 0.5f;
    private float lastPositionRecordTime = 0f;
    public const string BLOCK_TAG = "Block";
    public static event Action OnBlockedCollision;
    public float damageReductionMultiplier = 1f;

    //audio
    [HideInInspector]
    public AudioManager audioManager;


    private void Awake()
    {
        damageTextPrefab = Resources.Load<GameObject>("Prefabs/DamageText"); // Load the damage text prefab from Resources folder
        animator = GetComponent<Animator>();
        audioManager = FindAnyObjectByType<AudioManager>();
    }

    public void DefaultCommonUI()
    {
        commonUI = FindAnyObjectByType<CommonUI>();
        commonUI.SetExp(0, 100f);
        commonUI.levelText.text = "Level: " + commonUI.currentLevel.ToString();
        commonUI.SetCurrentHp(hp);
        commonUI.SetMaxHp(hp);
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        DefaultCommonUI();

        // Add 28.06/2025 |Quang Anh|  Ghi lại vị trí an toàn ban đầu
        lastSafePosition = transform.position;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        Move();
        UpdateAnimation();
        commonUI.levelText.text = "Level: " + commonUI.currentLevel.ToString();

        // Add 28.06/2025 |Quang Anh| Ghi lại vị trí an toàn mỗi 3 giây
        if (Time.time - lastPositionRecordTime >= positionRecordInterval)
        {
            lastSafePosition = transform.position;
            lastPositionRecordTime = Time.time;
        }

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
            audioManager.PlayCoinSound();
            Destroy(collision.gameObject);
            // Xu ly add them playprefabs
        }
        if (collision.tag == EXP_TAG)
        {
            audioManager.PlayCoinSound();
            Destroy(collision.gameObject);
            commonUI.AddExp(30f);
        }
        if (collision.tag == ENERMY_WEAPON)
        {
            WeaponBase weaponBase = collision.GetComponent<WeaponBase>();
            if (weaponBase != null)
            {
                TakeDamage(weaponBase.damage);
                if (!weaponBase.isThought)
                {
                    Destroy(collision.gameObject);
                }
            }
        }
        // Add 28.06/2025 |Quang Anh| === Thêm xử lý Block ===
        if (collision.CompareTag(BLOCK_TAG))
        {
            Debug.Log("==> Đã chạm Block. Quay lại vị trí cũ.");

            transform.position = lastSafePosition;
            OnBlockedCollision?.Invoke();
        }
    }

    protected virtual void Move()
    {
        moveInput.x = Input.GetAxis("Horizontal");
        moveInput.y = Input.GetAxis("Vertical");
        transform.position += moveInput * moveSpeed * Time.deltaTime;

        if (moveInput.x != 0)
        {
            onMovingCharacterHorizontal = moveInput.x;
            if (moveInput.x != 0)
            {
                Vector3 scale = transform.localScale;
                scale.x = moveInput.x > 0 ? Mathf.Abs(scale.x) : -Mathf.Abs(scale.x);
                transform.localScale = scale;
            }
        }
    }

    public float GetOnMovingCharacterHorizontal()
    {
        return this.onMovingCharacterHorizontal;
    }

    public Vector3 GetMoveInput()
    {
        return this.moveInput;
    }

    private void UpdateAnimation()
    {
        bool isRunning = Mathf.Abs(moveInput.x) > 0.01f || Mathf.Abs(moveInput.y) > 0.01f;
        animator.SetBool("isRunning", isRunning);
    }

    internal void TakeDamage(float damage)
    {
        skill1 = GetComponent<Skill1_Warrior>();
        if (skill1 != null)
        {
            damage = skill1.OnAbsorbDamage(damage);
        // Nếu damage bị phản lại toàn bộ, bạn có thể return nếu muốn
        if (damage <= 0) return;
        }
        if (isInvincible) return;
        damage *= damageReductionMultiplier;
        ShowDamageText(damage);
        hp -= damage;
        commonUI.SetCurrentHp(hp);
        commonUI.UpdateHealthBar();
        if (hp <= 0)
        {
            audioManager.PlayGameOverSound();
            if ( this is Warrior && !CanDie()) return;
            else
            {
                AllowDeath();
                 Destroy(gameObject);
                Time.timeScale = 0f;
            }
           
        }
        animator.SetBool("isHit", true);
        Invoke("ResetHitAnimation", 0.5f); // Reset hit animation after 0.5 seconds
    }

    private void ResetHitAnimation()
    {
        animator.SetBool("isHit", false);
    }


    private void ShowDamageText(float damage)
    {
        Vector3 spawnPos = transform.position + new Vector3(0, 1f, 0); // bay lên đầu enemy

        GameObject dmgTextObj = Instantiate(damageTextPrefab, spawnPos, Quaternion.identity);

        DamageText dmgText = dmgTextObj.GetComponent<DamageText>();
        dmgText.SetDamage(damage);
    }

    public Rigidbody2D GetRigidbody2D()
    {
        return rb;
    }

    public void ActiveNeckleItem(float duration)
    {
        isInvincible = true;
        invincibleEndTime = Time.time + duration;
    }
    public void ActiveSkillInvincible(float duration)
    {
        isInvincible = true;
        invincibleEndTime = Time.time + duration;
    }
    public void DeactiveInvincible()
    {
        isInvincible = false;
    }

    public float GetInvincibleEndTime()
    {
        return this.invincibleEndTime;
    }

    public void DeactiveNeckleItem()
    {
        isInvincible = false;
    }



    public abstract void Attack();


}
