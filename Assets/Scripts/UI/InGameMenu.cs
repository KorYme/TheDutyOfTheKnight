using UnityEngine.SceneManagement;
using UnityEngine;

public class InGameMenu : MonoBehaviour
{
    public GameObject settingsWindow;
    public string levelToLoad;

    public void ResumeGame()
    {
        LevelManager.instance.PauseAndUnpause();
    }

    public void Settings()
    {
        settingsWindow.SetActive(true);
        gameObject.SetActive(false);
    }

    public void ReturnToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(levelToLoad);
    }
}
