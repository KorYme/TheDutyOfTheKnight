using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroAbility : MonoBehaviour
{
    public static HeroAbility instance;
    private void Awake()
    {
        instance = null;
        if (instance != null)
        {
            Debug.LogError("More than one HeroAbility instance in the game !");
        }
        instance = this;
    }
    
    [Header("Ability unlocked")]
    public bool windUnlocked;
    public bool fireUnlocked;
    public bool earthUnlocked;

    [Header("Cooldowns states")]
    public bool windInCooldown;
    public bool fireInCooldown;
    public bool earthInCooldown;

    [Header("Cooldowns times")]
    public float cooldownWind;
    public float cooldownFire;
    public float cooldownEarth;

    [Header("KeyCodes")]
    public InputData inputData;

    [Header("To Define Values")]
    public HeroStats heroStats;
    public GameObject fireBall;
    public GameObject explosion;
    public float fireBallSpeed;
    public float radiusCharacter;
    public float knockBackFireBall;
    public float dashTime;
    public bool isDashing;
    public LayerMask enemyLayer;
    public LayerMask cantTPLayer;

    [HideInInspector] public bool shieldOpen;
    [HideInInspector] public bool damagingShield;
    [HideInInspector] public Vector3 cursorPosition;
    [HideInInspector] public Vector3 tpPosition;
    private bool explosionWithTP;
    private float dashDistance;
    private Vector2 destinationDash;
    private Collider2D[] tpZone;
    private CapsuleCollider2D playerCollider;
    private CircleCollider2D shieldCollider;
    private CoolDownManager coolDownManager;
    private SpriteRenderer spriteShield;
    private TrailRenderer dashTrail;
    private Rigidbody2D rb;
    private Animator animator;
    private Animator animatorShield;
    private List<Enemies> enemiesTouched;

    private void Start()
    {
        //Initializations
        coolDownManager = CoolDownManager.instance;
        spriteShield = transform.Find("Shield").GetComponent<SpriteRenderer>();
        animatorShield = transform.Find("Shield").GetComponent<Animator>();
        dashTrail = transform.Find("DashLane").GetComponent<TrailRenderer>();
        shieldCollider = transform.Find("Shield").GetComponent<CircleCollider2D>();
        playerCollider = GetComponent<CapsuleCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        damagingShield = false;
        dashTrail.emitting = false;
        shieldOpen = false;
        explosionWithTP = false;
        isDashing = false;
        enemiesTouched = new List<Enemies>();
    }

    private void Update()
    {
        if (isDashing)
        {
            transform.position = Vector2.MoveTowards(transform.position, destinationDash, Time.deltaTime / dashTime * dashDistance);
            if (Vector2.Distance(transform.position, destinationDash) < 0.001f)
            {
                EndDash();
            }
        }
        if (LevelManager.instance.pauseMenu || !HeroMovement.instance.canPlayerMove)
            return;
        //Check if the player clicked on an ability key
        if (Input.GetKey(inputData.abilityFire) && fireUnlocked && !fireInCooldown)
        {
            FireAbility();
        }
        else if (Input.GetKey(inputData.abilityWind) && windUnlocked && !windInCooldown)
        {
            WindAbility();
        }
        else if (Input.GetKey(inputData.abilityEarth) && earthUnlocked && !earthInCooldown && !shieldOpen)
        {
            EarthAbility();
        }
        else if (Input.GetKey(inputData.abilityExplosion) && earthUnlocked && windUnlocked && !earthInCooldown && !windInCooldown)
        {
            ExplosionAbility();
        }
        else if (Input.GetKey(inputData.abilityDash) && fireUnlocked && windUnlocked && !fireInCooldown && !windInCooldown)
        {
            DashAbility();
        }
        else if (Input.GetKey(inputData.abilityDamagingShield) && fireUnlocked && earthUnlocked && !fireInCooldown && !earthInCooldown && !shieldOpen)
        {
            ShieldDamageAbility();
        }
    }

    //USP ABILITIES

    /// <summary>
    /// Create a fireball and launch it in the direction of the cursor
    /// </summary>
    void FireAbility()
    {
        fireInCooldown = true;
        coolDownManager.ResetCoolDown("Fire");
        coolDownManager.DisplayRefreshKeyButton();
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mouseDirection = new Vector2(mousePosition.x - transform.position.x, mousePosition.y - transform.position.y);
        if (mouseDirection == Vector2.zero)
            mouseDirection = Vector2.up;
        GameObject bulletLaunch = Instantiate(fireBall, transform.position, Quaternion.identity);
        bulletLaunch.GetComponent<FireBall>().SetDirection(mouseDirection.normalized);
        bulletLaunch.GetComponent<FireBall>().fireBallSpeed = fireBallSpeed;
        rb.velocity += mouseDirection.normalized * -1 * knockBackFireBall;
        AudioManager.instance.PlayClip("Fireball");
    }

    /// <summary>
    /// Give the shield a color and launch the shield function
    /// </summary>
    void EarthAbility()
    {
        shieldOpen = true;
        earthInCooldown = true;
        coolDownManager.ResetCoolDown("Earth");
        coolDownManager.DisplayRefreshKeyButton();
        spriteShield.color = new Color(88, 255, 0);
        animatorShield.SetTrigger("ShieldEnter");
        StartCoroutine(ShieldDuration());
    }

    /// <summary>
    /// Teleport the player on the mouse position
    /// </summary>
    /// <param name="alreadyCheck">Do it without checking if the TP is right</param>
    void WindAbility(bool alreadyCheck = false)
    {
        if (CanUTP() || alreadyCheck)
        {
            windInCooldown = true;
            coolDownManager.ResetCoolDown("Wind");
            coolDownManager.DisplayRefreshKeyButton();
            tpPosition = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 0);
            animator.SetTrigger("TP");
            RoomManager.instance.EnemiesMoveEnable(false);
            HeroMovement.instance.canPlayerMove = false;
            rb.velocity = Vector2.zero;
            Interaction_Player.instance.ForceExit();
        }
    }

    /// <summary>
    /// Teleport the player on the mouse location if CanUTP is true and create a damaging zone around the location of the player
    /// </summary>
    void ExplosionAbility()
    {
        if (CanUTP())
        {
            earthInCooldown = true;
            coolDownManager.ResetCoolDown("Earth");
            explosionWithTP = true;
            WindAbility(true);
        }
    }

    /// <summary>
    /// Teleport the player on the mouse location if CanUTP is true and deal damage between the initial position and the new one
    /// </summary>
    void DashAbility()
    {
        if (CanUTP() && !isDashing)
        {
            fireInCooldown = true;
            coolDownManager.ResetCoolDown("Fire");
            windInCooldown = true;
            coolDownManager.ResetCoolDown("Wind");
            coolDownManager.DisplayRefreshKeyButton();
            isDashing = true;
            destinationDash = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
            dashTrail.emitting = true;
            HeroMovement.instance.canPlayerMove = false;
            RoomManager.instance.EnemiesMoveEnable(false);
            dashDistance = Vector2.Distance(transform.position, destinationDash);
            heroStats.invincibility = true;
            enemiesTouched.Clear();
            shieldCollider.isTrigger = true;
            playerCollider.isTrigger = true;
            animator.SetTrigger("EntryDash");
        }
    }

    /// <summary>
    /// Give the shield a color, damages and launch the shield function
    /// </summary>
    void ShieldDamageAbility()
    {
        fireInCooldown = true;
        coolDownManager.ResetCoolDown("Fire");
        EarthAbility();
        damagingShield = true;
        spriteShield.color = new Color(255,0,0);

    }

    //OTHER FUNCTIONS FOR ABILITY

    public void EndDash()
    {
        isDashing = false;
        dashTrail.emitting = false;
        HeroMovement.instance.canPlayerMove = true;
        RoomManager.instance.EnemiesMoveEnable(true);
        heroStats.invincibility = false;
        foreach (Enemies item in enemiesTouched)
        {
            item.TakeDamage(heroStats.dashDamage);
        }
        shieldCollider.isTrigger = false;
        playerCollider.isTrigger = false;
        animator.SetTrigger("ExitDash");
    }

    /// <summary>
    /// Activate the shield's sprite, its collider and make the player inivicible.
    /// Go back to normal after {shieldDuration} time.
    /// Also desactivate the damage of the shield
    /// </summary>
    /// <returns></returns>
    IEnumerator ShieldDuration()
    {
        yield return new WaitForSeconds(heroStats.shieldDuration);
        animatorShield.SetTrigger("ShieldExit");
        damagingShield = false;
    }

    /// <summary>
    /// Check if the mouse is somewhere the player can't TP (walls, obstacles, out of the screen)
    /// </summary>
    /// <returns>True if the player is allowed to, false if not</returns>
    bool CanUTP()
    {
        if (!Input.mousePresent || !Camera.main.rect.Contains(Camera.main.ScreenToViewportPoint(Input.mousePosition)))
        {
            return false;
        }
        tpZone = Physics2D.OverlapCircleAll(Camera.main.ScreenToWorldPoint(Input.mousePosition), radiusCharacter);
        foreach (Collider2D item in tpZone)
        {
            if (cantTPLayer == (cantTPLayer | (1 << item.gameObject.layer)))
            {
                return false;
            }
        }
        return true;
    }

    public void TP()
    {
        transform.position = tpPosition;
        if (explosionWithTP)
        {
            AudioManager.instance.PlayClip("Explosion");
            GameObject anExplosion = Instantiate(explosion, transform.position, Quaternion.identity);
            Destroy(anExplosion, 1f);
            foreach (var item in Physics2D.OverlapCircleAll(transform.position, 1.2f, enemyLayer))
            {
                item.gameObject.SendMessage("TakeDamage", heroStats.explosionDamage);
            }
            explosionWithTP = false;
        }
    }

    public void TPEnded()
    {
        HeroMovement.instance.canPlayerMove = true;
        RoomManager.instance.EnemiesMoveEnable(true);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isDashing)
        {
            if (collision.GetComponent<Enemies>() != null && !enemiesTouched.Contains(collision.GetComponent<Enemies>()))
            {
                enemiesTouched.Add(collision.GetComponent<Enemies>());
            }
        }
    }

    /// <summary>
    /// Update the cooldowns aftter an upgrade
    /// </summary>
    public void UpgradeCD(TotemsData totemsData)
    {
        cooldownEarth -= (cooldownEarth - totemsData.earthCooldownBonus > 0 ? totemsData.earthCooldownBonus : cooldownEarth);
        cooldownEarth = cooldownEarth > heroStats.shieldDuration ? cooldownEarth : heroStats.shieldDuration;
        cooldownWind -= (cooldownWind - totemsData.windCooldownBonus > 0 ? totemsData.windCooldownBonus : cooldownWind);
        cooldownFire -= (cooldownFire - totemsData.fireCooldownBonus > 0 ? totemsData.fireCooldownBonus : cooldownFire);
    }
}
