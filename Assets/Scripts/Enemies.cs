using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    protected virtual void Start()
    {
        roomManager = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<RoomManager>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    protected virtual void FixedUpdate()
    {
        DamagingHero();
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
    }

    protected virtual void Die()
    {
        Destroy(gameObject);
    }
}
