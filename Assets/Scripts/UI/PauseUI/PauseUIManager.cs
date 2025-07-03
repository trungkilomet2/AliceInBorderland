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


    private GameOverManager gameOverManager;
    private CommonUI commonUI;
    private AudioManager audioManager;
    private bool canPause = true;   

    private void Awake()
    {
        pauseMenuUI.SetActive(false);
        gameOverManager = FindObjectOfType<GameOverManager>();
        audioManager = FindObjectOfType<AudioManager>();
        commonUI = FindObjectOfType<CommonUI>();
    }

    private void Update()
    {
        if (canPause)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (pauseMenuUI.activeInHierarchy)
                {

                    pauseMenuUI.SetActive(false);
                    Time.timeScale = 1f;
                }
                else
                {
                    pauseMenuUI.SetActive(true);
                    Time.timeScale = 0f; // Pause the game
                }
            }
        }
    }

    public void SetCanPause(bool changePause)
    {
        this.canPause = changePause;
    }

    private void OnApplicationPause(bool pause)
    {
        if (gameOverManager != null && !gameOverManager.isGameOver)
        {
            pauseMenuUI.SetActive(pause);
        }
    }

    public void ResumeGame()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f; // Resume the game
    }
    public void MainMenu()
    {
        SceneManager.LoadSceneAsync(0);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void RestartGame()
    {
        SceneManager.LoadSceneAsync(1);
    }


    public void SoundVolume()
    {
        audioManager.ChangeSoundVolume(0.2f);
    }

    public void EffectVolume()
    {
        audioManager.ChangeEffectVolume(0.2f);
    }
}
