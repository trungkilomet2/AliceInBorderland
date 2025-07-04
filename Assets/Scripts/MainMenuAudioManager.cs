using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuAudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource backgroundAudioSource;
    [SerializeField] private AudioSource effectAudioSource;

    [SerializeField] private AudioClip backgroundClip;
    [SerializeField] private AudioClip clickOptionClip;
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
}
