using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction_Player : MonoBehaviour
{
    public static Interaction_Player instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("There is more than one Interaction_Player instance in the game");
            return;
        }
        instance = this;
    }

    private GameObject UI_Player;
    private float interactions;

    private void Start()
    {
        UI_Player = transform.parent.Find("UI_Player").gameObject;
        UI_Player.SetActive(false);
        interactions = 0;
    }

    public void ForceExit()
    {
        if (interactions > 0)
        {
            interactions--;
            if (interactions <= 0)
            {
                UI_Player?.SetActive(false);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Totem" || collision.tag == "Object" || collision.tag == "Merchant" || collision.tag == "Chest")
        {
            interactions++;
            if (interactions > 0)
            {
                UI_Player?.SetActive(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Totem" || collision.tag == "Object" || collision.tag == "Merchant" || collision.tag == "Chest")
        {
            interactions--;
            if (interactions <= 0)
            {
                UI_Player?.SetActive(false);
            }
        }
    }
}
