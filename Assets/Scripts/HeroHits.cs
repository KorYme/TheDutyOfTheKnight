using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroHits : MonoBehaviour
{
    //Public variables
    public float heroRange;
    public LayerMask enemyLayers;

    //Animator System
    private Animator animator;
    public Vector3 worldPosition;
    private float horizontalCursor;
    private float verticalCursor;
    private Camera cam;
    public float reloadTime;
    public bool isInReloadTime;

    //Points System
    public string direction;
    private HeroStats heroStats;
    private CoolDownManager coolDownManager;
    

    void Start()
    {
        //Variable definition
        animator = GetComponent<Animator>();
        heroStats = GameObject.FindGameObjectWithTag("Player").GetComponent<HeroStats>();
        cam = GameObject.FindGameObjectWithTag("SpawnCamera").GetComponent<Camera>();
        isInReloadTime = false;
        coolDownManager = GameObject.FindGameObjectWithTag("CoolDownManager").GetComponent<CoolDownManager>();
    }

    void FixedUpdate()
    {
        //Check if we need to change the current camera
        if (transform.position.y < 95)
        {
            cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        }

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

        //Lance le syst�me d'attaque si le joueur n'est pas en isInReloadTime
        if (Input.GetMouseButton(0) && !isInReloadTime)
        {
            Attack();
        }
    }

    void Attack()
    {
        //Play the hit animation
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
                enemy.GetComponent<Enemies>().SendMessage("TakeDamage", heroStats.heroAttaque);
            }
        }
    }

    void HasEndedHit()
    {
        StartCoroutine(WaitShoot());
    }

    IEnumerator WaitShoot()
    {
        // L� on dit au script de patienter pendant l'animation de coup
        // On a fini d'attendre donc on repasse isInReloadTime en false, donc on va pouvoir taper � nouveau
        yield return new WaitForSeconds(reloadTime);
        isInReloadTime = false;
    }
}
