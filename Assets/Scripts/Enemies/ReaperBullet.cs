using UnityEngine;

/// <summary>
/// Script of the boss' bullets -
/// Inherit from the Enemies class
/// </summary>
public class ReaperBullet : Enemies
{
    private BossFight boss;
    private bool launched;
    [HideInInspector] public Vector2 direction;
    [Header("Layers to fill")]
    [SerializeField] private LayerMask obstacles;
    [SerializeField] private LayerMask targets;

    protected override void Start()
    {
        base.Start();
        boss = GameObject.FindGameObjectWithTag("Boss").GetComponent<BossFight>();
        enemySpeed = boss.reaperFireBallSpeed;
        enemyDamage = boss.reaperFireBallDamage;
    }

    protected override void FixedUpdate()
    {
        if (launched)
        {
            base.FixedUpdate();
            transform.Translate(direction* enemySpeed * Time.fixedDeltaTime);
        }
        if (boss.dead && !dead)
        {
            IsDying();
        }
    }

    /// <summary>
    /// Destroy this enemy when it meets a wall or the player
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (obstacles == (obstacles | (1 << collision.gameObject.layer)))
        {
            IsDying();
        }
        else if (targets == (targets | (1 << collision.gameObject.layer)))
        {
            collision.gameObject.SendMessage("TakeDamageHero", enemyDamage);
            IsDying();
        }
    }

    /// <summary>
    /// Allow the bullets to move -
    /// Called at the end of its spawn animation
    /// </summary>
    public void HasBeenLaunched()
    {
        launched = true;
    }

    /// <inheritdoc />
    protected override void IsDying()
    {
        animator.SetTrigger("Death");
        base.IsDying();
    }
}
