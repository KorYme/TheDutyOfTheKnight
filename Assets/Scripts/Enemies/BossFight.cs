using System.Collections;
using UnityEngine;

/// <summary>
/// Script of the boss -
/// Inherit from the Enemies class
/// </summary>
public class BossFight : Enemies
{
    private DialogueManager dialogueManager;
    private GameObject healthBar;
    private Transform hitPoint1;
    private Transform hitPoint2;
    private SpriteRenderer shieldSprite;
    private CapsuleCollider2D bossCollider;
    private CircleCollider2D rangeCollider;
    private bool hasFightStarted;
    private bool isInRange;
    private bool bossAbility1;
    private bool firstInteraction;
    private bool secondInteraction;
    private float healthBossInitial;
    private float nbMinionsAlive;

    [SerializeField] private InputData inputData;

    [Header("To Spawn GameObject")]
    [SerializeField] private GameObject reaperBullet;
    [SerializeField] private GameObject reaperMinion;


    [Header("Boss Variables")]
    public float reaperFireBallSpeed;
    public float reaperMinionBallSpeed;
    public float reaperFireBallDamage;
    public float reaperMinionBallDamage;
    [SerializeField] private float multiplierSpeedPhase2;
    [SerializeField] private float multiplierDamagePhase2;
    [SerializeField] private float timeBetweenAbility1;

    protected override void Start()
    {
        InitializeValues();
        DisableBossStart();
    }

    /// <summary>
    /// Initialize all the values
    /// </summary>
    void InitializeValues()
    {
        base.Start();
        rangeCollider = GetComponent<CircleCollider2D>();
        bossCollider = GetComponent<CapsuleCollider2D>();
        shieldSprite = transform.Find("BossShield").GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        dialogueManager = DialogueManager.instance;
        healthBar = transform.Find("Canvas").gameObject;
        hitPoint1 = transform.Find("HitPoint1");
        hitPoint2 = transform.Find("HitPoint2");
        bossAbility1 = false;
        canMove = false;
        shieldSprite.enabled = false;
        hasFightStarted = false;
        isInRange = false;
        firstInteraction = true;
        secondInteraction = true;
        invulnerable = true;
        healthBossInitial = enemyHP;
        nbMinionsAlive = 0;
    }

    /// <summary>
    /// Make the boss inactive
    /// </summary>
    void DisableBossStart()
    {
        bossCollider.enabled = false;
        rangeCollider.enabled = true;
        tag = "SleepingBoss";
    }

    /// <summary>
    /// Play the summon animation and close the door
    /// </summary>
    public void Summoning()
    {
        Destroy(transform.Find("SleepingBossHitBox").gameObject);
        RoomManager.instance.AreEnemiesIn(false);
        Interaction_Player.instance.ForceExit();
        healthBar.GetComponent<Animator>().SetTrigger("Appear");
        animator.SetTrigger("FightStarted");
        rangeCollider.enabled = false;
        bossCollider.enabled = true;
        hasFightStarted=true;
        tag = "Boss";
        AudioManager.instance.PlayClip("BossTheme");
        StartCoroutine(RoomManager.instance.BossLights());
    }

    /// <summary>
    /// Enable the the boss for the fight
    /// </summary>
    void StartFight()
    {
        canMove = true;
        StartCoroutine(Ability1Manager());
        invulnerable = false;
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
        if (!hasFightStarted && isInRange && !dialogueManager.isMoving)
        {
            if (Input.GetKeyDown(inputData.interact))
            {
                AudioManager.instance.PlayClip("Confirm");
                dialogueManager.currentPanelUser = gameObject;
                if (firstInteraction)
                {
                    if (!dialogueManager.panelOpen)
                    {
                        dialogueManager.PanelEnable();
                    }
                    dialogueManager.UpdateTheScreen("???","A strange sphere is levitating in front of you, do you want to interact with it ?", 4);
                    firstInteraction = false;
                }
                else
                {
                    if (PlayerInventory.instance.nbKeyBoss >= 3)
                    {
                        Summoning();
                        PlayerInventory.instance.nbKeyBoss = 0;
                    }
                    else if (secondInteraction)
                    {
                        dialogueManager.UpdateTheScreen("???", "Nothing happened, maybe it's only the dungeon decoration after all.", 0);
                        secondInteraction = false;
                    }
                    else
                    {
                        dialogueManager.PanelDisable();
                        firstInteraction = true;
                        secondInteraction = true;
                    }
                }
            }
            if (Input.GetKeyDown(inputData.close) && dialogueManager.panelOpen)
            {
                AudioManager.instance.PlayClip("Close");
                dialogueManager.PanelDisable();
                firstInteraction = true;
                secondInteraction = true;
            }
        }
        else if (dialogueManager.panelOpen && !isInRange && dialogueManager.currentPanelUser == gameObject)
        {
            dialogueManager.PanelDisable();
        }
    }

    /// <summary>
    /// Allow the boss to play its first attack
    /// </summary>
    /// <returns>Time between each call</returns>
    IEnumerator Ability1Manager()
    {
        yield return new WaitForSeconds(timeBetweenAbility1);
        bossAbility1 = true;
        StartCoroutine(Ability1Manager());
    }

    /// <summary>
    /// Move the boss towards the player position
    /// </summary>
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

    /// <summary>
    /// Check if the player is in range of the boss melee attack
    /// </summary>
    void IsPlayerInRange()
    {
        if (Physics2D.OverlapCircle(hitPoint1.position, 1.5f, playerLayer) != null)
        {
            canMove = false;            
            animator.SetTrigger("Attack");
        }
    }

    /// <summary>
    /// Allow the boss to move and use its ability - 
    /// Called at the first frame of the idle animation
    /// </summary>
    void IdleAnimation()
    {
        canMove = true;
    }

    /// <summary>
    /// Reset the trigger for the boss abilities -
    /// Called at the end of each ability and attack of the boss
    /// </summary>
    void HasAttacked()
    {
        animator.ResetTrigger("Attack");
        animator.ResetTrigger("Ability1");
    }

    /// <summary>
    /// Play the first melee attack of the boss
    /// </summary>
    void AttackBoss1()
    {
        AudioManager.instance.PlayClip("Scythe1");
        if (Physics2D.OverlapCircle(hitPoint1.position, 1.6f, playerLayer) != null)
            HeroStats.instance.TakeDamageHero(enemyDamage);
    }

    /// <summary>
    /// Play the second melee attack of the boss
    /// </summary>
    void AttackBoss2()
    {
        AudioManager.instance.PlayClip("Scythe2");
        if (Physics2D.OverlapCircle(hitPoint2.position, 1.15f, playerLayer) != null)
            HeroStats.instance.TakeDamageHero(enemyDamage);
    }

    /// <summary>
    /// Play the first ability of the boss
    /// </summary>
    void BossAbility1()
    {
        animator.SetTrigger("Ability1");
        canMove = false;
        bossAbility1 = false;
    }

    /// <summary>
    /// Play the second ability of the boss
    /// </summary>
    void BossAbility2()
    {
        canMove = false;
        invulnerable = true;
        shieldSprite.enabled = true;
    }

    /// <summary>
    /// The boss launches 8 bullets which deals damage to the player
    /// The bullets spawn on the boss transform and go in a cardinal direction -
    /// Called at the beginning of the first ability
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

    /// <summary>
    /// Create four minions at the beginning of the second phase
    /// </summary>
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

    /// <summary>
    /// Check if all the minions are dead and disable the shield of the boss if it's true
    /// </summary>
    public void AreMinionsStillAlive()
    {
        nbMinionsAlive--;
        if (nbMinionsAlive == 0 && invulnerable)
        {
            invulnerable = false;
            shieldSprite.enabled = false;
        }
    }

    /// <inheritdoc />
    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        if (healthBossInitial/2 >= enemyHP && !dead)
            animator.SetBool("Phase2", true);
    }

    /// <summary>
    /// Launch the second phase
    /// Called at the moment the boss is mid-life
    /// </summary>
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

    /// <inheritdoc />
    protected override void IsDying()
    {
        StartCoroutine(WaitForHeroToStop());
    }

    /// <summary>
    /// Wait for the hero to get his control back
    /// </summary>
    /// <returns>Until the player can move</returns>
    IEnumerator WaitForHeroToStop()
    {
        while (!HeroMovement.instance.canPlayerMove)
        {
            yield return null;
        }
        AudioManager.instance.PlayClip("BossDeathSound");
        animator.speed /= multiplierSpeedPhase2;
        dead = true;
        canMove = false;
        animator.SetTrigger("Death");
        HeroStats.instance.StopHero();
    }

    /// <inheritdoc />
    protected override void Die()
    {
        animator.ResetTrigger("Death");
        base.Die();
        RoomManager.instance.CheckEnemiesStillIn();
        GameManager.instance.Die();
        AudioManager.instance.PlayClip("VictoryTheme");
    }

    /// <summary>
    /// Check if the player is in range of the boss
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isInRange = true;
        }
    }

    /// <summary>
    /// Check if the player is not anymore in the range of the boss.
    /// Disable the dialogue panel, it's open
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isInRange = false;
            firstInteraction = true;
            if (dialogueManager.currentPanelUser == gameObject)
            {
                dialogueManager.PanelDisable();
            }
        }
    }
}