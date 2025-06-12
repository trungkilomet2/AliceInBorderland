using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    CharacterCommonBehavior targetCharacter;
    [SerializeField] float hp = 50f;

    private GameObject damageTextPrefab;

    private void Awake()
    {
        damageTextPrefab = Resources.Load<GameObject>("Prefabs/DamageText");
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

    public virtual void OnTriggerEnter2D(Collider2D other)
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
    }

    private void ShowDamageText(float damage)
    {
        Vector3 spawnPos = transform.position + new Vector3(0, 1f, 0); // bay lên đầu enemy

        GameObject dmgTextObj = Instantiate(damageTextPrefab, spawnPos, Quaternion.identity);

        DamageText dmgText = dmgTextObj.GetComponent<DamageText>();
        dmgText.SetDamage(damage);
    }
}
