using System.Collections;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// Parent class of every enemy class
/// </summary>
public class Enemies : MonoBehaviour
{
    protected GameObject player;
    [HideInInspector] public bool dead = false;

    [Header("Enemy Variables")]
    public Animator animator;
    public SpriteRenderer sprite;
    public Slider slider;
    public GameObject[] drops;
    public LayerMask playerLayer;
    public float enemyHP = 10f;
    public float enemyDamage = 0f;
    public float enemySpeed = 0f;
    public bool invulnerable = false;
    public bool isTouchDamage = true;
    public bool canMove;
    
    protected virtual void Start()
    {
        player = HeroStats.instance.gameObject;
        if (slider != null)
        {
            slider.minValue = 0;
            slider.maxValue = enemyHP;
            slider.value = enemyHP;
        }
        canMove = true;
    }

    protected virtual void FixedUpdate()
    {
        DamagingHero();
        if (HeroStats.instance.isDead && animator.speed != 0)
        {
            StopPlaying();
        }
    }

    /// <summary>
    /// Check if the player has touched this enemy and if the enemy's hitbox deal damage if it's true, damage the player
    /// </summary>
    protected virtual void DamagingHero()
    {
        if (isTouchDamage && GetComponent<Collider2D>().IsTouching(player.GetComponent<Collider2D>()) && canMove)
        {
            player.SendMessage("TakeDamageHero", enemyDamage);
        }
    }

    /// <summary>
    /// Deal damage to this enemy
    /// </summary>
    /// <param name="damage">Number of damage</param>
    public virtual void TakeDamage(float damage)
    {
        if (dead)
            return;
        else if (!invulnerable)
        {
            AudioManager.instance.PlayClip("DamageEnemy");
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
    
    /// <summary>
    /// Allow this enemy to move and to be animated
    /// </summary>
    public virtual void StartMoving()
    {
        canMove = true;
        animator.speed = 1;
    }

    /// <summary>
    /// Stop the movement of this enemy and the animation too
    /// </summary>
    public virtual void StopMoving()
    {
        canMove = false;
        animator.speed = 0;
    }

    /// <summary>
    /// Toggle this enemy invulnerability
    /// </summary>
    protected virtual void ToggleInvulnerability()
    {
        invulnerable = !invulnerable;
    }

    /// <summary>
    /// Create a feedback when the enemy is touched
    /// </summary>
    /// <returns>Time of the stagger animation</returns>
    protected virtual IEnumerator Stagger()
    {
        sprite.color = new Color(255, 0, 0);
        yield return new WaitForSeconds(0.1f);
        sprite.color = new Color(255, 255, 255);
    }

    /// <summary>
    /// Play the dying animation and make the enemy harmless
    /// </summary>
    protected virtual void IsDying()
    {
        dead = true;
        enemySpeed = 0;
        animator.speed = 1;
        GetComponent<Collider2D>().enabled = false;
        RoomManager.instance.CheckEnemiesStillIn();
        DropItems(Random.Range(0, 6));
        if (slider != null)
        {
            Destroy(slider.gameObject);
        }
    }

    /// <summary>
    /// Drop items on the position of the enemy
    /// </summary>
    /// <param name="nbItems">Number of drops to spawn</param>
    protected virtual void DropItems(int nbItems)
    {
        if (drops.Length != 0)
        {
            for (int i = 0; i < nbItems; i++)
            {
                float x = transform.position.x + Random.Range(-0.75f, 0.75f);
                float y = transform.position.y + Random.Range(-0.75f, 0.75f);
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

    /// <summary>
    /// Check if an item can spawn
    /// </summary>
    /// <param name="position">The position where the item is gonna spawn</param>
    /// <param name="gameObject">The item to instantiate</param>
    /// <returns></returns>
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

    /// <summary>
    /// Disable all the enemy behaviour
    /// Called when the player dies or kills the boss
    /// </summary>
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

    /// <summary>
    /// Destroy the enemy -
    /// Called at the end of the dying animation of this enemy
    /// </summary>
    protected virtual void Die()
    {
        Destroy(gameObject);
    }
}
