using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloneWarriorController : MonoBehaviour
{
    public float dashSpeed = 10f;
    public float dashTime = 0.5f;
    public float explosionDamage = 30f;
    public GameObject explosionEffect;

    private Vector2 dashDirection;
    private float timer = 0f;
    private Warrior originalWarrior;
    private SkillBase skillBase;
    private bool isDashing = false;
    private CharacterCommonBehavior characterCommonBehavior;

    public void StartDash(Vector2 dir, Warrior origin)
    {
        dashDirection = dir.normalized;
        originalWarrior = origin;
    }
    void Start()
    {
        skillBase = GetComponent<SkillBase>();
        characterCommonBehavior = GetComponent<CharacterCommonBehavior>();
    }

    void Update()
    {
        transform.Translate(dashDirection * dashSpeed * Time.deltaTime);

        timer += Time.deltaTime;
        if (timer >= dashTime)
        {
            ExplodeAndMerge();
        }
    }

    void ExplodeAndMerge()
    {
        if (explosionEffect != null)
            Instantiate(explosionEffect, transform.position, Quaternion.identity);

        if (originalWarrior != null)
            originalWarrior.transform.position = transform.position;

        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Enemy"))
        {
            // Gây damage ở đây nếu cần
            col.GetComponent<Enemy>()?.TakeDamage(explosionDamage * 0.5f);
        }
    }

    private void OnEnable()
    {
        CharacterCommonBehavior.OnBlockedCollision += StopRolling;
    }

    private void OnDisable()
    {
        CharacterCommonBehavior.OnBlockedCollision -= StopRolling;
    }

    private void StopRolling()
    {
        if (isDashing)
        {
            Debug.Log("Va chạm Block, dừng dash.");
            isDashing = false;
            skillBase.CancelSkill();
            // THAY ĐỔI: Tắt isDashing khi dash bị dừng do va chạm
            if (characterCommonBehavior != null)
            {
                characterCommonBehavior.isDashing = false;
            }
        }
    }
}

