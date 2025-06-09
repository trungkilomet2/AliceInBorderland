using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warrior : CharacterCommonBehavior
{
    public override float moveSpeed { get; set; } = 4f;
    public GameObject axePrefabs;
    public Transform throwPoint;
    public float throwForce = 10f;
    // Start is called before the first frame update

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        if (Input.GetMouseButtonDown(0))
        {
            Attack();
        }
    }
        public override void Attack()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mousePosition - throwPoint.position).normalized;

        GameObject axe = Instantiate(axePrefabs, throwPoint.position, Quaternion.identity);
        Rigidbody2D rb = axe.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = direction * throwForce;
        }

        // Xoay rìu theo hướng bay (tùy hiệu ứng)
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        axe.transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}
