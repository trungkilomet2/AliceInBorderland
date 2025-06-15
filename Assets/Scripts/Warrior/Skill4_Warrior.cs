using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill4_Warrior : SkillBase
{
    public float reviveHpPercent = 0.5f;   // Hồi lại 50% máu tối đa
    public float damageMultiplier = 1.5f;  // Tăng sát thương
    public float buffDuration = 45f;       // Hiệu lực buff
    private bool skillUsed = false;        // Đảm bảo chỉ dùng 1 lần khi chết
    private float buffTimer = 0f;
    
    private CharacterCommonBehavior character;

    public override void Awake()
    {
        base.Awake();
        character = GetComponent<CharacterCommonBehavior>();
    }
    protected override void Activate()
    {
        if (character == null || skillUsed) return;

        skillUsed = true;

        // Hồi máu
        float reviveHp = character.hp * reviveHpPercent;
        character.hp = reviveHp;

        // Tăng sát thương


        // Bắt đầu timer buff
        buffTimer = buffDuration;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (character == null) return;

        // Kiểm tra nếu nhân vật "chết"
        if (!skillUsed && character.hp <= 0)
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
            }
        }
    }
}
