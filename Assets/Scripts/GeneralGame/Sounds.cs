using UnityEngine;

[System.Serializable]
public class Sounds
{
    public string name;
    public AudioClip clip;

    [Range(0f, 1f)]
    public float volume;
    [Range(.1f, 3f)]
    public float pitch;

    public bool soundtrack;
    public string nextSoundName;

    [HideInInspector]
    public AudioSource source;
}
