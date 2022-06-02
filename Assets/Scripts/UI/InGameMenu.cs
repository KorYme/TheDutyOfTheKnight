using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.UI;

public class InGameMenu : MonoBehaviour
{
    public GameObject settingsWindow;
    public string levelToLoad;

    [Header("Loading Panel")]
    [SerializeField] private Slider loadingBar;
    [SerializeField] private TMP_Text loadingText;
    [SerializeField] private TMP_Text percentage;
    [SerializeField] private Animator animator;

    public void ResumeGame()
    {
        AudioManager.instance.PlayClip("ButtonSound");
        LevelManager.instance.PauseAndUnpause();
    }

    public void Settings()
    {
        AudioManager.instance.PlayClip("ButtonSound");
        settingsWindow.SetActive(true);
        gameObject.SetActive(false);
    }

    public void ReturnToMenu()
    {
        AudioManager.instance.PlayClip("ButtonSound");
        Time.timeScale = 1f;
        SceneManager.LoadScene(levelToLoad);
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
            percentage.text = (Mathf.Floor(loadingBar.value*100)).ToString() + "%";
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
