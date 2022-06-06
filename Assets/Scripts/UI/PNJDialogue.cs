using UnityEngine;

/// <summary>
/// Script of the PNJs
/// </summary>
public class PNJDialogue : MonoBehaviour
{
    private SpriteRenderer sprite;
    private Transform playerPos;
    private bool isInRange;
    private int sentencesIndex;
    private DialogueManager dialogueManager;

    [SerializeField] private InputData inputData;
    public string namePNJ;
    public SentencesPNJ[] dialogue;

    private void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        playerPos = HeroStats.instance.transform;
        sentencesIndex = 0;
        dialogueManager = DialogueManager.instance;
    }

    private void FixedUpdate()
    {
        sprite.flipX = (transform.position.x >= playerPos.position.x);
    }

    private void Update()
    {
        if (isInRange && !dialogueManager.isMoving)
        {
            if (Input.GetKeyDown(inputData.interact))
            {
                dialogueManager.currentPanelUser = gameObject;
                if (sentencesIndex < dialogue.Length)
                {
                    AudioManager.instance.PlayClip("Confirm");
                    if (!dialogueManager.panelOpen)
                    {
                        dialogueManager.PanelEnable();
                    }
                    if (sentencesIndex < dialogue.Length - 1)
                    {
                        dialogueManager.UpdateTheScreen(namePNJ, dialogue[sentencesIndex].sentences);
                    }
                    else
                    {
                        dialogueManager.UpdateTheScreen(namePNJ, dialogue[sentencesIndex].sentences, 0);
                    }
                    AudioManager.instance.StopAllClips();
                    AudioManager.instance.PlayClip(dialogue[sentencesIndex].clipSentences);
                    sentencesIndex++;
                }
                else
                {
                    AudioManager.instance.PlayClip("Close");
                    dialogueManager.PanelDisable();
                    sentencesIndex = 0;
                }
            }
            else if (Input.GetKeyDown(inputData.close))
            {
                AudioManager.instance.PlayClip("Close");
                if (isInRange && dialogueManager.panelOpen && !dialogueManager.isMoving)
                {
                    dialogueManager.PanelDisable();
                    sentencesIndex = 0;
                }
            }
        }
        else if (dialogueManager.panelOpen && !isInRange && dialogueManager.currentPanelUser == gameObject)
        {
            dialogueManager.PanelDisable();
        }
    }

    /// <summary>
    /// Check if the player is in range of a PNJ
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isInRange = true;
        }
    }

    /// <summary>
    /// Check if the player is not anymore in range of a PNJ
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isInRange = false;
            if (dialogueManager.currentPanelUser == gameObject)
            {
                dialogueManager.PanelDisable();
                sentencesIndex = 0;
            }
        }
    }
}
