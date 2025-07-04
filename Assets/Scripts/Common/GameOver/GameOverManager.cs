using TMPro;
using Unity.Burst.Intrinsics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;

public class GameOverManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timeScore;
    public CanvasGroup gameOverGroup;
    private float fadeSpeed = 0.005f;
    public bool isGameOver = false;

    private CommonUI commonUI;
    private AudioManager audioManager;

    private void Awake()
    {
        gameOverGroup.gameObject.SetActive(false);
        audioManager = FindObjectOfType<AudioManager>();
        commonUI = FindObjectOfType<CommonUI>();
    }

    void Update()
    {
        if (isGameOver)
        {
            gameOverGroup.gameObject.SetActive(true);
            if (gameOverGroup.alpha < 1f)
            {
                gameOverGroup.alpha += fadeSpeed;
            }
            else
            {
                gameOverGroup.alpha = 1f;
            }
        }
    }

    

    public void TriggerGameOver()
    {
        Time.timeScale = 0f;
        gameOverGroup.alpha = 0f;
        gameOverGroup.blocksRaycasts = true;
        gameOverGroup.interactable = true;
        isGameOver = true;

        timeScore.text = commonUI.currentTime.ToString("F0") + "s";
        audioManager.PlayGameOverSound(); // Play game over sound
        Time.timeScale = 0f; // Pause the game
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("CharacterSelection");
    }

}
