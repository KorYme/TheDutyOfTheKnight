using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PNJDialogue : MonoBehaviour
{
    private SpriteRenderer sprite;
    private Transform playerPos;
    private bool isInRange;
    private int sentencesIndex;
    [SerializeField] private InputData inputData;
    private DialogueManager dialogueManager;
    public string namePNJ;
    public SentencesPNJ[] dialogue;

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
        if (isInRange && !dialogueManager.isMoving)
        {
            if (Input.GetKeyDown(inputData.interact))
            {
                dialogueManager.currentPanelUser = gameObject;
                if (sentencesIndex < dialogue.Length)
                {
                    AudioManager.instance.PlayClip("Confirm");
                    if (!dialogueManager.panelOpen)
                    {
                        dialogueManager.PanelEnable();
                    }
                    if (sentencesIndex < dialogue.Length - 1)
                    {
                        dialogueManager.UpdateTheScreen(namePNJ, dialogue[sentencesIndex].sentences);
                    }
                    else
                    {
                        dialogueManager.UpdateTheScreen(namePNJ, dialogue[sentencesIndex].sentences, 0);
                    }
                    AudioManager.instance.StopAllClips();
                    AudioManager.instance.PlayClip(dialogue[sentencesIndex].clipSentences);
                    sentencesIndex++;
                }
                else
                {
                    AudioManager.instance.PlayClip("Close");
                    dialogueManager.PanelDisable();
                    sentencesIndex = 0;
                }
            }
            else if (Input.GetKeyDown(inputData.close))
            {
                AudioManager.instance.PlayClip("Close");
                if (isInRange && dialogueManager.panelOpen && !dialogueManager.isMoving)
                {
                    dialogueManager.PanelDisable();
                    sentencesIndex = 0;
                }
            }
        }
        else if (dialogueManager.panelOpen && !isInRange && dialogueManager.currentPanelUser == gameObject)
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
            if (dialogueManager.currentPanelUser == gameObject)
            {
                dialogueManager.PanelDisable();
                sentencesIndex = 0;
            }
        }
    }
}
