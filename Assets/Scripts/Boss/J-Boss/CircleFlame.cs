using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleFlame : MonoBehaviour
{
    public GameObject effectPrefab; // Sprite hiệu ứng
    public int numEffects = 8;      // Số lượng hiệu ứng
    public float radius = 1f;       // Bán kính của vòng tròn

    void Start()
    {
        for (int i = 0; i < numEffects; i++)
        {
            float angle = i * Mathf.PI * 2f / numEffects;
            Vector3 localPos = new Vector3(Mathf.Cos(angle) * radius / 2, Mathf.Sin(angle) * radius / 2, 0);

            // Tạo hiệu ứng làm con của Circle
            GameObject effect = Instantiate(effectPrefab, transform);
            effect.transform.localPosition = localPos;
            effect.transform.localRotation = Quaternion.identity;
        }
    }
}
