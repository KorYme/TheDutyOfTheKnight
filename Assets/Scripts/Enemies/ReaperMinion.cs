using UnityEngine;

/// <summary>
/// Script of the boss' minions -
/// Inherit from the Enemies class
/// </summary>
public class ReaperMinion : Enemies
{
    private BossFight boss;
    [HideInInspector] public Vector2 direction;
    [HideInInspector] public LayerMask obstacles;
    [HideInInspector] public LayerMask targets;
    [HideInInspector] public bool launched;

    protected override void Start()
    {
        base.Start();
        boss = GameObject.FindGameObjectWithTag("Boss").GetComponent<BossFight>();
        enemySpeed = boss.reaperMinionBallSpeed;
        enemyDamage = boss.reaperMinionBallDamage;
    }

    /// <summary>
    /// Allow the minion to move
    /// Called after it spawns
    /// </summary>
    public void HasBeenLaunched()
    {
        launched = true;
    }

    /// <inheritdoc />
    protected override void StopPlaying()
    {
        base.StopPlaying();
        Destroy(transform.Find("ColliderBounce").gameObject);
    }

    /// <inheritdoc />
    protected override void IsDying()
    {
        dead = true;
        enemySpeed = 0;
        launched = false;
        animator.speed = 1;
        GetComponent<Collider2D>().enabled = false;
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        if (slider != null)
        {
            Destroy(slider.gameObject);
        }
        animator.SetTrigger("Death");
        boss.AreMinionsStillAlive();
    }
}