using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ThunderBolt : MonoBehaviour
{
    public float damage = 10f;

    private bool hasDealtDamage = false;

    private void Update()
    {
        if (hasDealtDamage) return;

        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, 1f);
        foreach (Collider2D hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Player"))
            {
                CharacterCommonBehavior character = hitCollider.GetComponent<CharacterCommonBehavior>();
                if (character != null)
                {
                    character.TakeDamage(damage);
                    hasDealtDamage = true;
                    break;
                }
            }
        }
    }
}
