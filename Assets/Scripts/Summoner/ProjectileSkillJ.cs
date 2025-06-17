using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSkillJ : MonoBehaviour
{
    private Vector3 startPos;
    public float maxDistance = 10f;

    public void SetDestroyAfterDistance(Vector3 origin, float maxDist)
    {
        startPos = origin;
        maxDistance = maxDist;
    }

    void Update()
    {
        if (Vector3.Distance(transform.position, startPos) > maxDistance)
        {
            Destroy(gameObject);
        }
    }
}


