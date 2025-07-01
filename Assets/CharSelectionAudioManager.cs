using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharSelectionAudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource backgroundAudioSource;
    [SerializeField] private AudioSource effectAudioSource;

    [SerializeField] private AudioClip backgroundClip;
    [SerializeField] private AudioClip clickOptionClip;
    [SerializeField] private AudioClip gameStartClip;
    void Start()
    {
        PlayBackGroundMusic();
    }

    public void PlayBackGroundMusic()
    {
        backgroundAudioSource.clip = backgroundClip;
        backgroundAudioSource.Play();
    }

    public void PlayClickOptionSound()
    {
        effectAudioSource.PlayOneShot(clickOptionClip);
    }

    public void PlayGameStartSound()
    {
        effectAudioSource.PlayOneShot(gameStartClip);
    }
}
