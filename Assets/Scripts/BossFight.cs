using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFight : Enemies
{
    GameObject player;

    public LayerMask playerLayer;
    Transform hitPoint1;
    Transform hitPoint2;
    Rigidbody2D rb;
    private Animator animator;

    [Header ("Other Variables")]
    public bool canChange = true;
    public bool invulnerable = false;
    private bool bossAbility1;
    private float healthBossInitial;

    protected override void Start()
    {
        healthBossInitial = enemyHP;
        player = GameObject.FindGameObjectWithTag("Player");
        hitPoint1 = this.transform.Find("HitPoint1");
        hitPoint2 = this.transform.Find("HitPoint2");
        animator = this.GetComponent<Animator>();
        bossAbility1 = false;
        rb = GetComponent<Rigidbody2D>();
        roomManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<RoomManager>();
    }

    private void Update()
    {
        if (canChange)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, enemySpeed * Time.fixedDeltaTime);
        }
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if (bossAbility1)
        {
            canChange = false;
            bossAbility1 = false;
        }
        else if (canChange)
        {
            DirectionBoss();
            IsPlayerInRange();
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
        transform.position = Vector3.MoveTowards(transform.position, player.transform.position, enemySpeed * Time.fixedDeltaTime);
    }

    void IsPlayerInRange()
    {
        if (Physics2D.OverlapCircle(hitPoint1.position, 1.5f, playerLayer) != null)
        {
            canChange = false;
            animator.SetTrigger("Attack");
        }
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
            HeroStats.instance.TakeDamageHero(enemyDamage);
    }

    void AttackBoss2()
    {

        if (Physics2D.OverlapCircle(hitPoint2.position, 1.15f, playerLayer) != null)
            HeroStats.instance.TakeDamageHero(enemyDamage);   
    }

    void BossAbility()
    {
        //Arrêter le mouvement
        //Instantiate little fireball
    }

    protected override void TakeDamage(float damage)
    {
        if (invulnerable)
            return;
        base.TakeDamage(damage);
        Debug.Log("Le boss a encore " + enemyHP.ToString());

        if (healthBossInitial/2 >= enemyHP)
            animator.SetBool("Phase2", true);

        if (enemyHP <= 0)
            IsDying();
    }


    protected override void IsDying()
    {
        canChange = false;
        animator.SetTrigger("Death");
        base.IsDying();
    }

    protected override void Die()
    {
        animator.ResetTrigger("Death");
        base.Die();
        roomManager.CheckEnemiesStillIn();
    }
}
