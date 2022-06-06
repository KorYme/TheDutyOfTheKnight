using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine.UI;
using System.Linq;
using TMPro;

/// <summary>
/// Script managing all the game windows settings
/// </summary>
public class SettingsMenu : MonoBehaviour
{
    private Resolution[] resolutions;
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private TMP_Dropdown dropdown;
    [SerializeField] private GameObject entryMenu;
    [SerializeField] private bool isFullScreen;
    [SerializeField] private int currentIndexResolutions;

    private void Awake()
    {
        transform.Find("VolumeSlider").GetComponent<Slider>().value = GameManager.instance.currentVolume;
    }

    public void Start()
    {
        resolutions = Screen.resolutions.Select(resolution => new Resolution { width = resolution.width, height = resolution.height }).Distinct().ToArray();
        dropdown.ClearOptions();
        List<string> options = new List<string>();
        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            options.Add(resolutions[i].width + "x" + resolutions[i].height);
            if (resolutions[i].width == Screen.width && resolutions[i].height == Screen.height)
            {
                currentResolutionIndex = i;
            }
        }
        dropdown.AddOptions(options);
        dropdown.value = currentResolutionIndex;
        dropdown.RefreshShownValue();
        gameObject.SetActive(false);
    }

    /// <summary>
    /// Set the volume of the game -
    /// Called on slider movement
    /// </summary>
    /// <param name="volume">Volume of the game</param>
    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("volume", volume);
        GameManager.instance.currentVolume = volume;
    }

    /// <summary>
    /// Set the game in full screen or not -
    /// Called on click
    /// </summary>
    /// <param name="isFullScreen"></param>
    public void SetFullScreen(bool isFullScreen)
    {
        AudioManager.instance.PlayClip("ButtonSound");
        Screen.fullScreen = isFullScreen;
    }

    /// <summary>
    /// Change the resolution of the game -
    /// Called on dropdown change
    /// </summary>
    /// <param name="resolutionIndex"></param>
    public void SetResolution(int resolutionIndex)
    {
        AudioManager.instance.PlayClip("ButtonSound");
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        GameManager.instance.currentIndexResolutions = resolutionIndex;
    }

    /// <summary>
    /// Close the settings panel -
    /// Called on click
    /// </summary>
    public void CloseSettings()
    {
        AudioManager.instance.PlayClip("ButtonSound");
        gameObject.SetActive(false);
        entryMenu.SetActive(true);
    }
}
