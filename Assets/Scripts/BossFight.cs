using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFight : MonoBehaviour
{
    GameObject player;

    public LayerMask playerLayer;
    Transform hitPoint1;
    Transform hitPoint2;
    Rigidbody2D rb;

    public float damageHitBoss;
    public float healthBoss;
    public bool canChange = true;
    public bool invulnerable = false;
    public float bossSpeed;
    bool bossAbility1;
    float healthBossInitial;
    private Animator animator;

    private void Start()
    {
        healthBossInitial = healthBoss;
        player = GameObject.FindGameObjectWithTag("Player");
        hitPoint1 = this.transform.Find("HitPoint1");
        hitPoint2 = this.transform.Find("HitPoint2");
        animator = this.GetComponent<Animator>();
        bossAbility1 = false;
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (canChange)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, bossSpeed * Time.fixedDeltaTime);
        }
    }

    private void FixedUpdate()
    {
        if (bossAbility1)
        {
            canChange = false;
            bossAbility1 = false;
        }
        else if (canChange)
        {
            DirectionBoss();
            if (Physics2D.OverlapCircle(hitPoint1.position, 1.5f, playerLayer) != null)
            {
                canChange = false;
                animator.SetTrigger("Attack");
            }
        }
    }

    IEnumerator PhaseManager()
    {
        bossAbility1 = true;
        yield return new WaitForSeconds(10f);
        PhaseManager();
    }

    void DirectionBoss()
    {
        if (this.transform.position.x > player.transform.position.x)
        {
            this.transform.eulerAngles = new Vector3(0, 180, 0);
        }
        else
        {
            this.transform.eulerAngles = new Vector3(0, 0, 0);
        }
        transform.position = Vector3.MoveTowards(transform.position, player.transform.position, bossSpeed * Time.fixedDeltaTime);
    }

    void IdleAnimation()
    {
        canChange = true;
    }

    void HasAttacked()
    {
        animator.ResetTrigger("Attack");
    }

    void AttackBoss1()
    { 
        
        if (Physics2D.OverlapCircle(hitPoint1.position, 1.6f, playerLayer) != null)
            HeroStats.instance.TakeDamageHero(damageHitBoss);
    }

    void AttackBoss2()
    {

        if (Physics2D.OverlapCircle(hitPoint2.position, 1.15f, playerLayer) != null)
            HeroStats.instance.TakeDamageHero(damageHitBoss);   
    }

    void BossAbility()
    {
        //Arrêter le mouvement
        //Instantiate little fireball
    }

    void TakeDamage(float damage)
    {
        if (invulnerable)
            return;
        healthBoss -= damage;
        Debug.Log("Le boss a encore " + healthBoss.ToString());

        if (healthBossInitial/2 >= healthBoss)
            animator.SetBool("Phase2", true);

        if (healthBoss <= 0)
            IsDying();
    }


    void IsDying()
    {
        canChange = false;
        animator.SetTrigger("Death");
    }

    void Die()
    {
        animator.ResetTrigger("Death");
        Destroy(gameObject);
    }
}
