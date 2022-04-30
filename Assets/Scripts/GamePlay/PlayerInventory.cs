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
    public int nbKey;
    public int nbKeyBoss;
    private InventoryPanel inventoryPanel;

    private void Start()
    {
        inventoryPanel = InventoryPanel.instance;
    }

    public void AddToInventory(ObjectsData objectData)
    {
        nbCoins += objectData.coinGiven;
        nbKey += objectData.keyGiven;
        nbKeyBoss += objectData.keyBossGiven;
        inventoryPanel.ShowInventory();
        inventoryPanel.UpdateInventory();
    }
}
