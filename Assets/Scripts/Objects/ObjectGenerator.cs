using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectGenerator : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private bool isInRange;
    private PNJDialogue merchant;
    private bool firstTimeTouched;
    private DialogueManager dialogueManager;
    private HeroStats heroStats;
    private PlayerInventory playerInventory;
    private Animator animator;
    public ObjectsData objectData;
    public InputData inputData;

    private void Start()
    {
        spriteRenderer = transform.Find("Graphic").gameObject.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = objectData.sprite;
        isInRange = false;
        firstTimeTouched = true;
        dialogueManager = DialogueManager.instance;
        heroStats = HeroStats.instance;
        playerInventory = PlayerInventory.instance;
        animator = transform.GetChild(0).GetComponent<Animator>();
    }

    public void TakeObject()
    {
        heroStats.IncreaseMaxHealthHero(objectData.maxHealthGiven);
        heroStats.HealHero(objectData.healthGiven);
        heroStats.speed += objectData.speedGiven;
        heroStats.heroAttack += objectData.attackGiven;
        InventoryPanel.instance.UpdateInventory();
        AudioManager.instance.PlayClip(objectData.clipToPlay);
        if (objectData.addToInventory)
        {
            playerInventory.AddToInventory(objectData);
        }
        if (!RoomManager.instance.IsItShop())
        {
            Destroy(gameObject);
        }
    }

    public void ResetAnimator()
    {
        animator.SetTrigger("EnterRoom");
    }

    private void InitializeMerchant()
    {
        if (merchant == null)
        {
            merchant = RoomManager.instance.GiveMeMerchant().GetComponent<PNJDialogue>();
        }
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
                AudioManager.instance.PlayClip("Confirm");
                dialogueManager.currentPanelUser = gameObject;
                if (RoomManager.instance.IsItShop())
                {
                    if (firstTimeTouched)
                    {
                        if (!dialogueManager.panelOpen)
                        {
                            dialogueManager.PanelEnable();
                        }
                        InitializeMerchant();
                        dialogueManager.UpdateTheScreen(merchant.namePNJ, objectData.description + " It costs " + objectData.coinCost + " coins.", 1);
                        firstTimeTouched = false;
                    }
                    else if (dialogueManager.panelOpen)
                    {
                        if (playerInventory.nbCoins < objectData.coinCost)
                        {
                            dialogueManager.UpdateTheScreen(merchant.namePNJ, "You don't have enough money, you'll need " + (objectData.coinCost - playerInventory.nbCoins).ToString() + " more coins to buy it !");
                            firstTimeTouched = true;
                        }
                        else if (!dialogueManager.isMoving)
                        {
                            dialogueManager.UpdateTheScreen(merchant.namePNJ, "Thanks for this purchase, if you want to buy more do not hesitate, I have plenty more !");
                            firstTimeTouched = true;
                            playerInventory.nbCoins -= objectData.coinCost;
                            TakeObject();
                        }
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
                        dialogueManager.UpdateTheScreen(objectData.objectName, objectData.description, 3);
                        firstTimeTouched = false;
                    }
                    else if (dialogueManager.panelOpen)
                    {
                        dialogueManager.PanelDisable();
                        TakeObject();
                    }
                }
            }
        }

        if (Input.GetKeyDown(inputData.close) && dialogueManager.panelOpen && !dialogueManager.isMoving)
        {
            AudioManager.instance.PlayClip("Close");
            if (RoomManager.instance.IsItShop())
            {
                if (dialogueManager.currentPanelUser == gameObject)
                {
                    firstTimeTouched = true;
                    dialogueManager.PanelDisable();
                }
            }
            else
            {
                if (dialogueManager.currentPanelUser == gameObject)
                {
                    firstTimeTouched = true;
                    dialogueManager.PanelDisable();
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
