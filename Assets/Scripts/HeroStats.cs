using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroStats : MonoBehaviour
{
    public static HeroStats instance;
    private void Awake()
    {
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

    [Header("Abilities Stats")]
    public float fireDamage;
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
    private HealthBar healthBar;

    void Start()
    {
        heroHP = heroMaxHealth;
        healthBar = GameObject.FindGameObjectWithTag("HealthBar").GetComponent<HealthBar>();
        healthBar.InitializeHealthBar(heroMaxHealth, heroHP);

        invicibility = false;
        //Check if the nightmare mode
        if (nightmareMode)
        {
            heroMaxHealth = 1;
        }
        //Vérifie si le héros est low dès le début du jeu
        if (heroHP > capHeroLow)
        {
            heroLow = false;
        }
        GameManager.instance.InitGame();
    }

    void FixedUpdate()
    {
        //Totalement à revoir
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
        if (heroHP<=0)
        {
            GameManager.instance.Die();
        }
    }

    public void TakeDamageHero(float damage)
    {
        if (!invicibility)
        {
            heroHP -= damage;
            healthBar.SetHealth(heroHP);
            Debug.Log("Le héro a désormais " + heroHP + " PV.");
            StartCoroutine(TheInvicibility());
        }
    }

    public void HealHero(float heal)
    {
        heroHP = heroHP + heal > heroMaxHealth ? heroMaxHealth : heroHP + heal;
        healthBar.SetHealth(heroHP);
        Debug.Log("Le héro a désormais " + heroHP + " PV.");
    }

    public void IncreaseMaxHealthHero(float increase)
    {
        heroMaxHealth += increase;
        healthBar.InitializeHealthBar(heroMaxHealth, heroHP);
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
        while (invicibility)
        {
        graphics.color = new Color(1f, 1f, 1f, 0f);
        yield return new WaitForSeconds(flashDelay);
        graphics.color = new Color(1f, 1f, 1f, 1f);
        yield return new WaitForSeconds(flashDelay);
        }
    }
}
