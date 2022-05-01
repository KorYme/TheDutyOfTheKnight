using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Totems : MonoBehaviour
{
    private bool inRange;
    private bool isPraying;
    private bool justPrayed;
    private DialogueManager dialogueManager;
    private TotemsRoom totemsRoom;
    private HeroStats heroStats;

    [Header ("To define values")]
    public TotemsData totemsData;
    public InputData inputData;

    private void Start()
    {
        inRange = false;
        isPraying = false;
        dialogueManager = DialogueManager.instance;
        totemsRoom = transform.parent.GetComponent<TotemsRoom>();
        heroStats = GameObject.FindGameObjectWithTag("Player").GetComponent<HeroStats>();
    }

    private void Update()
    {
        if (inRange && !dialogueManager.isMoving)
        {
            if (Input.GetKeyDown(inputData.interact))
            {
                dialogueManager.currentPanelUser = gameObject;
                if (dialogueManager.panelOpen)
                {
                    dialogueManager.PanelDisable();
                }
                else 
                {
                    if (totemsRoom.roomAlreadyPrayed)
                    {
                        dialogueManager.UpdateTheScreen(totemsData.name, "You've already prayed a totem in this room, go away !", 0);
                    }
                    else if (justPrayed)
                    {
                        dialogueManager.PanelDisable();
                        justPrayed = false;
                    }
                    else
                    {
                        dialogueManager.UpdateTheScreen(totemsData.name, totemsData.description, 2);
                        isPraying = true;
                    }
                    dialogueManager.PanelEnable();
                }
            }
            if (Input.GetKeyDown(inputData.accept) && isPraying)
            {
                totemsRoom.roomAlreadyPrayed = true;
                isPraying = false;
                if (dialogueManager.currentPanelUser == gameObject && dialogueManager.panelOpen)
                {
                    heroStats.AddStatsHero(totemsData);
                    dialogueManager.UpdateTheScreen(totemsData.name, totemsData.textForPrayer, 0);
                    justPrayed = true;
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            inRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            inRange = false;
            isPraying = false;
            justPrayed = false;
            if (dialogueManager.currentPanelUser == gameObject)
            {
                dialogueManager.PanelDisable();
            }
        }
    }

}
