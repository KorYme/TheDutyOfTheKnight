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
    [SerializeField] public InputData inputdata;

    [Header ("Hits Variables")]
    [SerializeField] public float heroRange;
    [SerializeField] public LayerMask enemyLayers;

    [SerializeField][HideInInspector] public Vector3 worldPosition;
    private Animator animator;
    private float horizontalCursor;
    private float verticalCursor;
    private Camera cam;

    [Header ("Animator System")]
    [SerializeField] public float reloadTime;
    [SerializeField] public bool isInReloadTime;

    //Points System
    [SerializeField][HideInInspector] public string direction;
    private CoolDownManager coolDownManager;
    

    void Start()
    {
        //Variable definition
        animator = GetComponent<Animator>();
        cam = Camera.main;
        isInReloadTime = false;
        coolDownManager = GameObject.FindGameObjectWithTag("CoolDownManager").GetComponent<CoolDownManager>();
    }

    void FixedUpdate()
    {
        LookingAtDirection();
    }

    void LookingAtDirection()
    {
        //Check the mouse coordinates in the world space
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = cam.nearClipPlane;
        worldPosition = cam.ScreenToWorldPoint(mousePos);
        horizontalCursor = worldPosition.x - this.transform.position.x;
        verticalCursor = worldPosition.y - this.transform.position.y;

        //Check in which direction the character will have to look
        if (Mathf.Abs(verticalCursor) >= Mathf.Abs(horizontalCursor))
        {
            animator.SetFloat("lookX", 0f);
            if (verticalCursor <= 0)
            {
                animator.SetFloat("lookY", -1f);
                direction = "AttBot";
            }
            else
            {
                animator.SetFloat("lookY", 1f);
                direction = "AttTop";
            }
        }
        else
        {
            animator.SetFloat("lookY", 0f);
            if (horizontalCursor < 0)
            {
                animator.SetFloat("lookX", -1f);
                direction = "AttLft";
            }
            else
            {
                animator.SetFloat("lookX", 1f);
                direction = "AttRgt";
            }
        }
    }

    private void Update()
    {
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
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(GameObject.FindGameObjectWithTag(direction).transform.position, heroRange, enemyLayers);
        foreach (Collider2D enemy in hitEnemies)
        {
            if (enemy.GetComponent<Enemies>())
            {
                enemy.GetComponent<Enemies>().SendMessage("TakeDamage", HeroStats.instance.heroAttack);
            }
        }
    }

    void HasEndedHit()
    {
        StartCoroutine(WaitShoot());
    }

    /// <summary>
    /// We wait for the hit cooldown to hit again
    /// </summary>
    /// <returns></returns>
    IEnumerator WaitShoot()
    {
        yield return new WaitForSeconds(reloadTime);
        isInReloadTime = false;
    }
}
