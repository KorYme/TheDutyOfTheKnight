using UnityEngine;

/// <summary>
/// Script managing the scene and the level
/// </summary>
public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    //Singleton initialization
    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("There is more than one LevelManager in the scene");
            return;
        }
        instance = this;
    }

    private LevelGenerator levelGenerator;

    [Header ("Inputs Data")]
    [SerializeField] private InputData inputData;

    [Header ("GameObjects to fill")]
    [SerializeField] private GameObject theBigGrid;
    [SerializeField] private GameObject hud;
    [SerializeField] private GameObject endGameMenu;

    [HideInInspector] public GameObject pauseMenuUI;
    [HideInInspector] public bool pauseMenu;

    private void Start()
    {
        GameManager.instance.InitGame();
        levelGenerator = GetComponent<LevelGenerator>();
        pauseMenu = false;
        hud.SetActive(true);
        pauseMenuUI.SetActive(false);
        endGameMenu.SetActive(false);
        Recreatelevel();
        AudioManager.instance.PlayClip("DungeonTheme1");
    }

    public void Update()
    {
        if (Input.GetKeyDown(inputData.menu) && !HeroStats.instance.isDead && !PlayerInventory.instance.miniMapOpen)
        {
            PauseAndUnpause();
        }
    }

    /// <summary>
    /// Destroy every gameobjects in the level and create a new level
    /// </summary>
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

    /// <summary>
    /// Pause or unpause the game
    /// </summary>
    public void PauseAndUnpause()
    {
        pauseMenu = !pauseMenu;
        pauseMenuUI.SetActive(pauseMenu);
        InvertTime();
    }

    /// <summary>
    /// Invert the time scale
    /// </summary>
    public void InvertTime()
    {
        Time.timeScale = Mathf.Abs(Time.timeScale - 1f);
    }

    /// <summary>
    /// Display the end game menu
    /// </summary>
    public void EndGameMenu()
    {
        hud.SetActive(false);
        endGameMenu.SetActive(true);
    }
}