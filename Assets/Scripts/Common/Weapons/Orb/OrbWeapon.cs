using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class OrbWeapon : MonoBehaviour
{
    public float damage = 10f;
    public float pushForce = 999f;
    public float damageInterval = 1f;

    private float timer;
    private float timeToAttack = 1f;

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
        //if (other.CompareTag("Enemy"))
        //{
        //    enemiesInRange.RemoveAll(other.gameObject);
        //}
    }

    private void FixedUpdate()
    {

        for (int i = enemiesInRange.Count - 1; i >= 0; i--)
        {   
            GameObject enemy  = enemiesInRange[i];
         
            //   EnemyData data = enemiesInRange[i];

            if (enemy == null)
            {
                enemiesInRange.RemoveAt(i);
                continue;
            }

            timer += Time.deltaTime;
            if (timer >= damageInterval)
            {
                timer = 0f;
                KnockBackEnemy(enemy);
            }
        }
    }

    private void KnockBackEnemy(GameObject enemy)
    {
        Rigidbody2D rb = enemy.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            Vector2 pushDir = (enemy.transform.position - transform.position).normalized;
            rb.velocity = pushDir * pushForce;  // đẩy ngay lập tức
        }
    }

}





