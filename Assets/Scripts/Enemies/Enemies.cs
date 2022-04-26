using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemies : MonoBehaviour
{
    //Useful access to others classes and objects
    protected RoomManager roomManager;
    protected GameObject player;


    [Header("Enemy Variables")]
    public float enemyHP = 10f;
    public float enemyDamage = 0f;
    public float enemySpeed = 0f;
    public SpriteRenderer sprite;
    public Animator animator;
    public bool invulnerable = false;
    public bool isTouchDamage = true;
    public bool dead = false;
    public Slider slider;

    protected virtual void Start()
    {
        roomManager = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<RoomManager>();
        player = GameObject.FindGameObjectWithTag("Player");
        if (slider != null)
        {
            slider.minValue = 0;
            slider.maxValue = enemyHP;
            slider.value = enemyHP;
        }
    }

    protected virtual void FixedUpdate()
    {
        DamagingHero();
        if (HeroStats.instance.isDead && animator.speed != 0)
        {
            StopPlaying();
        }
    }

    protected virtual void DamagingHero()
    {
        if (isTouchDamage && GetComponent<Collider2D>().IsTouching(player.GetComponent<Collider2D>()))
        {
            player.SendMessage("TakeDamageHero", enemyDamage);
        }
    }

    protected virtual void TakeDamage(float damage)
    {
        if (dead)
            return;
        if (!invulnerable)
        {
            GameManager.instance.score += damage;
            enemyHP -= damage;
            StartCoroutine(Stagger());
            if (slider != null)
            {
                slider.value = enemyHP;
            }
        }
        if (enemyHP <= 0)
        {
            IsDying();
        }
    }

    protected virtual void ToggleInvulnerability()
    {
        invulnerable = !invulnerable;
    }

    protected virtual IEnumerator Stagger()
    {
        sprite.color = new Color(255, 0, 0);
        yield return new WaitForSeconds(0.1f);
        sprite.color = new Color(255, 255, 255);
    }

    protected virtual void IsDying()
    {
        dead = true;
        enemySpeed = 0;
        GetComponent<Collider2D>().enabled = false;
        roomManager.CheckEnemiesStillIn();
        if (slider != null)
        {
            Destroy(slider.gameObject);
        }
    }

    protected virtual void StopPlaying()
    {
        if (transform.Find("Canvas") != null)
        {
            Destroy(transform.Find("Canvas").gameObject);
        }
        if (GetComponent<Rigidbody2D>() != null)
        {
            GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }
        animator.speed = 0;
        enemySpeed = 0;
    }

    protected virtual void Die()
    {
        Destroy(gameObject);
    }
}
