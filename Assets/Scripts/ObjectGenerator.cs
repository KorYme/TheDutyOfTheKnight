using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectGenerator : MonoBehaviour
{
    public ObjectsData objectData;
    private SpriteRenderer spriteRenderer;

    protected virtual void Start()
    {
        //Make sure the sprite is the good one
        spriteRenderer = gameObject.transform.Find("Graphic").gameObject.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = objectData.sprite;
    }

    public virtual void TakeObject()
    {
        HeroStats.instance.IncreaseMaxHealthHero(objectData.maxHealthGiven);
        HeroStats.instance.HealHero(objectData.healthGiven);
        HeroStats.instance.speed += objectData.speedGiven;
        HeroStats.instance.heroAttack += objectData.attackGiven;
        PlayerInventory.instance.nbCoins -= objectData.coinCost;
        Debug.Log("Vous avez désormais " + PlayerInventory.instance.nbCoins.ToString());
        if (objectData.addToInventory)
        {
            //Ajouter à l'inventaire
        }
        Destroy(gameObject);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (PlayerInventory.instance.nbCoins < objectData.coinCost)
            {
                //Afficher l'UI avec marqué "Pas assez de gold"
                Debug.Log("Pas assez de gold, il vous manque " + (objectData.coinCost - PlayerInventory.instance.nbCoins).ToString() + " golds");
            }
            else
            {
                TakeObject();
            }
        }
    }
}
