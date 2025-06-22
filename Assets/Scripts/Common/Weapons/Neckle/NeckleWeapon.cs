using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeckleWeapon : MonoBehaviour
{
    private bool isItemActive = true;
    private float itemDisableEndTime = 0f;
    private SpriteRenderer spriteRenderer;
    private Collider2D collider2D;
    CharacterCommonBehavior characterCommon;
    private float neckleWeaponColdown = 15f;
    private float neckleActiveTiming = 0.8f;
    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        collider2D = GetComponent<Collider2D>();
        characterCommon = GetComponentInParent<CharacterCommonBehavior>();
    }

    void Update()
    {
        if (!isItemActive && Time.time >= characterCommon.GetInvincibleEndTime())
        {
            characterCommon.DeactiveNeckleItem();
        }
        if (!isItemActive && Time.time >= itemDisableEndTime)
        {
            spriteRenderer.enabled = true;
            collider2D.enabled = true;
            isItemActive = true;
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isItemActive) return;

        if (collision.CompareTag("Enemy"))
        {
            if (characterCommon != null)
            {
                characterCommon.ActiveNeckleItem(neckleActiveTiming);
            }
            isItemActive = false;
            itemDisableEndTime = Time.time + neckleWeaponColdown;
            spriteRenderer.enabled = false;
            collider2D.enabled = false;
        }
    }
}
