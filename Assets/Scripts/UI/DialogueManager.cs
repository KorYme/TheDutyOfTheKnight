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
    private Animator animator;
    private bool isMoving;
    [field:HideInInspector]
    public bool panelOpen;
    [field: HideInInspector]
    public GameObject currentPanelUser;

    private void Start()
    {
        animator = GetComponent<Animator>();
        panelName = transform.Find("NamePanel").Find("TextPanelName").GetComponent<TMP_Text>();
        panelContent = transform.Find("TextPanelContent").GetComponent<TMP_Text>();
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

    public void UpdateTheScreen(string name, string content)
    {
        panelName.text = name;
        panelContent.text = content;
    }
}
