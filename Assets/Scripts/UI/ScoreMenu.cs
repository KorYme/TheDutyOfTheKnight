using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class ScoreMenu : MonoBehaviour
{
    public TMP_Text victoryOrNot;
    public TMP_Text recapText;

    [Header("Loading Panel")]
    [SerializeField] private Slider loadingBar;
    [SerializeField] private TMP_Text loadingText;
    [SerializeField] private TMP_Text percentage;
    [SerializeField] private Animator animator;


    public void UpdateTheScore()
    {
        victoryOrNot.text = GameManager.instance.victory ? "Victory" : "Defeat";
        recapText.text = (GameManager.instance.victory ? "You've survived to the dungeon!" : "You died in the donjon!") + " Your journey lasts " + (GameManager.instance.timer / 60).ToString() + " minutes and " + (GameManager.instance.timer % 60).ToString() + " seconds, you also dealt " + GameManager.instance.score.ToString() + " damages to enemies.";
    }

    public void RetryButton()
    {
        AudioManager.instance.PlayClip("ButtonSound");
        Time.timeScale = 1f;
        StartCoroutine(LoadingNewScene("Etage1"));
    }

    public void MainMenuButton()
    {
        AudioManager.instance.PlayClip("ButtonSound");
        Time.timeScale = 1f;
        StartCoroutine(LoadingNewScene("_MainMenu"));
    }

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
