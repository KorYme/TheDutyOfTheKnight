using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemies : MonoBehaviour
{
    //Useful access to another Class
    protected RoomManager roomManager;


    [Header("Enemy Variables")]
    public float enemyHP = 10f;
    public float enemyDamage = 0f;
    public float enemySpeed = 0f;
    public SpriteRenderer sprite;
    public bool isTouchDamage = true;
    protected bool dead = false;

    protected virtual void Start()
    {
        roomManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<RoomManager>();
    }

    protected virtual void FixedUpdate()
    {
        DamagingHero();
    }

    void DamagingHero()
    {
        if (isTouchDamage && this.GetComponent<Collider2D>().IsTouching(GameObject.FindGameObjectWithTag("Player").GetComponent<Collider2D>()))
        {
            GameObject.FindGameObjectWithTag("Player").SendMessage("TakeDamageHero", enemyDamage);
        }
        if (enemyHP <= 0 && !dead)
        {
            IsDying();
        }
    }
    protected virtual void TakeDamage(float damage)
    {
        enemyHP -= damage;
        StartCoroutine(Stagger());
    }

    protected IEnumerator Stagger()
    {
        sprite.color = new Color(255, 0, 0);
        yield return new WaitForSeconds(0.1f);
        sprite.color = new Color(255, 255, 255);
    }

    protected virtual void IsDying()
    {
        GetComponent<Collider2D>().enabled = false;
        roomManager.CheckEnemiesStillIn();
    }

    protected virtual void Die()
    {
        Destroy(gameObject);
    }
}
