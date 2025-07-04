using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DonutCollider2D : MonoBehaviour
{
    [Header("Donut Settings")]
    public float outerRadius = 1.0f;
    public float innerRadius = 0.8f;
    public int segmentCount = 64;

    private void Awake()
    {
        PolygonCollider2D poly = GetComponent<PolygonCollider2D>();
        poly.pathCount = 2;

        // Outer circle (clockwise)
        Vector2[] outerPoints = new Vector2[segmentCount];
        for (int i = 0; i < segmentCount; i++)
        {
            float angle = 2 * Mathf.PI * i / segmentCount;
            outerPoints[i] = new Vector2(Mathf.Cos(angle) * outerRadius, Mathf.Sin(angle) * outerRadius);
        }
        poly.SetPath(0, outerPoints);

        // Inner circle (counter-clockwise for hole)
        Vector2[] innerPoints = new Vector2[segmentCount];
        for (int i = 0; i < segmentCount; i++)
        {
            float angle = 2 * Mathf.PI * i / segmentCount;
            innerPoints[i] = new Vector2(Mathf.Cos(angle) * innerRadius, Mathf.Sin(angle) * innerRadius);
        }
        System.Array.Reverse(innerPoints); // counter-clockwise to make a hole
        poly.SetPath(1, innerPoints);

        poly.isTrigger = true;
    }
}
