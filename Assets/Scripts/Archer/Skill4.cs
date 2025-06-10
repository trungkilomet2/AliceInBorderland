using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill4 : SkillBase
{
    public GameObject arrow;
    public float bulletForce = 5f;
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
        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorld.z = 0;
        Vector3 direction = (mouseWorld - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        GameObject arrowInstance = Instantiate(arrow, transform.position, Quaternion.identity);
        arrowInstance.transform.localScale = new Vector3(skillWidth * 2, skillWidth * 2, 1f);
        arrowInstance.transform.rotation = Quaternion.Euler(0f, 0f, angle);
        Rigidbody2D rb = arrowInstance.GetComponent<Rigidbody2D>();
        rb.AddForce(direction * bulletForce, ForceMode2D.Impulse);
    }

}
