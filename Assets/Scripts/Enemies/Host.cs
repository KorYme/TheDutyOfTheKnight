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
        bulletLaunch.GetComponent<FireBall>().SetDirection((player.transform.position - transform.position).normalized);
        bulletLaunch.GetComponent<FireBall>().fireBallSpeed = fireBallSpeed;
        bulletLaunch.GetComponent<FireBall>().fireBallDamage = enemyDamage;
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        if (invulnerable)
        {
            StartCoroutine(NoStagger());
        }
    }

    protected virtual IEnumerator NoStagger()
    {
        sprite.color = Color.gray;
        yield return new WaitForSeconds(0.1f);
        sprite.color = new Color(255, 255, 255);
    }

    protected override void IsDying()
    {
        base.IsDying(); 
        animator.SetTrigger("Death");
    }
}
