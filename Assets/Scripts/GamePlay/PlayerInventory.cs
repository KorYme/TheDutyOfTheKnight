using UnityEngine;

/// <summary>
/// Script managing the player's inventory and the minimap
/// </summary>
public class PlayerInventory : MonoBehaviour
{
    public static PlayerInventory instance;
    //Singleton initialization
    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("There is more than one PlayerInventory in the game");
            return;
        }
        instance = this;
    }

    [SerializeField] private InputData inputData;
    [SerializeField] private GameObject miniMap;

    [Header ("Current stuff")]
    public int nbCoins;
    public int nbPotionRefresh;
    public int nbKeyBoss;

    private InventoryPanel inventoryPanel;
    [HideInInspector] public bool miniMapOpen;

    private void Start()
    {
        miniMapOpen = false;
        miniMap.SetActive(false);
        inventoryPanel = InventoryPanel.instance;
    }

    private void Update()
    {
        if (Input.GetKeyDown(inputData.useItem) && HeroMovement.instance.canPlayerMove)
        {
            UseRefreshPotion();
        }
        if (Input.GetKeyDown(KeyCode.K) && LevelGenerator.instance.testMode)
        {
            inventoryPanel.ShowInventory();
            nbPotionRefresh = 100;
            nbKeyBoss = 3;
            nbCoins = 1000;
            InventoryPanel.instance.UpdateInventory();
            HeroStats.instance.HealHero(100f);
            if (miniMapOpen)
            {
                LevelGenerator.instance.SeeAllMiniMap();
            }
        }
        if (Input.GetKeyDown(inputData.miniMap) && !LevelManager.instance.pauseMenu && HeroMovement.instance.canPlayerMove)
        {
            if (miniMapOpen)
            {
                AudioManager.instance.PlayClip("MiniMapOpen");
                MiniMapDisable();
            }
            else
            {
                AudioManager.instance.PlayClip("MiniMapClose");
                MiniMapEnable();
            }
        }
    }

    /// <summary>
    /// Add an object to the inventory
    /// </summary>
    /// <param name="objectData">The object's data</param>
    public void AddToInventory(ObjectsData objectData)
    {
        nbCoins += objectData.coinGiven;
        nbPotionRefresh += objectData.refreshPotionGiven;
        nbKeyBoss += objectData.keyBossGiven;
        nbKeyBoss = nbKeyBoss > 3 ? 3 : nbKeyBoss;
        inventoryPanel.UpdateInventory();
        inventoryPanel.ShowInventory();
    }

    /// <summary>
    /// Refresh all the cooldowns and remove a refresh potion of the inventory if the player has already one
    /// </summary>
    void UseRefreshPotion()
    {
        if (nbPotionRefresh > 0)
        {
            CoolDownManager.instance.InitializeCDTo0();
            CoolDownManager.instance.DisplayRefreshKeyButton();
            nbPotionRefresh--;
            inventoryPanel.UpdateInventory();
            inventoryPanel.ShowInventory();
        }
    }

    /// <summary>
    /// Enable the minimap
    /// </summary>
    public void MiniMapEnable()
    {
        miniMapOpen = true;
        miniMap.SetActive(true);
    }

    /// <summary>
    /// Disable the minimap
    /// </summary>
    public void MiniMapDisable()
    {
        miniMapOpen = false;
        miniMap.SetActive(false);
    }
}
