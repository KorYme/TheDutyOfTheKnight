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
    public int nbHeart;
    public int nbSword;
    public int nbKeys;

    private bool isInventoryOpen;
    //public GameObject inventoryUI;

    private void Start()
    {
        //inventoryUI.SetActive(false);
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            OpenOrCloseInventory();
        }
    }

    void OpenOrCloseInventory()
    {
        isInventoryOpen = !isInventoryOpen;
        //inventoryUI.SetActive(isInventoryOpen);
    }

}
