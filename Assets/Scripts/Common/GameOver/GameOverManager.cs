using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverManager : MonoBehaviour
{
    public CanvasGroup gameOverGroup;
    private float fadeSpeed = 0.005f;
    private bool isGameOver = false;

    void Update()
    {
        if (isGameOver)
        {
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
    }

    public void OnClickQuitGame()
    {
        SceneManager.LoadScene("MainMenu");
    }

}
