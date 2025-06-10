using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public float maxDistance = 10f;
    private Vector3 startPos;

    public float damage = 10;

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
