using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostImage : MonoBehaviour
{
    public GameObject ghostPrefab;
    public float ghostInterval = 0.05f;
    public int ghostSortingOrderOffset = -1;

    public IEnumerator SpawnGhosts(float duration)
    {
        float timer = 0f;

        SpriteRenderer playerSr = GetComponent<SpriteRenderer>(); // lấy đúng sprite hiện tại từ Animator
        while (timer < duration)
        {
            GameObject ghost = Instantiate(ghostPrefab, transform.position, transform.rotation);
            SpriteRenderer ghostSr = ghost.GetComponent<SpriteRenderer>();

            if (ghostSr != null && playerSr != null)
            {
                ghostSr.sprite = playerSr.sprite; // đúng sprite đang hiển thị
                ghostSr.flipX = playerSr.flipX;
                ghostSr.sortingLayerID = playerSr.sortingLayerID;
                ghostSr.sortingOrder = playerSr.sortingOrder + ghostSortingOrderOffset;
            }

            ghost.transform.localScale = transform.localScale;

            Destroy(ghost, 0.3f);
            yield return new WaitForSeconds(ghostInterval);
            timer += ghostInterval;
        }
    }

}
