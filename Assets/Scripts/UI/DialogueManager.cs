using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("There is more than one DialogueManager in the scene");
            return;
        }
        instance = this;
    }

    private TMP_Text panelName;
    private TMP_Text panelContent;
    private TMP_Text panelContinue;
    private Animator animator;
    [HideInInspector] public bool isMoving;
    [HideInInspector] public bool panelOpen;
    [HideInInspector] public GameObject currentPanelUser;
    public InputData inputData;

    private void Start()
    {
        animator = GetComponent<Animator>();
        panelName = transform.Find("NamePanel").Find("TextPanelName").GetComponent<TMP_Text>();
        panelContent = transform.Find("TextPanelContent").GetComponent<TMP_Text>();
        panelContinue = transform.Find("TextPanelContinue").GetComponent<TMP_Text>();
        currentPanelUser = null;
    }

    public void PanelEnable()
    {
        if (!isMoving && !panelOpen)
        {
            panelOpen = true;
            isMoving = true;
            animator.SetTrigger("OpenPanel");
        }
    }
    
    public void PanelDisable()
    {
        if (!isMoving && panelOpen)
        {
            panelOpen = false;
            isMoving = true;
            animator.SetTrigger("ClosePanel");
        }
    }

    public void PanelDialogueMoved()
    {
        isMoving = false;
    }

    public void UpdateTheScreen(string name, string content, int bottomRightText = -1)
    {
        StopCoroutine("PrintText");
        panelName.text = name;
        StartCoroutine("PrintText", content);
        switch (bottomRightText)
        {
            case 0:
                panelContinue.text = "Press " + inputData.interact.ToString() + " or " + inputData.close.ToString() + " to close >";
                return;
            case 1:
                panelContinue.text = "Press " + inputData.interact.ToString() + " to buy or " + inputData.close.ToString() + " to close >";
                return;
            case 2:
                panelContinue.text = " Hold " + inputData.interact.ToString() + " to pray or " + inputData.close.ToString() + " to close >";
                return;
            case 3:
                panelContinue.text = "Press " + inputData.interact.ToString() + " to pick up or " + inputData.close.ToString() + " to close >";
                return;
            case 4:
                panelContinue.text = "Press " + inputData.interact.ToString() + " to interact with the sphere or " + inputData.close.ToString() + " to leave >";
                return;
            default:
                panelContinue.text = "Press " + inputData.interact.ToString() + " to continue or " + inputData.close.ToString() + " to close >";
                return;
        }
    }

    IEnumerator PrintText(string text)
    {
        panelContent.text = "";
        bool fill = false;
        string stocking = "";
        foreach (char item in text.ToCharArray())
        {
            if (item == '<' || fill)
            {
                fill = true;
                stocking += item;
                if (item == '>')
                {
                    panelContent.text += stocking;
                    stocking = "";
                    yield return null;
                    fill = false;
                }
            }
            else
            {
                panelContent.text += item;
                yield return null;
            }
        }
    }
}