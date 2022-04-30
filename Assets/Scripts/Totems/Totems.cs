using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Totems : MonoBehaviour
{
    private bool inRange;
    private DialogueManager dialogueManager;

    [Header ("To define values")]
    public TotemsData totemsData;
    public InputData inputData;

    private void Start()
    {
        inRange = false;
        dialogueManager = DialogueManager.instance;
    }

    private void Update()
    {
        if (Input.GetKeyDown(inputData.interact))
        {
            if (inRange)
            {
                dialogueManager.currentPanelUser = gameObject;
                //Need Modifications
                dialogueManager.UpdateTheScreen(totemsData.name, totemsData.description);
                if (!dialogueManager.panelOpen)
                {
                    dialogueManager.PanelEnable();
                }
                else if (dialogueManager.currentPanelUser == gameObject)
                {
                    dialogueManager.PanelDisable();
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.Y))
        {
            if (dialogueManager.currentPanelUser == gameObject && dialogueManager.panelOpen)
            {
                //Add stats
                dialogueManager.PanelDisable();
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
            if (dialogueManager.currentPanelUser == gameObject)
            {
                dialogueManager.PanelDisable();
            }
        }
    }

}
