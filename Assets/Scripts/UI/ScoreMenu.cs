using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;

/// <summary>
/// Script managing the score menu
/// </summary>
public class ScoreMenu : MonoBehaviour
{
    [Header("EndGame Panel")]
    [SerializeField] private TMP_Text victoryOrNot;
    [SerializeField] private TMP_Text recapText;
    [Header("Loading Panel")]
    [SerializeField] private Slider loadingBar;
    [SerializeField] private TMP_Text loadingText;
    [SerializeField] private TMP_Text percentage;
    [SerializeField] private Animator animator;

    /// <summary>
    /// Update the endgame menu with the score and the timer -
    /// </summary>
    public void UpdateTheScore()
    {
        victoryOrNot.text = GameManager.instance.victory ? "Victory" : "Defeat";
        recapText.text = (GameManager.instance.victory ? "You've survived to the dungeon!" : "You died in the donjon!") + " Your journey lasts " + (GameManager.instance.timer / 60).ToString() + " minutes and " + (GameManager.instance.timer % 60).ToString() + " seconds, you also dealt " + GameManager.instance.score.ToString() + " damages to enemies.";
    }

    /// <summary>
    /// Reload the level -
    /// Called on click
    /// </summary>
    public void RetryButton()
    {
        AudioManager.instance.PlayClip("ButtonSound");
        Time.timeScale = 1f;
        StartCoroutine(LoadingNewScene("Etage1"));
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
        GameManager.instance.gameLaunched = false;
        HeroMovement.instance.canPlayerMove = false;
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

            if (loadingBar.value >= 1f)
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
