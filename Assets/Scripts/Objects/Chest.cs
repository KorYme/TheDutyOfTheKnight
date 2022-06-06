using UnityEngine;

/// <summary>
/// Script for the chests
/// </summary>
public class Chest : MonoBehaviour
{
    [Header ("Inputs data")]
    [SerializeField] private InputData inputData;
    
    private Animator animator;
    private GameObject sprinkleGameobject;
    private bool canBeOpen;
    private bool isInRange;

    void Start()
    {
        animator = GetComponent<Animator>();
        canBeOpen = false;
        sprinkleGameobject = transform.Find("ParticleSystem").gameObject;
        sprinkleGameobject.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKey(inputData.interact) && isInRange && canBeOpen)
        {
            OpenTheChest();
        }
    }

    /// <summary>
    /// Set the openable or not
    /// </summary>
    /// <param name="that">True, openable and false if not</param>
    public void CanBeOpen(bool that)
    {
        canBeOpen = that;
        sprinkleGameobject.SetActive(that);
        tag = that ? "Chest" : "Untagged";
    }

    /// <summary>
    /// Play the opening animation
    /// </summary>
    public void OpenTheChest()
    {
        animator.SetTrigger("Opening");
        AudioManager.instance.PlayClip("Chest");
        Interaction_Player.instance.ForceExit();
        CanBeOpen(false);
    }

    /// <summary>
    /// Spawn the object in the chest
    /// </summary>
    public void InstantiateItem()
    {
        Instantiate(LootManager.instance.GetALoot(), transform.position, Quaternion.identity);
    }

    /// <summary>
    /// Check if the player is in range of the chest
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Coordinates"))
        {
            isInRange = true;
        }
    }

    /// <summary>
    /// Check if the player is not anymore in range of the chest
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Coordinates"))
        {
            isInRange = false;
        }
    }
}
