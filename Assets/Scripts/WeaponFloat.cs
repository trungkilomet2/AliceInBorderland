using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponFloat : MonoBehaviour
{
    public float floatHeight = 0.3f;
    public float floatSpeed = 2f;
    private Vector3 startPos;

    void Start() => startPos = transform.position;

    void Update()
    {
        float offset = Mathf.Sin(Time.time * floatSpeed) * floatHeight;
        transform.position = startPos + new Vector3(0, offset, 0);
    }
}

