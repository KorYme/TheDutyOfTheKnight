using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReaperMinion : Enemies
{
    public Vector2 direction;
    BossFight boss;
    public LayerMask obstacles;
    public LayerMask targets;
    private Rigidbody2D rb;
    public bool launched;

    protected override void Start()
    {
        base.Start();
        boss = GameObject.FindGameObjectWithTag("Boss").GetComponent<BossFight>();
        enemySpeed = boss.reaperMinionBallSpeed;
        enemyDamage = boss.reaperMinionBallDamage;
        rb = GetComponent<Rigidbody2D>();
    }

    public void HasBeenLaunched()
    {
        launched = true;
    }

    protected override void StopPlaying()
    {
        base.StopPlaying();
        Destroy(transform.Find("ColliderBounce").gameObject);
    }

    protected override void IsDying()
    {
        base.IsDying();
        animator.SetTrigger("Death");
        boss.AreMinionsStillAlive();
    }
}