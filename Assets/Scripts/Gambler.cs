using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gambler : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Vector3 moveInput;

    private Rigidbody2D rb;



    private void Update()
    {
        moveInput.x = Input.GetAxis("Horizontal");
        moveInput.y = Input.GetAxis("Vertical");
        transform.position += moveInput * moveSpeed * Time.deltaTime;

        if (moveInput.x != 0)
        {
            if (moveInput.x != 0)
            {
                Vector3 scale = transform.localScale;
                scale.x = moveInput.x > 0 ? Mathf.Abs(scale.x) : -Mathf.Abs(scale.x);
                transform.localScale = scale;
            }
        }
    }
}
