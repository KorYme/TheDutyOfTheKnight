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

    private Animator animator;
    private bool isMoving;
    private bool inventoryOpen;
    public TMP_Text[] inventoryCount;

    void Start()
    {
        animator = GetComponent<Animator>();
        UpdateInventory();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && !isMoving)
        {
            if (inventoryOpen)
            {
                animator.SetTrigger("Close");
                isMoving = true;
                inventoryOpen = false;
            }
            else
            {
                animator.SetTrigger("Open");
                isMoving = true;
                inventoryOpen = true;
            }
        }
    }

    public void StopMovement()
    {
        isMoving = false;
    }

    public void UpdateInventory()
    {
        inventoryCount[0].text = PlayerInventory.instance.nbCoins.ToString();
        inventoryCount[1].text = PlayerInventory.instance.nbKey.ToString();
        inventoryCount[2].text = PlayerInventory.instance.nbKeyBoss.ToString();
    }
}
