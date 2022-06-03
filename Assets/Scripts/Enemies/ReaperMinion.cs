using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        dead = true;
        enemySpeed = 0;
        launched = false;
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