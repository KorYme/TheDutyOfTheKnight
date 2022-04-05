using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroAbility : MonoBehaviour
{
    public HeroStats heroStats;

    //Ability unlocked
    public bool windUnlocked;
    public bool fireUnlocked;
    public bool earthUnlocked;

    //Cooldowns states
    public bool windInCooldown;
    public bool fireInCooldown;
    public bool earthInCooldown;

    //Cooldowns times
    public float cooldownWind;
    public float cooldownFire;
    public float cooldownEarth;

    //KeyCodes
    public KeyCode KeyFire = KeyCode.A;
    public KeyCode KeyWind = KeyCode.Mouse1;
    public KeyCode KeyEarth = KeyCode.Space;
    public KeyCode KeyExplosion = KeyCode.Alpha1;
    public KeyCode KeyDash = KeyCode.Alpha2;
    public KeyCode KeyShieldDamage = KeyCode.Alpha3;

    public GameObject FireBall;
    public float fireBallSpeed;
    public bool damagingShield;
    private Collider2D[] tpZone;
    public float tailleRadius;
    public Vector3 cursorPosition;
    public LayerMask enemyLayer;
    public GameObject Explosion;


    private void Start()
    {
        //Initialization
        heroStats = GameObject.FindGameObjectWithTag("Player").GetComponent<HeroStats>();
        GameObject.FindGameObjectWithTag("Shield").GetComponent<SpriteRenderer>().enabled = false;
        GameObject.FindGameObjectWithTag("Shield").GetComponent<CircleCollider2D>().enabled = false;
        damagingShield = false;
        transform.Find("DashLane").GetComponent<TrailRenderer>().enabled = false;
    }

    private void FixedUpdate()
    {
        //Check if the player clicked on an ability key
        if (Input.GetKeyDown(KeyFire) && fireUnlocked && !fireInCooldown)
        {
            FireAbility();
            StartCoroutine(CooldownAbilityFire());
        }
        else if (Input.GetKey(KeyWind) && windUnlocked && !windInCooldown)
        {
            WindAbility();
            StartCoroutine(CooldownAbilityWind());
        }
        else if (Input.GetKey(KeyEarth) && earthUnlocked && !earthInCooldown)
        {
            EarthAbility();
            StartCoroutine(CooldownAbilityEarth());
        }
        else if (Input.GetKey(KeyExplosion) && earthUnlocked && windUnlocked && !earthInCooldown && !windInCooldown)
        {
            ExplosionAbility();
            StartCoroutine(CooldownAbilityEarth());
            StartCoroutine(CooldownAbilityWind());
        }
        else if (Input.GetKey(KeyDash) && fireUnlocked && windUnlocked && !fireInCooldown && !windInCooldown)
        {
            DashAbility();
            StartCoroutine(CooldownAbilityFire());
            StartCoroutine(CooldownAbilityWind());
        }
        else if (Input.GetKey(KeyShieldDamage) && fireUnlocked && earthUnlocked && !fireInCooldown && !earthInCooldown)
        {
            ShieldDamageAbility();
            StartCoroutine(CooldownAbilityFire());
            StartCoroutine(CooldownAbilityEarth());
        }
    }


    //Cooldowns' coroutines

    /// <summary>
    /// Create a cooldown for the wind ability
    /// </summary>
    /// <returns></returns>
    IEnumerator CooldownAbilityWind()
    {
        windInCooldown = true;
        yield return new WaitForSeconds(cooldownWind);
        windInCooldown = false;
    }

    /// <summary>
    /// Create a cooldown for the fire ability
    /// </summary>
    /// <returns></returns>
    IEnumerator CooldownAbilityFire()
    {
        fireInCooldown = true;
        yield return new WaitForSeconds(cooldownFire);
        fireInCooldown = false;
    }

    /// <summary>
    /// Create a cooldown for the earth ability
    /// </summary>
    /// <returns></returns>
    IEnumerator CooldownAbilityEarth()
    {
        earthInCooldown = true;
        yield return new WaitForSeconds(cooldownEarth + heroStats.shieldDuration);
        earthInCooldown = false;
    }


    //USP Abilities

    /// <summary>
    /// Create a fireball and launch it in the direction of the cursor
    /// </summary>
    void FireAbility()
    {
        Debug.Log("Fire");
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mouseDirection = new Vector2(mousePosition.x - transform.position.x, mousePosition.y - transform.position.y);

        GameObject bulletLaunch = Instantiate(FireBall, transform.position, new Quaternion(mouseDirection.x, mouseDirection.y,0,90)); 
        bulletLaunch.GetComponent<FireBall>().direction = mouseDirection.normalized;
        bulletLaunch.GetComponent<FireBall>().fireBallSpeed = fireBallSpeed;
    }

    /// <summary>
    /// Give the shield a color and launch the shield function
    /// </summary>
    void EarthAbility()
    {
        Debug.Log("Earth");
        GameObject.FindGameObjectWithTag("Shield").GetComponent<SpriteRenderer>().color = new Color(88, 255, 0);
        StartCoroutine(ShieldDuration());
    }

    /// <summary>
    /// Teleport the player on the mouse location if CanUTP is true
    /// </summary>
    void WindAbility()
    {
        if (CanUTP())
        {
            Debug.Log("Wind");
            transform.position = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x,Camera.main.ScreenToWorldPoint(Input.mousePosition).y,0);
        }
        else
        {
            windInCooldown = false;
        }
    }

    /// <summary>
    /// Teleport the player on the mouse location if CanUTP is true and create a damaging zone around the location of the player
    /// </summary>
    void ExplosionAbility()
    {
        if (CanUTP())
        {
            Debug.Log("Explosionnnnnnnnnn");
            transform.position = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 0);
            GameObject explosion = Instantiate(Explosion, transform.position, Quaternion.identity);
            Destroy(explosion, 1f);
            foreach (var item in Physics2D.OverlapCircleAll(transform.position, 1.2f, enemyLayer))
            {
                item.gameObject.SendMessage("TakeDamage",heroStats.explosionDamage);
            }
        }
        else
        {
            windInCooldown = false;
            earthInCooldown = false;
        }
    }

    /// <summary>
    /// Teleport the player on the mouse location if CanUTP is true and deal damage between the initial position and the new one
    /// </summary>
    void DashAbility()
    {
        if (CanUTP())
        {
            Debug.Log("Dash");
            StartCoroutine(TrailRenderer());
            cursorPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            foreach (var item in Physics2D.RaycastAll((Vector2)transform.position, new Vector2(cursorPosition.x - transform.position.x, cursorPosition.y - transform.position.y), Vector3.Distance(transform.position, cursorPosition), enemyLayer))
            {
                item.collider.gameObject.SendMessage("TakeDamage", heroStats.dashDamage);
            }
            transform.position = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 0);
        }
        else
        {
            windInCooldown = false;
            fireInCooldown = false;
        }
    }

    /// <summary>
    /// Give the shield a color, damages and launch the shield function
    /// </summary>
    void ShieldDamageAbility()
    {
        Debug.Log("Shield");
        damagingShield = true;
        GameObject.FindGameObjectWithTag("Shield").GetComponent<SpriteRenderer>().color = new Color(255,0,0);
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
        GameObject.FindGameObjectWithTag("Shield").GetComponent<SpriteRenderer>().enabled = true;
        GameObject.FindGameObjectWithTag("Shield").GetComponent<CircleCollider2D>().enabled = true;
        heroStats.invicibility = true;
        yield return new WaitForSeconds(heroStats.shieldDuration);
        GameObject.FindGameObjectWithTag("Shield").GetComponent<SpriteRenderer>().enabled = false;
        GameObject.FindGameObjectWithTag("Shield").GetComponent<CircleCollider2D>().enabled = false;
        heroStats.invicibility = false;
        damagingShield = false;
        GameObject.FindGameObjectWithTag("Shield").GetComponent<SpriteRenderer>().color = new Color(255,255,255);
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
        tpZone = Physics2D.OverlapCircleAll(Camera.main.ScreenToWorldPoint(Input.mousePosition), tailleRadius);
        foreach (Collider2D item in tpZone)
        {
            if (item.gameObject.layer == 6 || item.gameObject.layer == 8 || item.gameObject.layer == 9)
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
        transform.Find("DashLane").GetComponent<TrailRenderer>().enabled = true;
        yield return new WaitForSeconds(.15f);
        transform.Find("DashLane").GetComponent<TrailRenderer>().enabled = false;
    }
}