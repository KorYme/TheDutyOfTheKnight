using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.UI;

/// <summary>
/// Script managing all the menu in-game interactions
/// </summary>
public class InGameMenu : MonoBehaviour
{
    [Header ("Objects to fill")]
    [SerializeField] private GameObject settingsWindow;
    [SerializeField] private string levelToLoad;

    [Header("Loading Panel")]
    [SerializeField] private Slider loadingBar;
    [SerializeField] private TMP_Text loadingText;
    [SerializeField] private TMP_Text percentage;
    [SerializeField] private Animator animator;

    /// <summary>
    /// Resume the game -
    /// Called on click
    /// </summary>
    public void ResumeGame()
    {
        AudioManager.instance.PlayClip("ButtonSound");
        LevelManager.instance.PauseAndUnpause();
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
    /// Reload the game scene -
    /// Called on click
    /// </summary>
    public void RetryButton()
    {
        AudioManager.instance.PlayClip("ButtonSound");
        Time.timeScale = 1f;
        StartCoroutine(LoadingNewScene("Etage1"));
    }

    /// <summary>
    /// Go back to the menu -
    /// Called on click
    /// </summary>
    public void MainMenuButton()
    {
        AudioManager.instance.PlayClip("ButtonSound");
        Time.timeScale = 1f;
        StartCoroutine(LoadingNewScene("_MainMenu"));
    }

    /// <summary>
    /// Load a new scene asynchronously with a loading screen
    /// </summary>
    /// <param name="sceneToLoad"></param>
    /// <returns></returns>
    IEnumerator LoadingNewScene(string sceneToLoad)
    {
        GameManager.instance.gameLaunched = false;
        HeroMovement.instance.canPlayerMove = false;
        RoomManager.instance.EnemiesMoveEnable(false);
        AsyncOperation async = SceneManager.LoadSceneAsync(sceneToLoad);
        async.allowSceneActivation = false;
        loadingText.text = "Loading ...";
        percentage.text = "0 %";
        animator.SetTrigger("Open");
        loadingBar.value = 0f;
        while (!async.isDone)
        {
            loadingBar.value += 0.01f;
            percentage.text = (Mathf.Floor(loadingBar.value*100)).ToString() + "%";
            //loadingBar.value = (async.progress * 10 /9);
            //percentage.text = Mathf.Floor(async.progress * 1000 /9).ToString() + "%";

            if (loadingBar.value >= 1f)
            {
                loadingText.text = "Press any key to continue";
                if (Input.anyKey)
                {
                    AudioManager.instance.PlayClip("ButtonSound");
                    animator.SetTrigger("Close");
                    async.allowSceneActivation = true;
                }
            }
            yield return null;
        }
    }

}
