using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class OrbWeapon : MonoBehaviour
{
    public float damage = 10f;
    public float pushForce = 20f;
    public float damageInterval = 1f;

    private float timer;

    private List<GameObject> enemiesInRange = new List<GameObject>();

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            enemiesInRange.Add(other.GameObject());
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            enemiesInRange.Remove(other.gameObject);
        }
    }

    private void Update()
    {
        for (int i = enemiesInRange.Count - 1; i >= 0; i--)
        {   
            GameObject enemy  = enemiesInRange[i];
            if (enemy == null)
            {
                enemiesInRange.RemoveAt(i);
                continue;
            }
            timer += Time.deltaTime;
            if (timer >= damageInterval)
            {
                timer = 0f;
                TakeDame(enemy);
                KnockBack(enemy);
            }
        }
    }

    private void TakeDame(GameObject enemy) 
    {
        Enemy enemyScript = enemy.GetComponent<Enemy>();
        enemyScript?.TakeDamage(damage);

    }
    private void KnockBack(GameObject enemy)
    {
        Enemy enemyScript = enemy.GetComponent<Enemy>();
        if (enemyScript != null)
        {
            Vector2 pushDir = (enemy.transform.position - transform.position).normalized;
            enemyScript.KnockbackEnemy(pushDir * pushForce);
        }
    }

}





