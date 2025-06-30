using System.Collections.Generic;
using UnityEngine;

public class FireZone : MonoBehaviour
{
    public int damage = 1;             // Lượng dame
    public float damageCooldown = 1f;   // Giãn cách gây dame
    private Dictionary<Collider2D, float> lastDamageTime = new Dictionary<Collider2D, float>();

    private void OnTriggerStay2D(Collider2D other)
    {
        Debug.Log("Đã va chạm với: " + other.name + " - tag: " + other.tag);
        if (!other.CompareTag("Player")) return;
        Debug.Log("Va chạm đúng Player rồi!!!");

        float timeNow = Time.time;
        if (!lastDamageTime.ContainsKey(other))
            lastDamageTime[other] = -Mathf.Infinity;

        if (timeNow - lastDamageTime[other] >= damageCooldown)
        {
            var character = other.GetComponent<CharacterCommonBehavior>();
            if (character != null)
            {
                character.TakeDamage(damage);
                lastDamageTime[other] = timeNow;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (lastDamageTime.ContainsKey(other))
            lastDamageTime.Remove(other);
    }
}
