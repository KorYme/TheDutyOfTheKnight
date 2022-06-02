using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Interaction_Player : MonoBehaviour
{
    public static Interaction_Player instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("There is more than one Interaction_Player instance in the game");
            return;
        }
        instance = this;
    }

    private GameObject UI_Player;
    private Image imagE;
    private float interactions;

    [Header("List with tag for interaction")]
    public List<string> allTags;

    private void Start()
    {
        UI_Player = transform.parent.Find("UI_Player").gameObject;
        imagE = UI_Player.transform.Find("E_key").GetComponent<Image>();
        imagE.fillAmount = 1;
        interactions = 0;
        UI_Player.SetActive(false);
    }

    public void ForceExit()
    {
        if (interactions > 0)
        {
            interactions--;
            if (interactions <= 0)
            {
                interactions = 0;
                UI_Player?.SetActive(false);
            }
        }
    }

    public void SetImagE(float value)
    {
        imagE.fillAmount = value;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (allTags.Contains(collision.tag))
        {
            interactions++;
            if (interactions > 0)
            {
                UI_Player?.SetActive(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (allTags.Contains(collision.tag))
        {
            interactions--;
            if (interactions <= 0)
            {
                interactions = 0;
                UI_Player?.SetActive(false);
            }
        }
    }
}
