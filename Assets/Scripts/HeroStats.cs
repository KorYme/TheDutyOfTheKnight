using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroStats : MonoBehaviour
{
    [Header("Main Stats")]
    public int heroHP = 100;
    public int heroMaxHealth = 100;
    public int heroAttaque = 20;
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
    public int capHeroLow = 5;
    private bool heroLow;

    [Header("GameObjects and Components")]
    private HealthBar healthBar;
    public SpriteRenderer graphics;

    void Start()
    {
        heroHP = heroMaxHealth;
        healthBar = GameObject.FindGameObjectWithTag("HealthBar").GetComponent<HealthBar>();
        healthBar.InitializeHealthBar(heroMaxHealth);

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
            heroAttaque *= 2;
        }
        else if (heroHP > capHeroLow && heroLow)
        {
            heroLow = false;
            heroAttaque /= 2;
        }
        if (heroHP<=0)
        {
            //GameManager.instance.Die();
        }
    }

    private void TakeDamageHero(int damage)
    {
        if (!invicibility)
        {
            heroHP -= damage;
            healthBar.SetHealth(heroHP);
            Debug.Log("Le héro a désormais " + heroHP + " PV.");
            StartCoroutine(TheInvicibility());
        }
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
