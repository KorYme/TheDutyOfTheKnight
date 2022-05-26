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
    public InputData inputData;

    private void Start()
    {
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
            InventoryPanel.instance.UpdateInventory();
            nbPotionRefresh++;
        }
    }

    public void AddToInventory(ObjectsData objectData)
    {
        nbCoins += objectData.coinGiven;
        nbPotionRefresh += objectData.refreshPotionGiven;
        nbKeyBoss += objectData.keyBossGiven;
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
}
