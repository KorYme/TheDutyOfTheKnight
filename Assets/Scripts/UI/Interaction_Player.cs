using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction_Player : MonoBehaviour
{
    private GameObject UI_Player;
    private float interactions;

    private void Start()
    {
        UI_Player = GameObject.FindGameObjectWithTag("UIPlayer");
        UI_Player.SetActive(false);
        interactions = 0;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Totem" || collision.tag == "Object" || collision.tag == "Merchant")
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
        if (collision.tag == "Totem" || collision.tag == "Object" || collision.tag == "Merchant")
        {
            interactions--;
            if (interactions <= 0)
            {
                UI_Player?.SetActive(false);
            }
        }
    }
}
