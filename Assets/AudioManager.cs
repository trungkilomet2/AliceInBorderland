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
        backgroundAudioSource.clip = backgroundClip;
        backgroundAudioSource.Play();
    }

    public void PlayCoinSound()
    {
        effectAudioSource.PlayOneShot(coinClip);
    }
    public void PlayChooseItemSound()
    {
        effectAudioSource.PlayOneShot(clickItemClip);
    }

    public void PlayGameOverSound()
    {
        effectAudioSource.PlayOneShot(gameoverClip);
    }
    public void PlayVictorySound()
    {
        effectAudioSource.PlayOneShot(victoryClip);
    }

    public void PlaySoundClip(AudioClip sound)
    {
        effectAudioSource.PlayOneShot(sound);
    }
}
