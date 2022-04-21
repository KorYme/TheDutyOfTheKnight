using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFight : Enemies
{
    public LayerMask playerLayer;
    public GameObject reaperBullet;
    public GameObject reaperMinion;
    Transform hitPoint1;
    Transform hitPoint2;

    [Header("Boss Variables")]
    public float reaperFireBallSpeed;
    public float reaperMinionBallSpeed;
    public float reaperFireBallDamage;
    public float reaperMinionBallDamage;
    public float multiplierSpeedPhase2;
    public float multiplierDamagePhase2;

    [Header ("Other Variables")]
    public bool canMove;
    private bool bossAbility1;
    private float healthBossInitial;

    protected override void Start()
    {
        base.Start();
        healthBossInitial = enemyHP;
        hitPoint1 = transform.Find("HitPoint1");
        hitPoint2 = transform.Find("HitPoint2");
        animator = GetComponent<Animator>();
        bossAbility1 = false;
        canMove = false;
        StartCoroutine(PhaseManager());
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if (bossAbility1 && canMove)
        {
            BossAbility1();
        }
        else if (canMove)
        {
            DirectionBoss();
            IsPlayerInRange();
        }
    }

    IEnumerator PhaseManager()
    {
        yield return new WaitForSeconds(10f);
        bossAbility1 = true;
        StartCoroutine(PhaseManager());
    }

    void StartFight()
    {
        canMove = true;
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
            canMove = false;
            animator.SetTrigger("Attack");
        }
    }

    void IdleAnimation()
    {
        canMove = true;
        invulnerable = false;
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

    /// <summary>
    /// The boss launches 9 bullets which deals damage to the player
    /// The bullet spawn on the boss transform and go in a cardinal direction
    /// </summary>
    void BossAbility1()
    {
        canMove = false;
        bossAbility1 = false;
        //Launch Animation
        for (int i = -1; i < 2; i++)
        {
            for (int y = -1; y < 2; y++)
            {
                if (y != 0 || i != 0)
                {
                    GameObject bulletLaunch = Instantiate(reaperBullet, transform.position, Quaternion.identity);
                    bulletLaunch.GetComponent<ReaperBullet>().direction = new Vector2(i,y);
                }
            }
        }
    }

    protected override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        if (healthBossInitial/2 >= enemyHP && !dead)
            animator.SetBool("Phase2", true);
    }

    protected void Phase2()
    {
        canMove = false;
        invulnerable = true;
        enemySpeed *= multiplierSpeedPhase2;
        animator.speed *= multiplierSpeedPhase2;
        enemyDamage *= multiplierDamagePhase2;
    }

    protected override void IsDying()
    {
        animator.speed /= multiplierSpeedPhase2;
        dead = true;
        canMove = false;
        animator.SetTrigger("Death");
    }

    protected override void Die()
    {
        animator.ResetTrigger("Death");
        base.Die();
        roomManager.CheckEnemiesStillIn();
    }
}
