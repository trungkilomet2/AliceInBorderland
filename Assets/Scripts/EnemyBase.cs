using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBase : MonoBehaviour
{
    protected GameObject targetGameObject;

    [SerializeField] float hp = 50f;
    [SerializeField] float damage = 10f;

    private GameObject damageTextPrefab;

    public virtual void Awake()
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

    public void SetTarget(GameObject target)
    {
        targetGameObject = target;
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
        else if (other.CompareTag("Player"))
        {
            CharacterCommonBehavior targetCharacter = other.GetComponent<CharacterCommonBehavior>();
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
