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
    public InputData inputData;
    private DialogueManager dialogueManager;

    private void Start()
    {
        sprite = this.GetComponent<SpriteRenderer>();
        playerPos = GameObject.FindGameObjectWithTag("Player").transform;
        sentencesIndex = 0;
        dialogueManager = DialogueManager.instance;
    }

    private void FixedUpdate()
    {
        sprite.flipX = (transform.position.x >= playerPos.position.x);
    }

    private void Update()
    {
        if (!isInRange && dialogueManager.panelOpen && dialogueManager.currentPanelUser == gameObject)
        {
            dialogueManager.PanelDisable();
        }
        else if (isInRange && Input.GetKeyDown(KeyCode.E))
        {
            dialogueManager.currentPanelUser = gameObject;
            if ( sentencesIndex < sentencesMerchant.Length)
            {
                if (!dialogueManager.panelOpen)
                {
                    dialogueManager.PanelEnable();
                }
                if (sentencesIndex < sentencesMerchant.Length-1)
                {
                    dialogueManager.UpdateTheScreen(nameMerchant, sentencesMerchant[sentencesIndex]);
                }
                else
                {
                    dialogueManager.UpdateTheScreen(nameMerchant, sentencesMerchant[sentencesIndex],0);
                }
                sentencesIndex++;
            }
            else
            {
                dialogueManager.PanelDisable();
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
