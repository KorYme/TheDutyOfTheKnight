using UnityEngine.Audio;
using System;
using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Script managing every audio in the game
/// </summary>
public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("Main Mixer")]
    [SerializeField] private AudioMixerGroup audioMixerGroup;
    
    [Header("All the clips")]
    [SerializeField] private Sounds[] sounds;

    private Sounds currentSoundtrack;
    private string nextSoundtrack;
    private List<Sounds> liSTCurrentlyPlayed;

    //Singleton initialization
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        InitializeAllClips();
        liSTCurrentlyPlayed = new List<Sounds>();
    }

    /// <summary>
    /// Create an audio source for each clip and set it with the right parameter
    /// </summary>
    void InitializeAllClips()
    {
        foreach (Sounds s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.pitch = s.pitch;
            if (s.soundtrack)
            {
                s.source.volume = 0;
            }
            else
            {
                s.source.volume = s.volume;
            }
            s.source.outputAudioMixerGroup = audioMixerGroup;
        }
        currentSoundtrack = null;
        nextSoundtrack = "";
    }

    private void FixedUpdate()
    {
        if (currentSoundtrack != null ? !currentSoundtrack.source.isPlaying : currentSoundtrack != null)
        {
            PlayClip(nextSoundtrack);
        }
        IncreaseOrDecreaseVolumeOtherST();
    }

    /// <summary>
    /// Play a clip
    /// </summary>
    /// <param name="name">The name of the clip</param>
    public void PlayClip(string name)
    {
        if (name == "")
        {
            return;
        }
        Sounds s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("The clip " + name + " doesn't exist !");
            return;
        }
        else if (s.soundtrack)
        {
            //StopAllSoundTrack();
            currentSoundtrack = s;
            nextSoundtrack = s.nextSoundName != "" ? s.nextSoundName : s.name;
            if (!liSTCurrentlyPlayed.Contains(s))
            {
                liSTCurrentlyPlayed.Add(s);
            }
        }
        s.source.Play();
    }

    /// <summary>
    /// Switch between two soundtracks with a downscaling effect
    /// </summary>
    void IncreaseOrDecreaseVolumeOtherST()
    {
        for (int i = 0; i < liSTCurrentlyPlayed.Count; i++)
        {
            //Decrease here
            if (liSTCurrentlyPlayed[i] != currentSoundtrack)
            {
                if (liSTCurrentlyPlayed[i].source.volume > 0)
                {
                    liSTCurrentlyPlayed[i].source.volume -= Time.fixedDeltaTime * liSTCurrentlyPlayed[i].volume * 0.5f;
                }
                else
                {
                    liSTCurrentlyPlayed[i].source.Stop();
                    liSTCurrentlyPlayed.Remove(liSTCurrentlyPlayed[i]);
                }
            }
            else if (currentSoundtrack.source.volume < currentSoundtrack.volume)
            {
                currentSoundtrack.source.volume += Time.deltaTime * currentSoundtrack.volume * 0.3f;
            }
        }
    }
    
    /// <summary>
    /// Stop every soundtracks with a downscaling effect
    /// </summary>
    public void StopAllSoundTrack()
    {
        currentSoundtrack = null;
        nextSoundtrack = "";
        foreach (Sounds s in sounds)
        {
            if (s.soundtrack)
            {
                s.source.Stop();
            }
        }
    }

    /// <summary>
    /// Stop every clips immediately
    /// </summary>
    public void StopAllClips()
    {
        foreach (Sounds s in sounds)
        {
            if (!s.soundtrack)
            {
                s.source.Stop();
            }
        }
    }
}
