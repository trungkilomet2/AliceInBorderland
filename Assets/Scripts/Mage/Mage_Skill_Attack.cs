using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mage_Skill_Attack : MonoBehaviour
{
    public float maxDistance = 20f;
    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        if (Vector3.Distance(transform.position, startPos) > maxDistance)
        {
            Destroy(gameObject);
        }
    }
}
