using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class Skill4_Warrior : SkillBase
{
    public static float reviveHpPercent = 0.5f;   // Hồi lại 50% máu tối đa
    private float reviveHp = 0f;
    public float damageMultiplier = 1.5f;  // Tăng sát thương
    public float buffDuration = 45f;       // Hiệu lực buff
    public SharedWarriorState sharedState;
    private bool skillUsed = false;        // Đảm bảo chỉ dùng 1 lần khi chết
    private float buffTimer = 0f;
    private float originDamage = 0f;
    private float damageBuff = 0f;
    private CharacterCommonBehavior character;
    private WeaponBase weapon;

    public GameObject gameObject;
    public GameObject revivePrefabs;
    private Animator animator;
    private Enemy enemy;
    private bool isCharacterInvincible = false;
    public float invincibleDuration = 3f;
    private float InvincibleCd = 3f;
    private float CharacterInvincibleEndTime = 0f;

    public override void Awake()
    {
        base.Awake();
        character = GetComponent<CharacterCommonBehavior>();
        weapon = GetComponent<WeaponBase>();
        enemy = GetComponent<Enemy>();
    }
    protected override void Activate()
    {
        if (character == null || skillUsed || sharedState.hasUsedSkill4) return;

        skillUsed = true;
        sharedState.hasUsedSkill4 = true;
        // Nhân vật bất tử
        isCharacterInvincible = true;
        character.ActiveSkillInvincible(invincibleDuration);
        CharacterInvincibleEndTime = Time.time + invincibleDuration;

        // Hồi máu
        character.hp = reviveHp;

        // Tăng sát thương

        weapon.damage = damageBuff;
        // Bắt đầu timer buff
        buffTimer = buffDuration;
        createExplosion();
    }
    // Start is called before the first frame update
    void Start()
    {
        weapon = gameObject.GetComponent<WeaponBase>();
        animator = GetComponent<Animator>();
        reviveHp = character.hp * reviveHpPercent;
        damageBuff = weapon.damage * damageMultiplier;
        originDamage = weapon.damage;
        if (sharedState.hasUsedSkill4)
        {
            character.AllowDeath();
        }

    }
    // Update is called once per frame
    void Update()
    {
        if (character == null) return;

        // Kiểm tra nếu nhân vật "chết"
        if (!skillUsed && character.hp <= 0)
        {
            character.BlockDeath();
            Activate();
        }
        //Kiểm tra nếu nhân vật hết thời gian bất tử
        if (isCharacterInvincible && Time.time >= character.GetInvincibleEndTime())
        {
            isCharacterInvincible = false;
            character.DeactiveInvincible();
            CharacterInvincibleEndTime = 0f;
            character.AllowDeath();
        }

        // Đếm ngược thời gian buff
        if (buffTimer > 0 && character.hp <= 0 && skillUsed)
        {
            buffTimer = 0f;
            weapon.damage = originDamage;
        }

        if (buffTimer > 0)
        {
            buffTimer -= Time.deltaTime;
            if (buffTimer <= 0f)
            {
                // Hết thời gian buff → đưa damage về ban đầu
                weapon.damage = originDamage;
                sharedState.hasUsedSkill4 = false;
            }
        }
    }
    private void createExplosion()
    {
        if (revivePrefabs != null)
        {
            Instantiate(revivePrefabs, transform.position, Quaternion.identity);
        }
    }

}
