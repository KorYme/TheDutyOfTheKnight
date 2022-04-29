using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Totems : MonoBehaviour
{
    private bool inRange;

    [Header ("To define values")]
    public TotemsData totemsData;
    public InputData inputData;

    private void Start()
    {
        inRange = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (inRange)
            {
                if (!DialogueManager.instance.panelOpen)
                {
                    DialogueManager.instance.PanelEnable();
                }
                else if (DialogueManager.instance.currentPanelUser == gameObject)
                {
                    DialogueManager.instance.PanelDisable();
                }
            }
            else
            {

            }
        }
        if (Input.GetKeyDown(KeyCode.Y))
        {
            if (DialogueManager.instance.currentPanelUser == gameObject && DialogueManager.instance.panelOpen)
            {
                //Add stats
                DialogueManager.instance.PanelDisable();
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
            if (DialogueManager.instance.currentPanelUser == gameObject)
            {
                DialogueManager.instance.PanelDisable();
            }
        }
    }

}
