using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReaperMinion : Enemies
{
    public Vector2 direction;
    BossFight boss;
    public LayerMask obstacles;
    public LayerMask targets;

    protected override void Start()
    {
        base.Start();
        boss = GameObject.FindGameObjectWithTag("Boss").GetComponent<BossFight>();
        direction = new Vector2((Random.Range(0, 2)*2)-1, (Random.Range(0, 2) * 2) - 1);
        enemySpeed = boss.reaperMinionBallSpeed;
        enemyDamage = boss.reaperMinionBallDamage;
    }
}