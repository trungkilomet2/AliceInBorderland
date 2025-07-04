using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditScript : MonoBehaviour
{
    public float scrollSpeed = 10f;
    // Start is called before the first frame update
    private RectTransform rectTransform;
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        rectTransform.anchoredPosition += new Vector2(0, scrollSpeed * Time.deltaTime);
        if (rectTransform.anchoredPosition.y >= 1500)
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
}
