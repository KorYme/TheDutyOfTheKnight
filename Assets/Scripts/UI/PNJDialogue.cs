using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PNJDialogue : MonoBehaviour
{
    private SpriteRenderer sprite;
    private Transform playerPos;
    private bool isInRange;
    private int sentencesIndex;
    private DialogueManager dialogueManager;
    public string namePNJ;
    public string[] sentencesPNJ;
    public InputData inputData;

    private void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        playerPos = HeroStats.instance.transform;
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
            if (sentencesIndex < sentencesPNJ.Length)
            {
                if (!dialogueManager.panelOpen)
                {
                    dialogueManager.PanelEnable();
                }
                if (sentencesIndex < sentencesPNJ.Length - 1)
                {
                    dialogueManager.UpdateTheScreen(namePNJ, sentencesPNJ[sentencesIndex]);
                }
                else
                {
                    dialogueManager.UpdateTheScreen(namePNJ, sentencesPNJ[sentencesIndex], 0);
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
