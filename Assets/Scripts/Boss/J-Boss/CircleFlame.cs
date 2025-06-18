using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleFlame : MonoBehaviour
{
    public GameObject effectPrefab; // Sprite hiệu ứng
    public int numEffects = 8;      // Số lượng hiệu ứng
    public float radius = 1f;       // Bán kính của vòng tròn
    public float damageCooldown = 1f; // 1 giây
    private Dictionary<Collider2D, float> lastDamageTime = new Dictionary<Collider2D, float>();


    void Start()
    {
        for (int i = 0; i < numEffects; i++)
        {
            float angle = i * Mathf.PI * 2f / numEffects;
            Vector3 localPos = new Vector3(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius, 0);

            // Tạo hiệu ứng làm con của Circle
            GameObject effect = Instantiate(effectPrefab, transform);
            effect.transform.localPosition = localPos;
            effect.transform.localRotation = Quaternion.identity;
        }
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        // Kiểm tra cooldown
        float timeNow = Time.time;
        if (!lastDamageTime.ContainsKey(other))
        {
            lastDamageTime[other] = -Mathf.Infinity;
        }

        if (timeNow - lastDamageTime[other] >= damageCooldown)
        {
            var character = other.GetComponent<CharacterCommonBehavior>();
            if (character != null)
            {
                character.TakeDamage(1);
                lastDamageTime[other] = timeNow;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // Xoá khỏi danh sách khi đối tượng rời vùng va chạm
        if (lastDamageTime.ContainsKey(other))
        {
            lastDamageTime.Remove(other);
        }
    }
}
