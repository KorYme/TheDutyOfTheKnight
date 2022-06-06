using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Script managing the "E" key under the player
/// </summary>
public class Interaction_Player : MonoBehaviour
{
    public static Interaction_Player instance;
    //Singleton initialization
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
    [SerializeField] private List<string> allTags;

    private void Start()
    {
        UI_Player = transform.parent.Find("UI_Player").gameObject;
        imagE = UI_Player.transform.Find("E_key").GetComponent<Image>();
        imagE.fillAmount = 1;
        interactions = 0;
        UI_Player.SetActive(false);
    }

    /// <summary>
    /// Simulate the exit from the range of an interactable object
    /// </summary>
    public void ForceExit()
    {
        if (interactions > 0)
        {
            interactions--;
            if (interactions <= 0)
            {
                interactions = 0;
                if (UI_Player != null)
                {
                    UI_Player.SetActive(false);
                }
            }
        }
    }

    /// <summary>
    /// Fill the "E" key to render a completion
    /// </summary>
    /// <param name="value">Percentage of filling</param>
    public void SetImagE(float value)
    {
        imagE.fillAmount = value;
    }

    /// <summary>
    /// Check if the player is in range of an interactable object
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (allTags.Contains(collision.tag))
        {
            interactions++;
            if (interactions > 0)
            {
                if (UI_Player != null)
                {
                    UI_Player.SetActive(true);
                }
            }
        }
    }

    /// <summary>
    /// Check if the player is not anymore in range of an interactable object
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (allTags.Contains(collision.tag))
        {
            interactions--;
            if (interactions <= 0)
            {
                interactions = 0;
                if (UI_Player != null)
                {
                    UI_Player.SetActive(false);
                }
            }
        }
    }
}
