using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageText : MonoBehaviour
{
    public float moveUpSpeed = 1f;
    public float duration = 2f;
    public TextMeshProUGUI text;

    private Color originalColor;

    void Start()
    {
        Canvas canvas = GetComponentInChildren<Canvas>();
        if (canvas != null && canvas.renderMode == RenderMode.WorldSpace && canvas.worldCamera == null)
        {
            canvas.worldCamera = Camera.main;
        }

        originalColor = text.color;
        Destroy(gameObject, duration);
    }

    void Update()
    {
        transform.position += Vector3.up * moveUpSpeed * Time.deltaTime;

        // Mờ dần
        Color c = text.color;
        c.a -= Time.deltaTime / duration;
        text.color = c;
    }

    public void SetDamage(float damage)
    {
        text.text = damage.ToString();
        text.color = originalColor;
    }
}
