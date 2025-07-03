using UnityEngine;

public class BalloonFollow : MonoBehaviour
{
    Transform player;
    Vector3 offset;
    float angleOffset;
    public float rotationSpeed = 90f; // độ/giây
    public float orbitLifetime = 3f;  // thời gian quay quanh player
    public float moveToOrbitSpeed = 8f; // tốc độ bay đến quỹ đạo
    public GameObject bunnyPrefab;

    private bool inOrbit = false;
    private float timeInOrbit = 0f;

    public void Init(Transform target, Vector3 offset, float angle)
    {
        this.player = target;
        this.offset = offset;
        this.angleOffset = angle;
        inOrbit = false;
        timeInOrbit = 0f;
    }

    void Update()
    {
        if (player == null) return;

        Vector3 orbitTarget = player.position + offset;

        if (!inOrbit)
        {
            // Bay từ boss đến đúng vị trí quỹ đạo quanh player
            transform.position = Vector3.MoveTowards(transform.position, orbitTarget, moveToOrbitSpeed * Time.deltaTime);

            // Khi tới gần đúng vị trí trên quỹ đạo, bắt đầu quay
            if (Vector3.Distance(transform.position, orbitTarget) < 0.05f)
            {
                inOrbit = true;
                transform.position = orbitTarget;
            }
        }
        else
        {
            // Quay quanh player
            angleOffset += rotationSpeed * Mathf.Deg2Rad * Time.deltaTime;
            Vector3 newOffset = new Vector3(Mathf.Cos(angleOffset), Mathf.Sin(angleOffset), 0) * offset.magnitude;
            transform.position = player.position + newOffset;

            // Đếm thời gian sống
            timeInOrbit += Time.deltaTime;
            if (timeInOrbit >= orbitLifetime)
            {
                Explode();
            }
        }
    }

    public void Explode()
    {
        // Hướng ra xa player làm hướng cơ bản cho bunny
        Vector3 fromPlayer = (transform.position - player.position).normalized;
        float baseAngle = Mathf.Atan2(fromPlayer.y, fromPlayer.x);
        int bunnyCount = 3;
        float spread = Mathf.PI / 3; // 60 độ

        for (int i = 0; i < bunnyCount; i++)
        {
            float angle = baseAngle + spread * (i - (bunnyCount - 1) / 2f);
            Vector3 dir = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0);
            GameObject bunny = Instantiate(bunnyPrefab, transform.position, Quaternion.identity);
            bunny.GetComponent<BunnyProjectile>().Init(dir);
        }
        Destroy(gameObject);
    }
}
