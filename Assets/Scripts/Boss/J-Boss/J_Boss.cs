using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class J_Boss : BossBase
{
    public GameObject target;
    public GameObject projectile;
    public float bulletForce = 2f;
    public float timeBtwAttack = 2f;
    private float _timeBtwAttack = 2f;

    public float timeBtwAttack2 = 2f;
    private float _timeBtwAttack2 = 2f;

    public GameObject cirleFlame;

    private CharacterCommonBehavior targetCharacter;

    protected override void Awake()
    {
        base.Awake();
        targetCharacter = target.GetComponent<CharacterCommonBehavior>();
    }
    
    void Update()
    {
        //BossSkillBase skill1 = bossSkillBases[0];
        //if (Vector3.Distance(transform.position, target.transform.position) < skill1.skillRange)
        //{
        //    bossSkillBases[0].UseSkill();
        //}

        BossSkillBase skill2 = bossSkillBases[1];
        if (Vector3.Distance(transform.position, target.transform.position) < skill2.skillRange)
        {
            bossSkillBases[1].UseSkill();
        }
    }

    private void FixedUpdate()
    {
        //Vector3 direction = (targetCharacter.transform.position - transform.position).normalized;
        //rgb2d.velocity = direction * speed;
    }

}
