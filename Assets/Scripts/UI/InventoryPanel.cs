using UnityEngine;
using TMPro;

/// <summary>
/// Script managing all the behaviour related to the inventory panel
/// </summary>
public class InventoryPanel : MonoBehaviour
{
    public static InventoryPanel instance;
    //Singleton initialization
    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("There is more than one InventoryPanel in the game");
            return;
        }
        instance = this;
    }

    private Animator animator;
    private bool isMoving;
    private bool inventoryOpen;
    private float timeToClose;

    [SerializeField] private InputData inputData;
    [SerializeField] private PlayerInventory playerInventory;
    [SerializeField] private TMP_Text[] inventoryCount;
    [SerializeField] private TMP_Text textInventory;
    [SerializeField] private float initialTimeToClose;

    void Start()
    {
        animator = GetComponent<Animator>();
        UpdateInventory();
        timeToClose = initialTimeToClose;
        textInventory.text = "Press [" + inputData.inventory.ToString() + "]";
    }

    private void FixedUpdate()
    {
        if (inventoryOpen)
        {
            timeToClose -= Time.fixedDeltaTime;
            if (timeToClose <= 0)
            {
                inventoryOpen = false;
                animator.SetTrigger("Close");
                isMoving = true;
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(inputData.inventory) && !isMoving)
        {
            UpdateInventory();
            if (inventoryOpen)
            {
                animator.SetTrigger("Close");
                isMoving = true;
                inventoryOpen = false;
            }
            else
            {
                timeToClose = initialTimeToClose;
                animator.SetTrigger("Open");
                isMoving = true;
                inventoryOpen = true;
            }
        }
    }

    /// <summary>
    /// Open the inventory panel for a short time
    /// </summary>
    public void ShowInventory()
    {
        UpdateInventory();
        timeToClose = initialTimeToClose;
        if (!inventoryOpen)
        {
            animator.SetTrigger("Open");
            isMoving = true;
            inventoryOpen = true;
        }
    }

    /// <summary>
    /// Allow the inventory panel to be moved again -
    /// Called at the end of the opening and closing animations
    /// </summary>
    public void StopMovement()
    {
        isMoving = false;
    }

    /// <summary>
    /// Update the number of items in the inventory
    /// </summary>
    public void UpdateInventory()
    {
        inventoryCount[0].text = playerInventory.nbCoins.ToString();
        inventoryCount[1].text = playerInventory.nbPotionRefresh.ToString();
        inventoryCount[2].text = playerInventory.nbKeyBoss.ToString();
    }
}
