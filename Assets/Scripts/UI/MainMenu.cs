using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;

/// <summary>
/// Script managing the main menu
/// </summary>
public class MainMenu : MonoBehaviour
{
    [Header("Loading Panel")]
    [SerializeField] private Slider loadingBar;
    [SerializeField] private TMP_Text loadingText;
    [SerializeField] private TMP_Text percentage;
    [SerializeField] private Animator animator;

    [Header("Other variables to fill")]
    [SerializeField] private GameObject settingsWindow;
    [SerializeField] private string levelToLoad;

    /// <summary>
    /// Launch a game - 
    /// Called on click
    /// </summary>
    public void StartGame()
    {
        AudioManager.instance.PlayClip("ButtonSound");
        Time.timeScale = 1f;
        StartCoroutine(LoadingNewScene(levelToLoad));
    }

    /// <summary>
    /// Open the settings menu -
    /// Called on click
    /// </summary>
    public void Settings()
    {
        AudioManager.instance.PlayClip("ButtonSound");
        settingsWindow.SetActive(true);
        gameObject.SetActive(false);
    }

    /// <summary>
    /// Close the game window -
    /// Called on click
    /// </summary>
    public void QuitGame()
    {
        AudioManager.instance.PlayClip("ButtonSound");
        Application.Quit();
    }

    /// <summary>
    /// Return to the main menu -
    /// Called on click
    /// </summary>
    public void MainMenuButton()
    {
        AudioManager.instance.PlayClip("ButtonSound");
        Time.timeScale = 1f;
        StartCoroutine(LoadingNewScene("_MainMenu"));
    }

    /// <summary>
    /// Load asynchroneously the scene and display a loading screen
    /// </summary>
    /// <param name="sceneToLoad">The sceen to load</param>
    /// <returns></returns>
    IEnumerator LoadingNewScene(string sceneToLoad)
    {
        yield return null;
        AsyncOperation async = SceneManager.LoadSceneAsync(sceneToLoad);
        async.allowSceneActivation = false;
        loadingText.text = "Loading ...";
        percentage.text = "0 %";
        animator.SetTrigger("Open");
        loadingBar.value = 0f;
        while (!async.isDone)
        {
            loadingBar.value += 0.01f;
            percentage.text = (Mathf.Floor(loadingBar.value * 100)).ToString() + "%";
            //loadingBar.value = (async.progress * 10 /9);
            //percentage.text = Mathf.Floor(async.progress * 1000 /9).ToString() + "%";

            if (async.progress >= 0.9f)
            {
                loadingText.text = "Press any key to continue";
                if (Input.anyKey)
                {
                    animator.SetTrigger("Close");
                    async.allowSceneActivation = true;
                }
            }
            yield return null;
        }
    }
}
