using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroHits : MonoBehaviour
{
    public static HeroHits instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("There is more than one HeroHits in the scene");
            return;
        }
        instance = this;
    }

    [Header("Input Data")]
    public InputData inputdata;

    [Header ("Hits Variables")]
    public float heroRange;
    public LayerMask enemyLayers;
    public LayerMask fireballLayer;

    [SerializeField][HideInInspector] public Vector3 worldPosition;
    private Animator animator;
    private float horizontalCursor;
    private float verticalCursor;
    private Camera cam;

    [Header ("Animator System")]
    public float reloadTime;
    public bool isInReloadTime;

    //Points System
    [HideInInspector] public string direction;
    private CoolDownManager coolDownManager;
    

    void Start()
    {
        //Variable definition
        animator = GetComponent<Animator>();
        cam = Camera.main;
        isInReloadTime = false;
        coolDownManager = CoolDownManager.instance;
    }

    void FixedUpdate()
    {
        if (LevelManager.instance.pauseMenu || PlayerInventory.instance.miniMapOpen)
            return;
        LookingAtDirection();
    }

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
        if (LevelManager.instance.pauseMenu || PlayerInventory.instance.miniMapOpen)
            return;
        if (Input.GetKey(inputdata.swordHit) && !isInReloadTime)
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
    }

    void HasHitted()
    {
        foreach (Collider2D enemy in Physics2D.OverlapCircleAll(transform.Find(direction).transform.position, heroRange, enemyLayers))
        {
            if (enemy.GetComponent<Enemies>())
            {
                enemy.GetComponent<Enemies>().SendMessage("TakeDamage", HeroStats.instance.heroAttack);
            }
        }
        foreach (Collider2D fireball in Physics2D.OverlapCircleAll(transform.Find(direction).transform.position, heroRange, fireballLayer))
        {
            Debug.Log("Ca detecte");
            fireball.GetComponent<FireBall>().SetDirection(new Vector2(horizontalCursor,verticalCursor));
            fireball.GetComponent<FireBall>().playerFireball = true;
        }
    }
}
