using System.Collections;
using UnityEngine;

/// <summary>
/// Script managing all the statistics of the player
/// </summary>
public class HeroStats : MonoBehaviour
{
    public static HeroStats instance;
    //Singleton Initialization
    private void Awake()
    {
        Time.timeScale = 1f;
        if (instance != null)
        {
            Debug.LogError("More than one heroStats instance in the game !");
            return;
        }
        instance = this;
    }

    [Header("Main Stats")]
    public float heroHP = 100;
    public float heroMaxHealth = 100f;
    public float heroAttack = 20f;
    public float speed = 250f;
    [SerializeField] private float flashDelay;
    [SerializeField] private float invincibilityDelay;

    [Header("Abilities Stats")]
    public float fireDamage = 25f;
    public float shieldDamage = 25f;
    public float shieldDuration = 3f;
    public float dashDamage = 50f;
    public float explosionDamage = 100f;

    [Header("GameObjects and Components")]
    [SerializeField] private SpriteRenderer graphics;
    [SerializeField] private Animator animator;
    [SerializeField] private HealthBar healthBar;

    private HeroMovement heroMovement;
    private HeroAbility heroAbility;
    [HideInInspector] public bool invincibility;
    [HideInInspector] public bool isDead;

    void Start()
    {
        heroMovement = HeroMovement.instance;
        heroAbility = HeroAbility.instance;

        heroHP = heroMaxHealth;
        healthBar.InitializeHealthBar(heroMaxHealth, heroHP);

        invincibility = false;
        isDead = false;
    }

    /// <summary>
    /// Check if the player is dead after being damaged
    /// </summary>
    void CheckStateHero()
    {
        if (heroHP <= 0)
        {
            GameManager.instance.victory = false;
            IsDying();
        }
    }

    /// <summary>
    /// Play the dying animation of the player -
    /// Called when the player has just died
    /// </summary>
    public void IsDying()
    {
        isDead = true;
        StopHero();
        graphics.sortingOrder = 51;
        animator.SetTrigger("Death");
    }
    
    /// <summary>
    /// Stop the hero's movement
    /// </summary>
    public void StopHero()
    {
        heroMovement.rb.bodyType = RigidbodyType2D.Kinematic;
        heroMovement.rb.velocity = Vector2.zero;
        heroMovement.canPlayerMove = false;
        invincibility = true;
        if (!isDead)
        {
            animator.speed = 0;
        }
    }

    /// <summary>
    /// Display the lose panel and play the death's clips
    /// </summary>
    public void Death()
    {
        animator.speed = 0;
        GameManager.instance.Die();
        AudioManager.instance.PlayClip("BellDeath");
        AudioManager.instance.PlayClip("DeathTheme");
    }

    /// <summary>
    /// Deal damage to the player if he is not invincible
    /// </summary>
    /// <param name="damage">Number of damage to deal</param>
    public void TakeDamageHero(float damage)
    {
        if (!invincibility)
        {
            AudioManager.instance.PlayClip("Damage");
            heroHP -= damage;
            healthBar.SetHealth(heroHP);
            CheckStateHero();
            if (!isDead && damage != 0)
            {
                StartCoroutine(TheInvicibility());
            }
        }
    }

    /// <summary>
    /// Upgrade the hero's stats with the totem stats he prayed for
    /// </summary>
    /// <param name="totemsData">Data of all totems</param>
    public void AddStatsHero(TotemsData totemsData)
    {
        AudioManager.instance.PlayClip("Benediction");
        fireDamage += totemsData.fireDamageBonus;
        shieldDamage += totemsData.fireDamageBonus;
        shieldDuration += totemsData.earthDurationBonus;
        dashDamage += totemsData.fireDamageBonus;
        explosionDamage += totemsData.explosionDamageBonus;
        heroAbility.UpgradeCD(totemsData);
    }

    /// <summary>
    /// Heal the hero
    /// </summary>
    /// <param name="heal">Number of damage to heal</param>
    public void HealHero(float heal)
    {
        heroHP = heroHP + heal > heroMaxHealth ? heroMaxHealth : heroHP + heal;
        healthBar.SetHealth(heroHP);
        CheckStateHero();
    }

    /// <summary>
    /// Increase the maximum health of the hero
    /// </summary>
    /// <param name="increase">Number of health point to add to the max health</param>
    public void IncreaseMaxHealthHero(float increase)
    {
        heroMaxHealth += increase;
        healthBar.InitializeHealthBar(heroMaxHealth, heroHP);
        CheckStateHero();
    }

    /// <summary>
    /// Timing of the hero's invicibility 
    /// </summary>
    /// <returns>Time of the invicibility</returns>
    IEnumerator TheInvicibility()
    {
        invincibility = true;
        StartCoroutine(FlashInvicibility());
        yield return new WaitForSeconds(invincibilityDelay);
        invincibility = false;
    }

    /// <summary>
    /// Create a feedback flash while the player is invincible
    /// </summary>
    /// <returns>Time between each flash of the invicibility</returns>
    IEnumerator FlashInvicibility()
    {
        while (invincibility && !isDead && HeroMovement.instance.canPlayerMove)
        {
        graphics.color = new Color(1f, 1f, 1f, 0f);
        yield return new WaitForSeconds(flashDelay);
        graphics.color = new Color(1f, 1f, 1f, 1f);
        yield return new WaitForSeconds(flashDelay);
        }
    }

    /// <summary>
}
