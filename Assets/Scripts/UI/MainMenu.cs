using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public string levelToLoad;
    public GameObject settingsWindow;

    public void StartGame()
    {
        GameManager.instance.SaveParameters();
        SceneManager.LoadScene(levelToLoad);
    }
    public void Settings()
    {
        settingsWindow.SetActive(true);
        gameObject.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
