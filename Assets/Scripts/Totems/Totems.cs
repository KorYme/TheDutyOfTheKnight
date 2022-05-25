using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Totems : MonoBehaviour
{
    private bool inRange;
    private bool isPraying;
    private bool justPrayed;
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
        justPrayed = false;
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
                if (justPrayed)
                {
                    dialogueManager.PanelDisable();
                    justPrayed = false;
                }
                else if (HasPlayerAlreadyPrayed())
                {
                    dialogueManager.UpdateTheScreen(totemsData.name, "You've already prayed a totem in this room, go away !", 0);
                    justPrayed = true;
                }
                else if (isPraying)
                {
                    hasAlreadyPrayed = true;
                    isPraying = false;
                    if (dialogueManager.currentPanelUser == gameObject && dialogueManager.panelOpen)
                    {
                        heroStats.AddStatsHero(totemsData);
                        dialogueManager.UpdateTheScreen(totemsData.name, totemsData.textForPrayer, 0);
                        justPrayed = true;
                    }
                }
                else
                {
                    dialogueManager.UpdateTheScreen(totemsData.name, totemsData.description, 2);
                    isPraying = true;
                }
                dialogueManager.PanelEnable();
            }
            if (Input.GetKeyDown(inputData.close) && dialogueManager.panelOpen)
            {
                dialogueManager.PanelDisable();
            }
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
