using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseUIManager : MonoBehaviour
{
    // Pause Menu
    [SerializeField] private GameObject pauseMenuUI;

    private void Awake()
    {
        pauseMenuUI.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (pauseMenuUI.activeInHierarchy)
            {
                pauseMenuUI.SetActive(false);
                Time.timeScale = 1f; // Resume the game
            }
            else
            {
                pauseMenuUI.SetActive(true);
                Time.timeScale = 0f; // Pause the game
            }
        }
    }

    private void OnApplicationPause(bool pause)
    {
        pauseMenuUI.SetActive(pause);
    }

    public void ResumeGame()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f; // Resume the game
    }
    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
