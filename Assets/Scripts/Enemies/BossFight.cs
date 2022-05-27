using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BossFight : Enemies
{
    public GameObject reaperBullet;
    public GameObject reaperMinion;
    public InputData inputData;
    private Transform hitPoint1;
    private Transform hitPoint2;
    private float nbMinionsAlive;
    private SpriteRenderer shield;
    private GameObject healthBar;
    private CapsuleCollider2D bossCollider;
    private CircleCollider2D rangeCollider;
    private bool hasFightStarted;
    private bool isInRange;

    [Header("Boss Variables")]
    public float reaperFireBallSpeed;
    public float reaperMinionBallSpeed;
    public float reaperFireBallDamage;
    public float reaperMinionBallDamage;
    public float multiplierSpeedPhase2;
    public float multiplierDamagePhase2;
    public float timeBetweenAbility1;

    [Header ("Other Variables")]
    public bool bossAbility1;


    private float healthBossInitial;
    private bool firstInteraction;

    protected override void Start()
    {
        InitializeValues();
        DisableBossStart();
    }

    void InitializeValues()
    {
        base.Start();
        healthBossInitial = enemyHP;
        hitPoint1 = transform.Find("HitPoint1");
        hitPoint2 = transform.Find("HitPoint2");
        animator = GetComponent<Animator>();
        bossAbility1 = false;
        canMove = false;
        nbMinionsAlive = 0;
        shield = transform.Find("BossShield").GetComponent<SpriteRenderer>();
        shield.enabled = false;
        healthBar = transform.Find("Canvas").gameObject;
        bossCollider = GetComponent<CapsuleCollider2D>();
        rangeCollider = GetComponent<CircleCollider2D>();
        hasFightStarted = false;
        isInRange = false;
        firstInteraction = true;
    }

    void DisableBossStart()
    {
        bossCollider.enabled = false;
        rangeCollider.enabled = true;
        tag = "SleepingBoss";
    }

    public void Summoning()
    {
        RoomManager.instance.AreEnemiesIn(false);
        Destroy(transform.Find("SleepingBossHitBox").gameObject);
        rangeCollider.enabled = false;
        bossCollider.enabled = true;
        Interaction_Player.instance.ForceExit();
        animator.SetTrigger("FightStarted");
        hasFightStarted=true;
        tag = "Boss";
        healthBar.GetComponent<Animator>().SetTrigger("Appear");
    }

    void StartFight()
    {
        canMove = true;
        StartCoroutine(Ability1Manager());
    }

    protected override void FixedUpdate()
    {
        if (hasFightStarted)
        {
            base.FixedUpdate();
            if (bossAbility1 && canMove && !dead)
            {
                BossAbility1();
            }
            else if (canMove && !dead)
            {
                DirectionBoss();
                IsPlayerInRange();
            }
        }
    }

    private void Update()
    {
        if (!hasFightStarted && isInRange && !DialogueManager.instance.isMoving)
        {
            if (Input.GetKeyDown(inputData.interact))
            {
                DialogueManager.instance.currentPanelUser = gameObject;
                if (firstInteraction)
                {
                    if (!DialogueManager.instance.panelOpen)
                    {
                        DialogueManager.instance.PanelEnable();
                    }
                    DialogueManager.instance.UpdateTheScreen("???","A strange sphere is levitating in front of you, do you want to interact with it ?", 4);
                    firstInteraction = false;
                }
                else
                {
                    if (PlayerInventory.instance.nbKeyBoss >= 3)
                    {
                        Summoning();
                        PlayerInventory.instance.nbKeyBoss = 0;
                    }
                    else
                    {
                        DialogueManager.instance.UpdateTheScreen("???", "Nothing happened, maybe it's only the dungeon decoration after all.", 5);
                        firstInteraction = true;
                    }
                }
            }
            if (Input.GetKeyDown(inputData.close) && DialogueManager.instance.panelOpen)
            {
                DialogueManager.instance.PanelDisable();
                firstInteraction = true;
            }
        }
    }

    IEnumerator Ability1Manager()
    {
        yield return new WaitForSeconds(timeBetweenAbility1);
        bossAbility1 = true;
        StartCoroutine(Ability1Manager());
    }


    void DirectionBoss()
    {
        if (transform.position.x > player.transform.position.x)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
            slider.transform.eulerAngles = new Vector3(0, 0, 0);
        }
        else
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
            slider.transform.eulerAngles = new Vector3(0, 0, 0);
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
    }

    void HasAttacked()
    {
        animator.ResetTrigger("Attack");
        animator.ResetTrigger("Ability1");
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

    void BossAbility1()
    {
        animator.SetTrigger("Ability1");
        canMove = false;
        bossAbility1 = false;
    }

    void BossAbility2()
    {
        canMove = false;
        invulnerable = true;
        shield.enabled = true;
    }

    /// <summary>
    /// The boss launches 8 bullets which deals damage to the player
    /// The bullets spawn on the boss transform and go in a cardinal direction
    /// </summary>
    protected void LaunchBullets()
    {
        for (int i = -1; i < 2; i++)
        {
            for (int y = -1; y < 2; y++)
            {
                if (y != 0 || i != 0)
                {
                    GameObject bulletLaunch = Instantiate(reaperBullet, transform.position, Quaternion.identity);
                    bulletLaunch.GetComponent<ReaperBullet>().direction = new Vector2(i, y);
                }
            }
        }
    }

    protected void SpawnMinion()
    {
        for (int i = -1; i < 2; i+=2)
        {
            for (int y = -1; y < 2; y+=2)
            {
                Vector3 position = new Vector3(transform.position.x + i, transform.position.y + y, 0);
                GameObject bulletLaunch = Instantiate(reaperMinion, position, Quaternion.identity);
                bulletLaunch.GetComponent<ReaperMinion>().direction = new Vector2(i, y);
                nbMinionsAlive++;
            }
        }
    }

    public void AreMinionsStillAlive()
    {
        nbMinionsAlive--;
        if (nbMinionsAlive == 0 && invulnerable)
        {
            invulnerable = false;
            shield.enabled = false;
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
        reaperBullet.GetComponent<ReaperBullet>().animator.speed *= multiplierSpeedPhase2;
        reaperBullet.GetComponent<ReaperBullet>().enemySpeed *= multiplierSpeedPhase2;
        reaperBullet.GetComponent<ReaperBullet>().enemyDamage *= multiplierDamagePhase2;
    }

    protected override void IsDying()
    {
        animator.speed /= multiplierSpeedPhase2;
        dead = true;
        canMove = false;
        animator.SetTrigger("Death");
        HeroStats.instance.StopHero();
    }

    protected override void Die()
    {
        animator.ResetTrigger("Death");
        base.Die();
        RoomManager.instance.CheckEnemiesStillIn();
        GameManager.instance.Die();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            isInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            isInRange = false;
            firstInteraction = true;
            if (DialogueManager.instance.currentPanelUser == gameObject)
            {
                DialogueManager.instance.PanelDisable();
            }
        }
    }
}