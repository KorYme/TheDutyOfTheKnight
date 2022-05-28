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

    [HideInInspector] public GameObject pauseMenuUI;
    public LevelGenerator levelGenerator;
    public GameObject hud;
    public GameObject endGameMenu;
    public InputData inputData;
    [HideInInspector] public bool pauseMenu;
    [SerializeField] private GameObject theBigGrid;

    private void Start()
    {
        GameManager.instance.InitGame();
        levelGenerator = GetComponent<LevelGenerator>();
        pauseMenu = false;
        hud.SetActive(true);
        pauseMenuUI.SetActive(false);
        endGameMenu.SetActive(false);
        Recreatelevel();
    }

    public void Update()
    {
        if (Input.GetKeyDown(inputData.menu) && !HeroStats.instance.isDead && !PlayerInventory.instance.miniMapOpen)
        {
            PauseAndUnpause();
        }
    }

    public void Recreatelevel()
    {
        foreach (Transform item in theBigGrid.transform)
        {
            if (item.name == "MiniMapBlocks")
            {
                foreach (Transform block in item)
                {
                    Destroy(block.gameObject);
                }
            }
            else
            {
                Destroy(item.gameObject);
            }
        }
        levelGenerator.CreatingLevel();
    }

    public void PauseAndUnpause()
    {
        pauseMenu = !pauseMenu;
        pauseMenuUI.SetActive(pauseMenu);
        InvertTime();
    }

    public void InvertTime()
    {
        Time.timeScale = Mathf.Abs(Time.timeScale - 1f);
    }

    public void EndGameMenu()
    {
        hud.SetActive(false);
        endGameMenu.SetActive(true);
    }
}
