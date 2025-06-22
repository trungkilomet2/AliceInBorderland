using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class J_Skill1 : BossSkillBase
{
    public GameObject projectile;
    public float bulletForce;

    protected override void Activate()
    {
        Vector2 direction1 = ((Vector2)target.transform.position - (Vector2)transform.position).normalized;
        Vector2 direction2 = Quaternion.Euler(0, 0, 10) * direction1;
        Vector2 direction3 = Quaternion.Euler(0, 0, -10) * direction1;
        Vector2 direction4 = Quaternion.Euler(0, 0, 20) * direction1;
        Vector2 direction5 = Quaternion.Euler(0, 0, -20) * direction1;

        for (int i = 0; i < 5; i++)
        {
            Vector2 direction;
            switch (i)
            {
                case 0: direction = direction1; break;
                case 1: direction = direction2; break;
                case 2: direction = direction3; break;
                case 3: direction = direction4; break;
                case 4: direction = direction5; break;
                default: direction = direction1; break;
            }
            GameObject newArrow = Instantiate(projectile, transform.position, Quaternion.identity);
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            newArrow.transform.rotation = Quaternion.Euler(0f, 0f, angle);
            Rigidbody2D arrowRb = newArrow.GetComponent<Rigidbody2D>();
            arrowRb.AddForce(direction * bulletForce, ForceMode2D.Impulse);
        }
    }
}
