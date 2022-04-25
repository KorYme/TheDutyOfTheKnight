using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectGenerator : MonoBehaviour
{
    public ObjectsData objectData;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        //Make sure the sprite is the good one
        spriteRenderer = gameObject.transform.Find("Graphic").gameObject.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = objectData.sprite;
    }

    public void TakeObject()
    {
        HeroStats.instance.IncreaseMaxHealthHero(objectData.maxHealthGiven);
        HeroStats.instance.HealHero(objectData.healthGiven);
        HeroStats.instance.speed += objectData.speedGiven;
        HeroStats.instance.heroAttack += objectData.attackGiven;
        Debug.Log(HeroStats.instance.heroHP);
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
            TakeObject();
        }
    }
}
