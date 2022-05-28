using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroStats : MonoBehaviour
{
    public static HeroStats instance;
    private void Awake()
    {
        Time.timeScale = 1f;
        instance = null;
        if (instance != null)
        {
            Debug.LogError("More than one heroStats instance in the game !");
        }

        instance = this;
    }

    [Header("Main Stats")]
    public float heroHP = 100;
    public float heroMaxHealth = 100f;
    public float heroAttack = 20f;
    public float speed = 250f;
    public float flashDelay;
    public float invicibilityDelay;
    public bool invicibility;
    public bool isDead;

    [Header("Abilities Stats")]
    public float fireDamage = 25f;
    public float shieldDamage = 25f;
    public float shieldDuration = 3f;
    public float dashDamage = 50f;
    public float explosionDamage = 100f;
    
    [Header("Gamemode Parameters")]
    public bool nightmareMode;
    public float capHeroLow = 5;
    private bool heroLow;

    [Header("GameObjects and Components")]
    public SpriteRenderer graphics;
    public Animator animator;
    public HealthBar healthBar;

    private HeroMovement heroMovement;
    private HeroAbility heroAbility;

    void Start()
    {
        heroMovement = HeroMovement.instance;
        heroAbility = HeroAbility.instance;

        heroHP = heroMaxHealth;
        healthBar.InitializeHealthBar(heroMaxHealth, heroHP);

        invicibility = false;
        isDead = false;
    }

    void CheckStateHero()
    {
        if (heroHP <= capHeroLow && !(heroLow))
        {
            heroLow = true;
            heroAttack *= 2;
        }
        else if (heroHP > capHeroLow && heroLow)
        {
            heroLow = false;
            heroAttack /= 2;
        }
        if (heroHP <= 0)
        {
            GameManager.instance.victory = false;
            IsDying();
        }
    }

    public void IsDying()
    {
        isDead = true;
        StopHero();
        graphics.sortingOrder = 51;
        animator.SetTrigger("Death");
    }
    
    public void StopHero()
    {
        heroMovement.rb.bodyType = RigidbodyType2D.Kinematic;
        heroMovement.rb.velocity = Vector2.zero;
        heroMovement.enabled = false;
        HeroHits.instance.enabled = false;
        HeroAbility.instance.enabled = false;
        invicibility = true;
        if (!isDead)
        {
            animator.speed = 0;
        }
    }

    public void Death()
    {
        animator.speed = 0;
        GameManager.instance.Die();
    }

    public void TakeDamageHero(float damage)
    {
        if (!invicibility)
        {
            heroHP -= damage;
            healthBar.SetHealth(heroHP);
            CheckStateHero();
            if (!isDead && damage != 0)
            {
                StartCoroutine(TheInvicibility());
            }
        }
    }

    public void AddStatsHero(TotemsData totemsData)
    {
        fireDamage += totemsData.fireDamageBonus;
        shieldDamage += totemsData.fireDamageBonus;
        shieldDuration += totemsData.earthDurationBonus;
        dashDamage += totemsData.fireDamageBonus;
        explosionDamage += totemsData.explosionDamageBonus;
        heroAbility.UpgradeCD(totemsData);
    }


    public void HealHero(float heal)
    {
        heroHP = heroHP + heal > heroMaxHealth ? heroMaxHealth : heroHP + heal;
        healthBar.SetHealth(heroHP);
        CheckStateHero();
    }

    public void IncreaseMaxHealthHero(float increase)
    {
        heroMaxHealth += increase;
        healthBar.InitializeHealthBar(heroMaxHealth, heroHP);
        CheckStateHero();
    }

    IEnumerator TheInvicibility()
    {
        invicibility = true;
        StartCoroutine(FlashInvicibility());
        yield return new WaitForSeconds(invicibilityDelay);
        invicibility = false;
    }

    IEnumerator FlashInvicibility()
    {
        while (invicibility && !isDead && GameManager.instance.gameLaunched)
        {
        graphics.color = new Color(1f, 1f, 1f, 0f);
        yield return new WaitForSeconds(flashDelay);
        graphics.color = new Color(1f, 1f, 1f, 1f);
        yield return new WaitForSeconds(flashDelay);
        }
    }
}
