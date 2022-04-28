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

    public int nbCoins;
    public int nbKey;
    public int nbKeyBoss;

    public void AddToInventory(ObjectsData objectData)
    {
        nbCoins += objectData.coinGiven;
        nbKey += objectData.keyGiven;
        nbKeyBoss += objectData.keyBossGiven;
        InventoryPanel.instance.UpdateInventory();
    }
}
