using System.Collections;
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
    public GameObject FireBall;
    public float fireBallSpeed;
    public bool damagingShield;
    private Collider2D[] tpZone;
    public float radiusCharacter;
    public Vector3 cursorPosition;
    public LayerMask enemyLayer;
    public LayerMask cantTPLayer;
    public GameObject Explosion;

    private CoolDownManager coolDownManager;
    private SpriteRenderer spriteShield;
    private CircleCollider2D colliderShield;
    private TrailRenderer dashTrail;

    private void Start()
    {
        //Initializations
        coolDownManager = CoolDownManager.instance;
        spriteShield = transform.Find("Shield").GetComponent<SpriteRenderer>();
        colliderShield = transform.Find("Shield").GetComponent<CircleCollider2D>();
        dashTrail = transform.Find("DashLane").GetComponent<TrailRenderer>();
        spriteShield.enabled = false;
        colliderShield.enabled = false;
        damagingShield = false;
        dashTrail.enabled = false;
    }

    private void Update()
    {
        if (LevelManager.instance.pauseMenu || PlayerInventory.instance.miniMapOpen)
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
        else if (Input.GetKey(inputData.abilityEarth) && earthUnlocked && !earthInCooldown)
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
        else if (Input.GetKey(inputData.abilityDamagingShield) && fireUnlocked && earthUnlocked && !fireInCooldown && !earthInCooldown)
        {
            ShieldDamageAbility();
        }
    }

    //USP Abilities

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
        GameObject bulletLaunch = Instantiate(FireBall, transform.position, Quaternion.identity);
        bulletLaunch.GetComponent<FireBall>().SetDirection(mouseDirection.normalized);
        bulletLaunch.GetComponent<FireBall>().fireBallSpeed = fireBallSpeed;
    }

    /// <summary>
    /// Give the shield a color and launch the shield function
    /// </summary>
    void EarthAbility()
    {
        earthInCooldown = true;
        coolDownManager.ResetCoolDown("Earth");
        coolDownManager.DisplayRefreshKeyButton();
        spriteShield.color = new Color(88, 255, 0);
        StartCoroutine(ShieldDuration());
    }

    /// <summary>
    /// Teleport the player on the mouse location if CanUTP is true
    /// </summary>
    void WindAbility()
    {
        if (CanUTP())
        {
            windInCooldown = true;
            coolDownManager.ResetCoolDown("Wind");
            coolDownManager.DisplayRefreshKeyButton();
            transform.position = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x,Camera.main.ScreenToWorldPoint(Input.mousePosition).y,0);
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
            windInCooldown = true;
            coolDownManager.ResetCoolDown("Wind");
            coolDownManager.DisplayRefreshKeyButton();
            transform.position = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 0);
            GameObject explosion = Instantiate(Explosion, transform.position, Quaternion.identity);
            Destroy(explosion, 1f);
            foreach (var item in Physics2D.OverlapCircleAll(transform.position, 1.2f, enemyLayer))
            {
                item.gameObject.SendMessage("TakeDamage",heroStats.explosionDamage);
            }
        }
    }

    /// <summary>
    /// Teleport the player on the mouse location if CanUTP is true and deal damage between the initial position and the new one
    /// </summary>
    void DashAbility()
    {
        if (CanUTP())
        {
            fireInCooldown = true;
            coolDownManager.ResetCoolDown("Fire");
            windInCooldown = true;
            coolDownManager.ResetCoolDown("Wind");
            coolDownManager.DisplayRefreshKeyButton();
            StartCoroutine(TrailRenderer());
            cursorPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            foreach (var item in Physics2D.RaycastAll((Vector2)transform.position, new Vector2(cursorPosition.x - transform.position.x, cursorPosition.y - transform.position.y), Vector3.Distance(transform.position, cursorPosition), enemyLayer))
            {
                item.collider.gameObject.SendMessage("TakeDamage", heroStats.dashDamage);
            }
            transform.position = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 0);
        }
    }

    /// <summary>
    /// Give the shield a color, damages and launch the shield function
    /// </summary>
    void ShieldDamageAbility()
    {
        fireInCooldown = true;
        coolDownManager.ResetCoolDown("Fire");
        earthInCooldown = true;
        coolDownManager.ResetCoolDown("Earth");
        coolDownManager.DisplayRefreshKeyButton();
        damagingShield = true;
        spriteShield.color = new Color(255,0,0);
        StartCoroutine(ShieldDuration());
    }
    
    /// <summary>
    /// Activate the shield's sprite, its collider and make the player inivicible.
    /// Go back to normal after {shieldDuration} time.
    /// Also desactivate the damage of the shield
    /// </summary>
    /// <returns></returns>
    IEnumerator ShieldDuration()
    {
        spriteShield.enabled = true;
        colliderShield.enabled = true;
        heroStats.invicibility = true;
        yield return new WaitForSeconds(heroStats.shieldDuration);
        spriteShield.enabled = false;
        colliderShield.enabled = false;
        heroStats.invicibility = false;
        damagingShield = false;
        spriteShield.color = new Color(255,255,255);
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

    /// <summary>
    /// Create a line on the way of the dash
    /// </summary>
    IEnumerator TrailRenderer()
    {
        dashTrail.enabled = true;
        yield return new WaitForSeconds(.15f);
        dashTrail.enabled = false;
    }


    /// <summary>
    /// Update the cooldowns aftter an upgrade
    /// </summary>
    public void UpgradeCD(TotemsData totemsData)
    {
        cooldownEarth -= (cooldownEarth - totemsData.earthCooldownBonus > 0 ? totemsData.earthCooldownBonus : cooldownEarth);
        cooldownWind -= (cooldownWind - totemsData.windCooldownBonus > 0 ? totemsData.windCooldownBonus : cooldownWind);
        cooldownFire -= (cooldownFire - totemsData.fireCooldownBonus > 0 ? totemsData.fireCooldownBonus : cooldownFire);
    }
}
