using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    Transform targetDestination;
    GameObject targetGameObject;
    CharacterCommonBehavior targetCharacter;
    [SerializeField] float speed;
    private GameObject damageTextPrefab;

    Rigidbody2D rgb2d;

    [SerializeField] float hp = 50f;
    [SerializeField] float damage = 10f;

    private void Awake()
    {
        damageTextPrefab = Resources.Load<GameObject>("Prefabs/DamageText"); // Load the damage text prefab from Resources folder
        rgb2d = GetComponent<Rigidbody2D>();
    }

    public void SetTarget(GameObject target)
    {
        targetGameObject = target;
        if (targetGameObject != null)
        {
            targetDestination = targetGameObject.transform;
        }
    }

    private void FixedUpdate()
    {
        Vector3 direction = (targetDestination.position - transform.position).normalized;
        rgb2d.velocity = direction * speed;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject == targetGameObject)
        {
            Attack();
        }
    }

    private void Attack()
    {
        Debug.Log("Enemy attacks!");
    }

    public void TakeDamage(float damage)
    {
        ShowDamageText(damage);
        hp -= damage;
        if (hp <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Weapon"))
        {
            WeaponBase weapon = other.GetComponent<WeaponBase>();
            if (weapon != null)
            {
                TakeDamage(weapon.damage);
                if (!weapon.isThought)
                {
                    Destroy(other.gameObject);
                }
            }
        }
        else if (other.CompareTag("Player"))
        {
            targetCharacter = other.GetComponent<CharacterCommonBehavior>();
            if (targetCharacter != null)
            {
                targetCharacter.TakeDamage(damage);
            }
        }
    }

    private void ShowDamageText(float damage)
    {
        Vector3 spawnPos = transform.position + new Vector3(0, 1f, 0); // bay lên đầu enemy

        GameObject dmgTextObj = Instantiate(damageTextPrefab, spawnPos, Quaternion.identity);

        DamageText dmgText = dmgTextObj.GetComponent<DamageText>();
        dmgText.SetDamage(damage);
    }
}
