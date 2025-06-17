using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSkill2 : MonoBehaviour
{
   
        private Transform player;
        private float angle;
        private float radius;
        private float rotationSpeed = 2f; // radians per second

        public void Initialize(Transform player, float startAngle, float radius)
        {
            this.player = player;
            this.angle = startAngle;
            this.radius = radius;
        }

        void Update()
        {
            if (player == null) return;

            angle += rotationSpeed * Time.deltaTime;

            Vector3 offset = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * radius;
            transform.position = player.position + offset;
        }
    }


