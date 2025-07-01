using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private MainMenuAudioManager audioManager;

    private void Awake()
    {
        audioManager = FindAnyObjectByType<MainMenuAudioManager>();
    }
    
    public void PlayGame()
    {
        audioManager?.PlayClickOptionSound();
        StartCoroutine(LoadSceneAfterDelay(1f));
    }

    private IEnumerator LoadSceneAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadSceneAsync(1);
    }

    public void QuitGame()
    {
        audioManager?.PlayClickOptionSound();
        Application.Quit();
    }
}