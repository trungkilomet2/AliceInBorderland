using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponBase : MonoBehaviour
{
    public float maxDistance = 10f;
    public float damage = 10;
    public bool isThought = false; // Indicates if the fireball is thought to be a weapon

    private Vector3 startPos;

    protected virtual void Start()
    {
        startPos = transform.position;
    }

    protected virtual void Update()
    {
        if (Vector3.Distance(transform.position, startPos) > maxDistance)
        {
            Destroy(gameObject);
        }
    }
}
