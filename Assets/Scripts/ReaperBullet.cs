using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReaperBullet : Enemies
{
    public Vector2 direction;
    bool launched;
    BossFight boss;
    public LayerMask obstacles;
    public LayerMask targets;

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

    public void HasBeenLaunched()
    {
        launched = true;
    }

    protected override void IsDying()
    {
        animator.SetTrigger("Death");
        base.IsDying();
    }
}
