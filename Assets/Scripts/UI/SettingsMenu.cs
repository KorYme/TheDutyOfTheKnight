using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.SceneManagement;

public class SettingsMenu : MonoBehaviour
{
    public AudioMixer audioMixer;
    public bool isFullScreen;
    public TMPro.TMP_Dropdown dropdown;
    Resolution[] resolutions;
    public GameObject entryMenu;

    public float currentVolume;
    public int currentIndexResolutions;

    public void Start()
    {
        GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioSource>();
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
        transform.Find("VolumeSlider").GetComponent<Slider>().value = GameManager.instance.currentVolume;
    }

    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("volume", volume);
        currentVolume = volume;
    }

    public void SetFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void CloseSettings()
    {
        gameObject.SetActive(false);
        entryMenu.SetActive(true);
    }

    public void RetryButton()
    {
        GameManager.instance.SaveParameters();
        SceneManager.LoadScene("Etage1");
    }

    public void MainMenuButton()
    {
        GameManager.instance.SaveParameters();
        SceneManager.LoadScene("MainMenu");
    }
}
