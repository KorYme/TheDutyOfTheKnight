using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InventoryPanel : MonoBehaviour
{
    public static InventoryPanel instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("There is more than one InventoryPanel in the game");
            return;
        }
        instance = this;
    }

    [SerializeField] private InputData inputData;
    [SerializeField] private PlayerInventory playerInventory;
    [SerializeField] private TMP_Text[] inventoryCount;
    [SerializeField] private TMP_Text textInventory;
    private Animator animator;
    private bool isMoving;
    private bool inventoryOpen;
    public float initialTimeToClose;
    private float timeToClose;

    void Start()
    {
        animator = GetComponent<Animator>();
        UpdateInventory();
        timeToClose = initialTimeToClose;
        textInventory.text = "Press [" + inputData.inventory.ToString() + "]";
    }

    private void FixedUpdate()
    {
        if (inventoryOpen)
        {
            timeToClose -= Time.fixedDeltaTime;
            if (timeToClose <= 0)
            {
                inventoryOpen = false;
                animator.SetTrigger("Close");
                isMoving = true;
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(inputData.inventory) && !isMoving)
        {
            UpdateInventory();
            if (inventoryOpen)
            {
                animator.SetTrigger("Close");
                isMoving = true;
                inventoryOpen = false;
            }
            else
            {
                timeToClose = initialTimeToClose;
                animator.SetTrigger("Open");
                isMoving = true;
                inventoryOpen = true;
            }
        }
    }

    /// <summary>
    /// Open the inventory panel for a short time
    /// </summary>
    public void ShowInventory()
    {
        UpdateInventory();
        timeToClose = initialTimeToClose;
        if (!inventoryOpen)
        {
            animator.SetTrigger("Open");
            isMoving = true;
            inventoryOpen = true;
        }
    }

    public void StopMovement()
    {
        isMoving = false;
    }

    public void UpdateInventory()
    {
        inventoryCount[0].text = playerInventory.nbCoins.ToString();
        inventoryCount[1].text = playerInventory.nbPotionRefresh.ToString();
        inventoryCount[2].text = playerInventory.nbKeyBoss.ToString();
    }
}
