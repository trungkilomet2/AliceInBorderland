using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms;

public class PauseUIManager : MonoBehaviour
{
    // Pause Menu
    [SerializeField] private GameObject pauseMenuUI;

    //Game Over Menu
    [SerializeField] private GameObject gameOverMenuUI;
    [SerializeField] private TextMeshProUGUI timeScore;


    private CommonUI commonUI;
    private AudioManager audioManager;

    private void Awake()
    {
        pauseMenuUI.SetActive(false);
        gameOverMenuUI.SetActive(false);
        audioManager = FindObjectOfType<AudioManager>();
        commonUI = FindObjectOfType<CommonUI>();
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

    public void QuitGame()
    {
        Application.Quit();
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void GameOver()
    {
        timeScore = commonUI.timerCounter; // Get the time score from CommonUI
        gameOverMenuUI.SetActive(true);
        Time.timeScale = 0f; // Pause the game
        audioManager.PlayGameOverSound(); // Play game over sound
    }
}
