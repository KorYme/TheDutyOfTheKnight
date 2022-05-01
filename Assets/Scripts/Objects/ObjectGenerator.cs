using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectGenerator : MonoBehaviour
{
    public ObjectsData objectData;
    private SpriteRenderer spriteRenderer;
    private bool isInRange;
    private Merchant merchant;
    private bool firstTimeTouched;
    public InputData inputData;
    private DialogueManager dialogueManager;
    private HeroStats heroStats;
    private PlayerInventory playerInventory;

    protected virtual void Start()
    {
        //Make sure the sprite is the good one
        merchant = GameObject.FindGameObjectWithTag("Merchant").GetComponent<Merchant>();
        spriteRenderer = gameObject.transform.Find("Graphic").gameObject.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = objectData.sprite;
        isInRange = false;
        firstTimeTouched = true;
        dialogueManager = DialogueManager.instance;
        heroStats = HeroStats.instance;
        playerInventory = PlayerInventory.instance;
    }

    public virtual void TakeObject()
    {
        heroStats.IncreaseMaxHealthHero(objectData.maxHealthGiven);
        heroStats.HealHero(objectData.healthGiven);
        heroStats.speed += objectData.speedGiven;
        heroStats.heroAttack += objectData.attackGiven;
        InventoryPanel.instance.UpdateInventory();
        if (objectData.addToInventory)
        {
            playerInventory.AddToInventory(objectData);
        }
        Destroy(gameObject);
    }

    private void Update()
    {
        if (Input.GetKeyDown(inputData.interact))
        {
            if (!isInRange && dialogueManager.currentPanelUser == gameObject)
            {
                dialogueManager.PanelDisable();
            }
            else if (isInRange && !dialogueManager.isMoving)
            {
                dialogueManager.currentPanelUser = gameObject;
                if (RoomManager.instance.IsItShop())
                {
                    if (firstTimeTouched)
                    {
                        if (!dialogueManager.panelOpen)
                        {
                            dialogueManager.PanelEnable();
                        }
                        dialogueManager.UpdateTheScreen(merchant.nameMerchant, objectData.description + " It costs " + objectData.coinCost + " coins.", 1);
                        firstTimeTouched = false;
                    }
                    else if (dialogueManager.panelOpen)
                    {
                        firstTimeTouched = true;
                        dialogueManager.PanelDisable();
                    }
                }
                else
                {
                    if (firstTimeTouched)
                    {
                        if (!dialogueManager.panelOpen)
                        {
                            dialogueManager.PanelEnable();
                        }
                        dialogueManager.UpdateTheScreen(objectData.name, objectData.description, 3);
                        firstTimeTouched = false;
                    }
                    else if (dialogueManager.panelOpen)
                    {
                        firstTimeTouched = true;
                        dialogueManager.PanelDisable();
                    }
                }
            }
        }

        if (Input.GetKeyDown(inputData.accept))
        {
            if (RoomManager.instance.IsItShop())
            {
                if (dialogueManager.currentPanelUser == gameObject && !firstTimeTouched)
                {
                    if (playerInventory.nbCoins < objectData.coinCost)
                    {
                        dialogueManager.UpdateTheScreen(merchant.nameMerchant, "You don't have enough money, you'll need " + (objectData.coinCost - playerInventory.nbCoins).ToString() + " more coins to buy it !");
                        firstTimeTouched = true;
                    }
                    else if (!dialogueManager.isMoving)
                    {
                        dialogueManager.PanelDisable();
                        playerInventory.nbCoins -= objectData.coinCost;
                        TakeObject();
                    }
                }
            }
            else
            {
                if (dialogueManager.currentPanelUser == gameObject && !firstTimeTouched)
                {
                    dialogueManager.PanelDisable();
                    TakeObject();
                }
            }
        }

        if (!isInRange && dialogueManager.currentPanelUser == gameObject && dialogueManager.panelOpen)
        {
            dialogueManager.PanelDisable();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            isInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            isInRange = false;
            firstTimeTouched = true;
            if (dialogueManager.currentPanelUser == gameObject)
            {
                dialogueManager.PanelDisable();
            }
        }
    }
}
