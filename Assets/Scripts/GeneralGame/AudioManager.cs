using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField]
    private AudioMixerGroup audioMixerGroup;

    public Sounds[] sounds;

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

    private void Start()
    {
        PlayClip("BossTheme");
    }

    public void PlayClip(string name)
    {
        Sounds s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("The clip " + name + " doesn't exist !");
            return;
        }
        s.source.Play();
    }
    
    public void StopClip(string name)
    {
        Sounds s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("The clip " + name + " doesn't exist !");
            return;
        }
        s.source.Stop();
    }
}
