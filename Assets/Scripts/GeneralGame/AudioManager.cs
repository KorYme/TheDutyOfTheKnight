using UnityEngine.Audio;
using System;
using UnityEngine;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField]
    private AudioMixerGroup audioMixerGroup;

    public Sounds[] sounds;
    private Sounds currentSoundtrack;
    private string nextSoundtrack;
    private List<Sounds> liSTCurrentlyPlayed;

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
        if (currentSoundtrack !=null ? !currentSoundtrack.source.isPlaying : false)
        {
            PlayClip(nextSoundtrack);
        }
        IncreaseOrDecreaseVolumeOtherST();
    }

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
    
    public void StopSoundTrack()
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
}
