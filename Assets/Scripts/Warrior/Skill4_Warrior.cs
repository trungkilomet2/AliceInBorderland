using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class Skill4_Warrior : SkillBase
{
    public static float reviveHpPercent = 0.5f;   // Hồi lại 50% máu tối đa
    public float damageMultiplier = 1.5f;  // Tăng sát thương
    public float buffDuration = 45f;       // Hiệu lực buff
    private bool skillUsed = false;        // Đảm bảo chỉ dùng 1 lần khi chết
    private float buffTimer = 0f;
    private CharacterCommonBehavior character;
    private WeaponBase weapon;
    private float reviveHp = 0f;
    public GameObject gameObject;
    public GameObject revivePrefabs;
    private Animator animator;

    public override void Awake()
    {
        base.Awake();
        character = GetComponent<CharacterCommonBehavior>();
        weapon = GetComponent<WeaponBase>();
    }
    protected override void Activate()
    {
        if (character == null || skillUsed) return;

        skillUsed = true;

        // Hồi máu
        character.hp = reviveHp;

        // Tăng sát thương
        weapon = gameObject.GetComponent<WeaponBase>();

        weapon.damage = weapon.damage * damageMultiplier;
        // Bắt đầu timer buff
        buffTimer = buffDuration;
        animator.SetTrigger("isRevive");
        createExplosion();
    }

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        reviveHp = character.hp * 0.5f;
    }

    // Update is called once per frame
    void Update()
    {
        if (character == null) return;

        // Kiểm tra nếu nhân vật "chết"
        if (!skillUsed && character.hp <= 10)
        {
            Activate();
        }

        // Đếm ngược thời gian buff
        if (buffTimer > 0)
        {
            buffTimer -= Time.deltaTime;
            if (buffTimer <= 0f)
            {
                // Hết thời gian buff → đưa damage về ban đầu
                weapon.damage = weapon.damage / damageMultiplier;
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
