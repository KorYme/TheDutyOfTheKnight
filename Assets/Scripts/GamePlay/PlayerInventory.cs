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
    private bool miniMapOpen;
    private Vector2 initialVelocity;
    private Rigidbody2D rb;
    [SerializeField] private InputData inputData;
    [SerializeField] private GameObject miniMap;

    private void Start()
    {
        initialVelocity = Vector2.zero;
        rb = GetComponent<Rigidbody2D>();
        inventoryPanel = InventoryPanel.instance;
        MiniMapDisable();
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
            nbKeyBoss++;
            InventoryPanel.instance.UpdateInventory();
        }
        if (Input.GetKeyDown(inputData.miniMap) && RoomManager.instance.IsNotInFight())
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
        Debug.Log("Enable");
        if (!LevelManager.instance.pauseMenu)
        {
            miniMapOpen = true;
            miniMap.SetActive(true);
            HeroMovement.instance.canPlayerMove = false;
            initialVelocity = rb.velocity;
            rb.velocity = Vector2.zero;
        }
    }

    public void MiniMapDisable()
    {
        Debug.Log("Disable");
        if (!LevelManager.instance.pauseMenu)
        {
            miniMapOpen = false;
            miniMap.SetActive(false);
            HeroMovement.instance.canPlayerMove = true;
            rb.velocity = initialVelocity;
        }
    }
}
