using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script managing all the totems
/// </summary>
public class Totems : MonoBehaviour
{
    private DialogueManager dialogueManager;
    private HeroStats heroStats;
    private bool inRange;
    private bool isPraying;
    private bool eToClose;
    private bool firstInteraction;
    private float prayTime;
    [HideInInspector] public List<GameObject> totemsList;
    [HideInInspector] public bool hasAlreadyPrayed;

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
                AudioManager.instance.PlayClip("Confirm");
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
            if (Input.GetKeyDown(inputData.close) && dialogueManager.panelOpen)
            {
                AudioManager.instance.PlayClip("Close");
                dialogueManager.PanelDisable();
                firstInteraction = true;
            }
            if (Input.GetKey(inputData.interact) && isPraying)
            {
                prayTime += Time.deltaTime;
                Interaction_Player.instance.SetImagE(1 - prayTime);
                if (prayTime >= 1)
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
            else if (prayTime > 0)
            {
                AudioManager.instance.PlayClip("Close");
                prayTime = 0;
                Interaction_Player.instance.SetImagE(1f);
            }
        }
        else if (dialogueManager.panelOpen && !inRange && dialogueManager.currentPanelUser == gameObject)
        {
            dialogueManager.PanelDisable();
        }
    }

    /// <summary>
    /// Check if the player has already prayed in this room
    /// </summary>
    /// <returns>True if the player has alreadyPrayed, else false</returns>
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

    /// <summary>
    /// Check if the player is in range of the totem
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            inRange = true;
            firstInteraction = true;
        }
    }

    /// <summary>
    /// Check if the player is not anymore in range of the totem
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
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
