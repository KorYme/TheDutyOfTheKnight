using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    public AudioClip[] playlist;
    private AudioSource audioSource;
    private int musicIndex;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        musicIndex = 0;
        audioSource.clip = playlist[musicIndex];
        audioSource.Play();
    }

    void Update()
    {
        if (!audioSource.isPlaying)
        {
            PlayNextSong();
        }
    }

    void PlayNextSong()
    {
        musicIndex++;
        if (musicIndex == playlist.Length)
        {
            musicIndex = 0;
        }
        audioSource.clip = playlist[musicIndex];
        audioSource.Play();
    }
}
