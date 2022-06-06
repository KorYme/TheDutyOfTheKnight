using UnityEngine;

/// <summary>
/// Script which manages all the sword attack of the player
/// </summary>
public class HeroHits : MonoBehaviour
{
    public static HeroHits instance;
    //Singleton initialization
    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("There is more than one HeroHits in the scene");
            return;
        }
        instance = this;
    }

    [SerializeField] private InputData inputdata;

    [Header ("Hits Variables")]
    [SerializeField] private LayerMask enemyLayers;
    [SerializeField] private LayerMask fireballLayer;
    [SerializeField] private float heroRange;
    
    [Header ("Animator System")]
    public float reloadTime;
    public bool isInReloadTime;

    private CoolDownManager coolDownManager;  
    private Camera cam;
    private Animator animator;
    [HideInInspector] public Vector3 worldPosition;
    [HideInInspector] public string direction;
    private float horizontalCursor;
    private float verticalCursor;

    void Start()
    {
        animator = GetComponent<Animator>();
        cam = Camera.main;
        isInReloadTime = false;
        coolDownManager = CoolDownManager.instance;
    }

    void FixedUpdate()
    {
        if (LevelManager.instance.pauseMenu)
            return;
        if (HeroMovement.instance.canPlayerMove)
        {
            LookingAtDirection();
        }
    }

    /// <summary>
    /// Send the direction the player is looking at to the animator
    /// </summary>
    void LookingAtDirection()
    {
        //Check the mouse coordinates in the world space
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = cam.nearClipPlane;
        worldPosition = cam.ScreenToWorldPoint(mousePos);
        horizontalCursor = worldPosition.x - transform.position.x;
        verticalCursor = worldPosition.y - transform.position.y;

        //Check in which direction the character will have to look
        if (Mathf.Abs(verticalCursor) >= Mathf.Abs(horizontalCursor))
        {
            animator.SetFloat("lookX", 0f);
            if (verticalCursor <= 0)
            {
                animator.SetFloat("lookY", -1f);
                direction = "AttackPointBot";
            }
            else
            {
                animator.SetFloat("lookY", 1f);
                direction = "AttackPointTop";
            }
        }
        else
        {
            animator.SetFloat("lookY", 0f);
            if (horizontalCursor < 0)
            {
                animator.SetFloat("lookX", -1f);
                direction = "AttackPointLeft";
            }
            else
            {
                animator.SetFloat("lookX", 1f);
                direction = "AttackPointRight";
            }
        }
    }

    private void Update()
    {
        if (LevelManager.instance.pauseMenu)
            return;
        if (Input.GetKey(inputdata.swordHit) && !isInReloadTime && HeroMovement.instance.canPlayerMove)
        {
            Attack();
        }
    }

    /// <summary>
    /// Play a hit animation
    /// </summary>
    void Attack()
    {
        animator.SetTrigger("Hitting");
        isInReloadTime = true;
        coolDownManager.ResetCoolDown("Hit");
        coolDownManager.DisplayRefreshKeyButton();
    }
    
    /// <summary>
    /// Deal the damage to the enemies in the range of the attack
    /// </summary>
    void HasHitted()
    {
        AudioManager.instance.PlayClip("SwordSwing" + Random.Range(1,6));
        foreach (Collider2D enemy in Physics2D.OverlapCircleAll(transform.Find(direction).transform.position, heroRange, enemyLayers))
        {
            if (enemy.GetComponent<Enemies>())
            {
                enemy.GetComponent<Enemies>().SendMessage("TakeDamage", HeroStats.instance.heroAttack);
            }
        }
        foreach (Collider2D fireball in Physics2D.OverlapCircleAll(transform.Find(direction).transform.position, heroRange, fireballLayer))
        {
            fireball.GetComponent<FireBall>().SetDirection(new Vector2(horizontalCursor,verticalCursor).normalized);
            fireball.GetComponent<FireBall>().playerFireball = true;
        }
    }
}
