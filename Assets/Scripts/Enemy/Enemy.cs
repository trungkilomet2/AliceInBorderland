using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : EnemyBase
{
    [SerializeField] float speed;

    Rigidbody2D rgb2d;

    public override void Awake()
    {
        base.Awake();
        rgb2d = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        Vector3 direction = (targetGameObject.transform.position - transform.position).normalized;
        rgb2d.velocity = direction * speed;
    }

    public override void OnTriggerEnter2D(Collider2D collision) // Added 'override' keyword to fix CS0114
    {
        base.OnTriggerEnter2D(collision);
    }
}
