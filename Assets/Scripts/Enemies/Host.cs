using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Host : Enemies
{
    [Header ("Host Special Variables")]
    [SerializeField] public GameObject fireBall;
    [SerializeField] public float fireBallSpeed;
    [SerializeField] public Transform shotPoint;


    protected override void Start()
    {
        base.Start();
        invulnerable = true;
        isTouchDamage = false;
    }

    public void FireBallCast()
    {
        GameObject bulletLaunch = Instantiate(fireBall, shotPoint.position, Quaternion.identity);
        bulletLaunch.GetComponent<FireBall>().direction = (player.transform.position - transform.position).normalized;
        bulletLaunch.GetComponent<FireBall>().fireBallSpeed = fireBallSpeed;
        bulletLaunch.GetComponent<FireBall>().fireBallDamage = enemyDamage;
    }

    protected override void IsDying()
    {
        base.IsDying();
        animator.SetTrigger("Death");
    }
}
