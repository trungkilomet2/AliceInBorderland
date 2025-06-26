using UnityEngine;

public class ExplosionOrb : MonoBehaviour
{
    public float explosionRadius = 3f; // Bán kính vụ nổ
    public GameObject explosionEffectPrefab; // Hiệu ứng vụ nổ (optional)

    private float _damage;

    public void SetDamage(float damage)
    {
        _damage = damage;
    }

    // Hàm này sẽ được gọi ngay sau khi quả cầu được tạo ra
    public void ExplodeImmediately()
    {
        Explode(); // Gọi hàm nổ ngay lập tức
    }

    // Bỏ qua Update vì không cần di chuyển nữa

    private void Explode()
    {
        // Tạo hiệu ứng vụ nổ nếu có
        if (explosionEffectPrefab != null)
        {
            Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);
        }

        // Tìm tất cả các Collider trong bán kính vụ nổ
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius);

        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Enemy"))
            {
                // hitCollider.GetComponent<HealthComponent>()?.TakeDamage(_damage);
                Debug.Log($"Enemy {hitCollider.name} took {_damage} damage from explosion orb.");
            }
        }

        // Phá hủy quả cầu sau khi nổ
        Destroy(gameObject);
    }

    // Vẽ bán kính nổ trong editor để dễ debug
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}