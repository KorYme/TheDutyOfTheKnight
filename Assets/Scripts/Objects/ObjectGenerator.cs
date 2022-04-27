using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectGenerator : MonoBehaviour
{
    public ObjectsData objectData;
    private SpriteRenderer spriteRenderer;
    private bool isInRange;
    private Merchant merchant;

    protected virtual void Start()
    {
        //Make sure the sprite is the good one
        spriteRenderer = gameObject.transform.Find("Graphic").gameObject.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = objectData.sprite;
        isInRange = false;
        merchant = GameObject.FindGameObjectWithTag("Merchant").GetComponent<Merchant>();
    }

    public virtual void TakeObject()
    {
        HeroStats.instance.IncreaseMaxHealthHero(objectData.maxHealthGiven);
        HeroStats.instance.HealHero(objectData.healthGiven);
        HeroStats.instance.speed += objectData.speedGiven;
        HeroStats.instance.heroAttack += objectData.attackGiven;
        PlayerInventory.instance.nbCoins -= objectData.coinCost;
        if (objectData.addToInventory)
        {
            //Ajouter à l'inventaire
        }
        Destroy(gameObject);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (!isInRange && DialogueManager.instance.currentPanelUser == "Object")
            {
                DialogueManager.instance.PanelDisable();
            }
            else if (isInRange)
            {
                DialogueManager.instance.currentPanelUser = "Object";
                if (RoomManager.instance.IsItShop())
                {
                    if (PlayerInventory.instance.nbCoins < objectData.coinCost)
                    {
                        DialogueManager.instance.UpdateTheScreen(merchant.nameMerchant, "Tu n'as pas assez d'argent, il va te falloir " + (objectData.coinCost - PlayerInventory.instance.nbCoins).ToString() + " pieces de plus si tu comptes me l'acheter !");
                        if (!DialogueManager.instance.panelOpen)
                        {
                            DialogueManager.instance.PanelEnable();
                        }

                    }
                    else
                    {
                        if (DialogueManager.instance.panelOpen)
                            Debug.Log("C'est ca ouais");
                        DialogueManager.instance.PanelDisable();
                        TakeObject();
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
            Debug.Log(isInRange);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            isInRange = false;
            if (DialogueManager.instance.currentPanelUser == "Object")
            {
                DialogueManager.instance.PanelDisable();
            }
        }
    }
        
}
