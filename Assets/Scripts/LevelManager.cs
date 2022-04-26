using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("There is more than one LevelManager in the scene");
            return;
        }
        instance = this;
    }

    public LevelGenerator levelGenerator;
    public GameObject hud;
    public GameObject pauseMenuUI;
    public GameObject endGameMenu;
    private bool pauseMenu;

    private void Start()
    {
        levelGenerator = GetComponent<LevelGenerator>();
        pauseMenu = false;
        hud.SetActive(true);
        pauseMenuUI.SetActive(false);
        endGameMenu.SetActive(false);
        Recreatelevel();
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseAndUnpause();
        }
    }

    public void Recreatelevel()
    {
        foreach (var item in GameObject.FindGameObjectsWithTag("Room"))
        {
            Destroy(item);
        }
        levelGenerator.CreatingLevel();
    }

    public void PauseAndUnpause()
    {
        pauseMenu = !pauseMenu;
        pauseMenuUI.SetActive(pauseMenu);
        Time.timeScale = Mathf.Abs(Time.timeScale - 1f);
    }

    public void EndGameMenu()
    {
        hud.SetActive(false);
        endGameMenu.SetActive(true);
    }
}
