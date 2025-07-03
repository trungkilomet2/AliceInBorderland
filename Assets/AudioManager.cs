using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    [SerializeField] private AudioSource backgroundAudioSource;
    [SerializeField] private AudioSource effectAudioSource;

    [SerializeField] private AudioClip backgroundClip;
    [SerializeField] private AudioClip coinClip;

    [SerializeField] private AudioClip clickItemClip;
    [SerializeField] private AudioClip gameoverClip;
    [SerializeField] private AudioClip victoryClip;



    // Start is called before the first frame update
    void Start()
    {
        PlayBackGroundMusic();
    }

    public void PlayBackGroundMusic()
    {
        if (backgroundAudioSource != null)
        {
            backgroundAudioSource.clip = backgroundClip;
            backgroundAudioSource.Play();
        }
    }

    public void PlayCoinSound()
    {
        if (effectAudioSource != null)
        {
            effectAudioSource.PlayOneShot(coinClip);
        }
    }
    public void PlayChooseItemSound()
    {
        if (effectAudioSource != null)
        {
            effectAudioSource.PlayOneShot(clickItemClip);
        }
    }

    public void PlayGameOverSound()
    {
        if (effectAudioSource != null)
        {
            effectAudioSource.PlayOneShot(gameoverClip);
        }
    }
    public void PlayVictorySound()
    {
        if (effectAudioSource != null)
        {
            effectAudioSource.PlayOneShot(victoryClip);
        }
    }

    public void PlaySoundClip(AudioClip sound)
    {
        if (effectAudioSource != null)
        {
            effectAudioSource.PlayOneShot(sound);
        }
    }

    public void ChangeSoundVolume(float _change)
    {
        float currentVolume = PlayerPrefs.GetFloat("soundVolume");
        currentVolume += _change;

        if (currentVolume > 1)
            currentVolume = 0;
        else if (currentVolume < 0)
            currentVolume = 1;

        backgroundAudioSource.volume = currentVolume;

        PlayerPrefs.SetFloat("soundVolume", currentVolume);
    }

    public void ChangeEffectVolume(float _change)
    {
        float currentVolume = PlayerPrefs.GetFloat("effectVolume");
        currentVolume += _change;

        if (currentVolume > 1)
            currentVolume = 0;
        else if (currentVolume < 0)
            currentVolume = 1;

        backgroundAudioSource.volume = currentVolume;

        PlayerPrefs.SetFloat("effectVolume", currentVolume);
    }
}
