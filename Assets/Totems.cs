using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Totems : MonoBehaviour
{
    private bool inRange;

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
            }
            else
            {

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
        }
    }

}
