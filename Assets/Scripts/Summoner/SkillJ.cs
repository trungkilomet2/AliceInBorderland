using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillJ : MonoBehaviour
{ 
    [Header("Dragon Skill Settings")]
    public GameObject DraSkillJ;
    public GameObject SkillJProjectile;
    private float dragonTimer = 0f;
    private GameObject currentDragon;
    private float dragonDuration = 5f;

    [Header("Skill Timing")]
    public float cooldownTime = 10f;
    private float cooldownTimer = 0f;

    public float bulletForce = 10f;
    public float delayBetweenWaves = 2f;

    [Header("Wave Settings")]
    public int[] waveCounts = { 5, 10, 15 };
    public float[] waveDistances = { 5f, 10f, 15f };

    private bool isCasting = false;

    void Update()
    {
        if (cooldownTimer > 0f)
            cooldownTimer -= Time.deltaTime;
        if (currentDragon != null)
        {
            
            currentDragon.transform.position = transform.position + Vector3.up * 2f;

           
            dragonTimer -= Time.deltaTime;
            if (dragonTimer <= 0f)
            {
                Destroy(currentDragon);
                currentDragon = null;
            }
        }

        if (Input.GetKeyDown(KeyCode.J) && cooldownTimer <= 0 && !isCasting)
        {
            StartCoroutine(CastDragonSkill());
        }
    }

    private IEnumerator CastDragonSkill()
    {
        isCasting = true;
       
        
        cooldownTimer = cooldownTime;
        dragonTimer = dragonDuration;

       
        currentDragon = Instantiate(DraSkillJ, transform.position + Vector3.up * 2f, Quaternion.identity);

      
        Animator anim = currentDragon.GetComponent<Animator>();
        if (anim != null)
        {
            anim.SetTrigger("Appear");
        }

        // Fire 3 waves
        for (int i = 0; i < waveCounts.Length; i++)
        {
            FireInCircle(waveCounts[i], waveDistances[i]);
            yield return new WaitForSeconds(delayBetweenWaves);
        }

        //Destroy(dragon, 3f); // Optional: remove dragon after few seconds
        isCasting = false;
    }

    private void FireInCircle(int count, float maxDistance)
    {
        float angleStep = 360f / count;
        Vector3 origin = transform.position;

        for (int i = 0; i < count; i++)
        {
            float angle = i * angleStep * Mathf.Deg2Rad;
            Vector2 direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));

            // Spawn projectile hơi xa player một chút để tránh collision
            Vector3 spawnPosition = transform.position + (Vector3)(direction * 1f);

            GameObject fireball = Instantiate(SkillJProjectile, spawnPosition, Quaternion.identity);
            Rigidbody2D rb = fireball.GetComponent<Rigidbody2D>();

            // Ignore collision giữa projectile và player
            Collider2D projCol = fireball.GetComponent<Collider2D>();
            Collider2D playerCol = GetComponent<Collider2D>();
            if (projCol != null && playerCol != null)
            {
                Physics2D.IgnoreCollision(projCol, playerCol);
            }

            if (rb != null)
            {
                rb.AddForce(direction * bulletForce, ForceMode2D.Impulse);
            }

            ProjectileSkillJ fb = fireball.GetComponent<ProjectileSkillJ>();
            if (fb != null)
            {
                fb.SetDestroyAfterDistance(origin, maxDistance);
            }
        }
    }
}



