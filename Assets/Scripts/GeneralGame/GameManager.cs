using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [field:HideInInspector]
    public bool gameLaunched;
    public bool victory;
    public float score;
    public int timer;
    private SettingsMenu settingsMenu;

    public int currentIndexResolutions = -1;
    public float currentVolume;

    //Create the Singleton
    public static GameManager instance;
    private void Awake()
    {
        instance = null;
        if (instance != null)
        {
            Debug.LogError("More than one GameManager instance in the game !");
        }
        instance = this;
        settingsMenu = GameObject.FindGameObjectWithTag("SettingsMenu").GetComponent<SettingsMenu>();
        settingsMenu.gameObject.SetActive(false);
    }

    public void SaveParameters()
    {
        currentIndexResolutions = settingsMenu.currentIndexResolutions;
        currentVolume = settingsMenu.currentVolume;
    }

    /// <summary>
    /// Check if the game starts
    /// </summary>
    public void InitGame ()
    {
        Debug.Log("The game starts !");
        timer = 0;
        score = 0f;
        gameLaunched = true;
        victory = true;
        StartCoroutine(Timer());
    }

    /// <summary>
    /// Add 1 second to the timer each second
    /// </summary>
    /// <returns></returns>
    IEnumerator Timer()
    {
        yield return new WaitForSeconds(1f);
        if (gameLaunched)
        {
            timer++;
            StartCoroutine(Timer());
        }
    }

    /// <summary>
    /// Display the ending screen after dying and stops the timer
    /// </summary>
    public void Die()
    {
        gameLaunched = false;
        LevelManager.instance.EndGameMenu();
    }
}
