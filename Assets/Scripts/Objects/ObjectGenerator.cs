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

    protected virtual void Start()
    {
        //Make sure the sprite is the good one
        merchant = GameObject.FindGameObjectWithTag("Merchant").GetComponent<Merchant>();
        spriteRenderer = gameObject.transform.Find("Graphic").gameObject.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = objectData.sprite;
        isInRange = false;
        firstTimeTouched = true;
    }

    public virtual void TakeObject()
    {
        HeroStats.instance.IncreaseMaxHealthHero(objectData.maxHealthGiven);
        HeroStats.instance.HealHero(objectData.healthGiven);
        HeroStats.instance.speed += objectData.speedGiven;
        HeroStats.instance.heroAttack += objectData.attackGiven;
        InventoryPanel.instance.UpdateInventory();
        if (objectData.addToInventory)
        {
            PlayerInventory.instance.AddToInventory(objectData);
        }
        Destroy(gameObject);
    }

    private void Update()
    {
        if (Input.GetKeyDown(inputData.interact))
        {
            if (!isInRange && DialogueManager.instance.currentPanelUser == gameObject)
            {
                DialogueManager.instance.PanelDisable();
            }
            else if (isInRange)
            {
                DialogueManager.instance.currentPanelUser = gameObject;
                if (RoomManager.instance.IsItShop())
                {
                    if (firstTimeTouched)
                    {
                        if (!DialogueManager.instance.panelOpen)
                        {
                            DialogueManager.instance.PanelEnable();
                        }
                        DialogueManager.instance.UpdateTheScreen(merchant.nameMerchant, objectData.description + " It costs " + objectData.coinCost + " coins.");
                        firstTimeTouched = false;
                    }
                    else
                    {
                        if (PlayerInventory.instance.nbCoins < objectData.coinCost)
                        {
                            DialogueManager.instance.UpdateTheScreen(merchant.nameMerchant, "You don't have enough money, you'll need " + (objectData.coinCost - PlayerInventory.instance.nbCoins).ToString() + " more coins to buy it !");
                            firstTimeTouched = true;
                        }
                        else
                        {
                            DialogueManager.instance.PanelDisable();
                            PlayerInventory.instance.nbCoins -= objectData.coinCost;
                            TakeObject();
                        }
                    }
                }
                else
                {
                    //Code Ramassage objet hors shop
                }
            }
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
            if (DialogueManager.instance.currentPanelUser == gameObject)
            {
                DialogueManager.instance.PanelDisable();
            }
        }
    }
}
