using UnityEngine;

/// <summary>
/// Script managing all the movement of the player
/// </summary>
public class HeroMovement : MonoBehaviour
{
    public static HeroMovement instance;
    //Singleton Initialization
    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("More than one HeroMovement instance in the game !");
        }
        instance = this;
    }

    [HideInInspector] public Rigidbody2D rb;
    private Animator animator;
    private Vector3 velocity = Vector3.zero;
    [HideInInspector] public bool canPlayerMove;

    //Initialization
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        canPlayerMove = true;
    }

    void FixedUpdate()
    {
        if (LevelManager.instance.pauseMenu)
            return;
        //Move the player with the project setting keybinding
        if (canPlayerMove)
        {
            float horizontalInput = Input.GetAxis("Horizontal") * HeroStats.instance.speed * Time.fixedDeltaTime;
            float verticalInput = Input.GetAxis("Vertical") * HeroStats.instance.speed * Time.fixedDeltaTime;
            Vector3 targetVelocity = new Vector2(horizontalInput, verticalInput);
            rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref velocity, .05f);
            animator.SetFloat("Speed", Mathf.Abs(horizontalInput) + Mathf.Abs(verticalInput));
        }
    }

    /// <summary>
    /// Play the falling animation and disable the controls on the player
    /// </summary>
    public void IsFalling()
    {
        animator.SetTrigger("IsFalling");
        AudioManager.instance.PlayClip("Falling");
        AllowMovement(false);
        RoomManager.instance.EnemiesMoveEnable(false);
    }

    /// <summary>
    /// Respawn the player and give him access to the controls -
    /// Called at the moment the falling animation ends
    /// </summary>
    public void RespawnInRoomAnimation()
    {
        RoomManager.instance.RespawnInRoom();
        AllowMovement(true);
    }

    /// <summary>
    /// Give the controls back to the player or take them away
    /// </summary>
    /// <param name="that">Enable if true and disable if false</param>
    public void AllowMovement(bool that)
    {
        canPlayerMove = that;
        rb.velocity = Vector3.zero;
    }
}