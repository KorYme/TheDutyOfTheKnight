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

    public InputData inputData;
    public PlayerInventory playerInventory;
    private Animator animator;
    private bool isMoving;
    private bool inventoryOpen;
    public TMP_Text[] inventoryCount;
    public float initialTimeToClose;
    private float timeToClose;

    void Start()
    {
        animator = GetComponent<Animator>();
        UpdateInventory();
        timeToClose = initialTimeToClose;
        //playerInventory = PlayerInventory.instance;
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

    public void ShowInventory()
    {
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
        inventoryCount[1].text = playerInventory.nbKey.ToString();
        inventoryCount[2].text = playerInventory.nbKeyBoss.ToString();
    }
}
