using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Totems : MonoBehaviour
{
    private bool inRange;
    private bool isPraying;
    private bool eToClose;
    private bool firstInteraction;
    private float prayTime;
    private DialogueManager dialogueManager;
    private HeroStats heroStats;
    public List<GameObject> totemsList;
    [HideInInspector] public bool hasAlreadyPrayed;

    [Header ("To define values")]
    public TotemsData totemsData;
    public InputData inputData;

    private void Start()
    {
        dialogueManager = DialogueManager.instance;
        heroStats = HeroStats.instance;
        inRange = false;
        isPraying = false;
        eToClose = false;
        hasAlreadyPrayed = false;
        for (int i = 0; i < transform.parent.childCount; i++)
        {
            totemsList.Add(transform.parent.GetChild(i).gameObject);
        }
    }

    private void Update()
    {
        if (inRange && !dialogueManager.isMoving)
        {
            if (Input.GetKeyDown(inputData.interact))
            {
                dialogueManager.currentPanelUser = gameObject;
                if (eToClose)
                {
                    dialogueManager.PanelDisable();
                    eToClose = false;
                }
                else if (firstInteraction)
                {
                    dialogueManager.PanelEnable();
                    dialogueManager.UpdateTheScreen(totemsData.totemName, totemsData.description, 2);
                    firstInteraction = false;
                }
                else
                {
                    if (HasPlayerAlreadyPrayed())
                    {
                        firstInteraction = true;
                        dialogueManager.UpdateTheScreen(totemsData.totemName, "You've already prayed a totem in this room, go away !", 0);
                        eToClose = true;
                    }
                    else
                    {
                        isPraying = true;
                    }
                }
            }
            if (Input.GetKeyDown(inputData.close))
            {
                dialogueManager.PanelDisable();
            }
            if (Input.GetKey(inputData.interact) && isPraying)
            {
                prayTime += Time.deltaTime;
                if (prayTime >= 2)
                {
                    hasAlreadyPrayed = true;
                    isPraying = false;
                    eToClose = true;
                    if (dialogueManager.currentPanelUser == gameObject && dialogueManager.panelOpen)
                    {
                        heroStats.AddStatsHero(totemsData);
                        dialogueManager.UpdateTheScreen(totemsData.totemName, totemsData.textForPrayer, 0);
                    }
                }
            }
            else
            {
                prayTime = 0;
            }
        }
        else if (dialogueManager.panelOpen && !inRange && dialogueManager.currentPanelUser == gameObject)
        {
            dialogueManager.PanelDisable();
        }
    }

    bool HasPlayerAlreadyPrayed()
    {
        foreach (var item in totemsList)
        {
            if (item.GetComponent<Totems>().hasAlreadyPrayed)
            {
                return true;
            }
        }
        return false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            inRange = true;
            firstInteraction = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            inRange = false;
            isPraying = false;
            eToClose = false;
            prayTime = 0;
            if (dialogueManager.currentPanelUser == gameObject)
            {
                dialogueManager.PanelDisable();
            }
        }
    }

}
