using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill2 : SkillBase
{
    public GameObject skillEffect;

    private GameObject skillEffectInstant;

    public override void Awake()
    {
        base.Awake();
    }

    public override void Update()
    {
        base.Update();
    }


    protected override void Activate()
    {
        Vector3 damagePosition = skillTransform.position;

        skillEffectInstant = Instantiate(skillEffect, damagePosition, Quaternion.identity);
        skillEffectInstant.transform.localScale = new Vector3(skillRange, skillRange, 1f);

        StartCoroutine(DealContinuousDamage(damagePosition));
        Destroy(skillEffectInstant, skillDuration);
    }

    private IEnumerator DealContinuousDamage(Vector3 position)
    {
        float elapsed = 0f;
        float damageInterval = 1f;

        while (elapsed < skillDuration)
        {
            Collider2D[] hitColliders = Physics2D.OverlapCircleAll(position, skillRange / 2);
            foreach (Collider2D hitCollider in hitColliders)
            {
                if (hitCollider.CompareTag("Enemy"))
                {
                    Enemy enemy = hitCollider.GetComponent<Enemy>();
                    if (enemy != null)
                    {
                        enemy.TakeDamage(skillDamage);
                    }
                }
            }
            yield return new WaitForSeconds(damageInterval);
            elapsed += damageInterval;
        }
    }


}
