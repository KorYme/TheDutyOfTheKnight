using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInventory : MonoBehaviour
{
    public static PlayerInventory instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("There is more than one PlayerInventory in the game");
            return;
        }
        instance = this;
    }

    [Header ("Current stuff")]
    public int nbCoins;
    public int nbPotionRefresh;
    public int nbKeyBoss;
    private InventoryPanel inventoryPanel;
    [HideInInspector] public bool miniMapOpen;
    [SerializeField] private InputData inputData;
    [SerializeField] private GameObject miniMap;

    private void Start()
    {
        miniMapOpen = false;
        miniMap.SetActive(false);
        inventoryPanel = InventoryPanel.instance;
    }

    private void Update()
    {
        if (Input.GetKeyDown(inputData.useItem))
        {
            UseRefreshPotion();
        }
        if (Input.GetKeyDown(KeyCode.K) && LevelGenerator.instance.testMode)
        {
            inventoryPanel.ShowInventory();
            nbPotionRefresh++;
            nbKeyBoss = 3;
            InventoryPanel.instance.UpdateInventory();
            HeroStats.instance.HealHero(100f);
            if (miniMapOpen)
            {
                LevelGenerator.instance.SeeAllMiniMap();
            }
        }
        if (Input.GetKeyDown(inputData.miniMap) && !LevelManager.instance.pauseMenu)
        {
            if (miniMapOpen)
            {
                MiniMapDisable();
            }
            else
            {
                MiniMapEnable();
            }
        }
    }

    public void AddToInventory(ObjectsData objectData)
    {
        nbCoins += objectData.coinGiven;
        nbPotionRefresh += objectData.refreshPotionGiven;
        nbKeyBoss += objectData.keyBossGiven;
        nbKeyBoss = nbKeyBoss > 3 ? 3 : nbKeyBoss;
        inventoryPanel.UpdateInventory();
        inventoryPanel.ShowInventory();
    }

    void UseRefreshPotion()
    {
        if (nbPotionRefresh > 0)
        {
            CoolDownManager.instance.InitializeCDTo0();
            nbPotionRefresh--;
            inventoryPanel.UpdateInventory();
            inventoryPanel.ShowInventory();
        }
    }

    public void MiniMapEnable()
    {
        miniMapOpen = true;
        miniMap.SetActive(true);
    }

    public void MiniMapDisable()
    {
        miniMapOpen = false;
        miniMap.SetActive(false);
    }
}
