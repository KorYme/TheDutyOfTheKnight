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
    public GameObject[] drops;

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
        DropItems(Random.Range(0,6));
        if (slider != null)
        {
            Destroy(slider.gameObject);
        }
    }

    protected virtual void DropItems(int nbItems)
    {
        if (drops.Length != 0)
        {
            for (int i = 0; i < nbItems; i++)
            {
                float x = transform.position.x + Random.Range(0f, 1.5f);
                float y = transform.position.y + Random.Range(0f, 1.5f);
                if (CanPopItem(new Vector3(x, y, 0), drops[0]))
                {
                    Instantiate(drops[0], new Vector3(x, y, 0), Quaternion.identity);
                }
                else
                {
                    i--;
                }
            }
        }
    }

    protected bool CanPopItem(Vector3 position, GameObject gameObject)
    {
        foreach (Collider2D item in Physics2D.OverlapCircleAll(position, gameObject.GetComponent<CircleCollider2D>().radius))
        {
            if (item.gameObject.layer == 6 || item.gameObject.layer == 8 || item.gameObject.layer == 9)
            {
                return false;
            }
        }
        return true;
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
