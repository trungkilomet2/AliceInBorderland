using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SumSkill2 : MonoBehaviour
{
  
        public GameObject ProjectileSkill2;
        public int projectileCount = 7;
        public float radius = 4f;
        public float duration = 10f;
        public float cooldown = 15f;

        private bool skillActive = false;
        private float cooldownTimer = 0f;

        private List<GameObject> activeProjectiles = new List<GameObject>();

        void Update()
        {
            // Cooldown logic
            if (cooldownTimer > 0)
                cooldownTimer -= Time.deltaTime;

            // Skill activation
            if (!skillActive && cooldownTimer <= 0 && IsSkill2Pressed())
            {
                ActivateSkill();
            }
        }

        bool IsSkill2Pressed()
        {
            return Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2);
        }

        void ActivateSkill()
        {
            skillActive = true;
            cooldownTimer = cooldown;

            for (int i = 0; i < projectileCount; i++)
            {
                float angle = i * Mathf.PI * 2 / projectileCount;
                Vector3 offset = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * radius;
                
                GameObject proj = Instantiate(ProjectileSkill2, transform.position + offset, Quaternion.identity);
            Collider2D projCol = proj.GetComponent<Collider2D>();
            Collider2D playerCol = GetComponent<Collider2D>();
            if (projCol != null && playerCol != null)
            {
                Physics2D.IgnoreCollision(projCol, playerCol);
            }
            proj.GetComponent<ProjectileSkill2>().Initialize(this.transform, angle, radius);
                activeProjectiles.Add(proj);
            }

            StartCoroutine(EndSkillAfterDuration());
        }

        IEnumerator EndSkillAfterDuration()
        {
            yield return new WaitForSeconds(duration);

            foreach (var proj in activeProjectiles)
            {
                if (proj != null) Destroy(proj);
            }

            activeProjectiles.Clear();
            skillActive = false;
        }
    }


