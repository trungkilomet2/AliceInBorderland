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
    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        collider2D = GetComponent<Collider2D>();
        characterCommon = GetComponentInParent<CharacterCommonBehavior>();
    }

    void Update()
    {
        // Nếu item đang bị vô hiệu hóa và thời gian đã hết thì bật lại
        if (!isItemActive && Time.time >= itemDisableEndTime)
        {
            spriteRenderer.enabled = true;
            collider2D.enabled = true;
            isItemActive = true;
            characterCommon.DeactiveNeckleItem();
        }
        Debug.Log("Time : " + Time.time);
        Debug.Log("TimeDisable : " + itemDisableEndTime);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isItemActive) return;

        if (collision.CompareTag("Enemy"))
        {
            if (characterCommon != null)
            {
                characterCommon.ActiveNeckleItem(0.5f);
            }

            isItemActive = false;
            itemDisableEndTime = Time.time + 3f;

            spriteRenderer.enabled = false;
            collider2D.enabled = false;
        }
    }
}
