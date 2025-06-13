using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : EnemyBase
{
    [SerializeField] float speed;

    Rigidbody2D rgb2d;

    public GameObject coin;
    public GameObject exp;
    private const float MAX_RATTING_DROPCOIN = 10f;
    private const float MAX_RATTING_DROPEXP = 50f;



    private void Awake()
    {
        base.Awake();
        rgb2d = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        Vector3 direction = (targetGameObject.transform.position - transform.position).normalized;
        rgb2d.velocity = direction * speed;
    }

    public override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
    }
}
