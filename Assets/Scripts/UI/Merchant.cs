using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Merchant : MonoBehaviour
{
    private SpriteRenderer sprite;
    private Transform playerPos;
    private bool isInRange;
    public string nameMerchant;
    public string[] sentencesMerchant;
    private int sentencesIndex;

    private void Start()
    {
        sprite = this.GetComponent<SpriteRenderer>();
        playerPos = GameObject.FindGameObjectWithTag("Player").transform;
        sentencesIndex = 0;
    }

    private void FixedUpdate()
    {
        sprite.flipX = (transform.position.x >= playerPos.position.x);
    }

    private void Update()
    {
        if (!isInRange && DialogueManager.instance.panelOpen && DialogueManager.instance.currentPanelUser == "Merchant")
        {
            DialogueManager.instance.PanelDisable();
        }
        else if (isInRange && Input.GetKeyDown(KeyCode.E))
        {
            DialogueManager.instance.currentPanelUser = "Merchant";
            if ( sentencesIndex < sentencesMerchant.Length)
            {
                if (!DialogueManager.instance.panelOpen)
                {
                    DialogueManager.instance.PanelEnable();
                }   
                DialogueManager.instance.UpdateTheScreen(nameMerchant, sentencesMerchant[sentencesIndex]);
                sentencesIndex++;
            }
            else
            {
                DialogueManager.instance.PanelDisable();
                sentencesIndex = 0;
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
        }
    }
}
