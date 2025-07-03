using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public float changeTime; 
    public string sceneName;

    private float timer = 0f;
    private bool isLoading = false;

    void Update()
    {
        if (isLoading) return;

        timer += Time.deltaTime;

        if (timer >= changeTime || Input.anyKeyDown || Input.GetMouseButtonDown(0))
        {
            isLoading = true; 
            SceneManager.LoadScene(sceneName);
        }
    }
}
