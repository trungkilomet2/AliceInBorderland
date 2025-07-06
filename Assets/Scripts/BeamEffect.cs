using UnityEngine;

public class BeamEffect : MonoBehaviour
{
    public Transform target;
    public float speed = 5f;

    void Update()
    {
        if (target != null)
        {
            Vector3 dir = (target.position - transform.position).normalized;
            transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
            
        }
    }
}
