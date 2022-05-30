using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField]
    private AudioMixerGroup audioMixerGroup;

    public Sounds[] sounds;

    enum musicPhase
    {
        Title,
        Dungeon,
        Boss,
        Death,
        Victory,
    }

    musicPhase currentMusicPhase;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);

        InitializeAllClips();
    }

    void InitializeAllClips()
    {
        foreach (Sounds s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.pitch = s.pitch;
            s.source.volume = s.volume;
            s.source.loop = s.loop;
            s.source.outputAudioMixerGroup = audioMixerGroup;
        }
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
        if (s.soundtrack)
        {
            switch (name)
            {
                case "Title":
                    currentMusicPhase = musicPhase.Title;
                    return;
                default:
                    return;
            }
        }
        s.source.Play();
    }
    
    public void StopAllSoundTrack()
    {
        foreach (Sounds s in sounds)
        {
            if (s.soundtrack && s.source.isPlaying)
            {
                s.source.Stop();
            }
        }
    }
}
