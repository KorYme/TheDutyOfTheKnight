using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skull : Enemies
{
    public float coolDownDashSkull;
    public float distanceToDetect;
    [SerializeField]public bool canDash;
    public Rigidbody2D rb;
    private Vector2 direction;

    protected override void Start()
    {
        base.Start();
        StartCoroutine(CoolDown(1f));
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if (canDash)
        {
            Dash();
        }
        //if (rb.velocity.magnitude != 0)
        //{
        //    rb.velocity *= Time.fixedDeltaTime / (coolDownDashSkull - 2);
        //}
    }

    void Dash()
    {
        StartCoroutine(CoolDown(coolDownDashSkull));
        if (Vector2.Distance(player.transform.position, transform.position) < distanceToDetect)
        {
            direction = (player.transform.position - transform.position).normalized;
        }
        else
        {
            direction = new Vector2(Random.Range(-1f,1f), Random.Range(-1f, 1f)).normalized;
        }
        rb.velocity = direction * enemySpeed;
        //CODE DASH
    }

    protected override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        if (slider.maxValue/2 >= enemyHP)
        {
            coolDownDashSkull /= 2;
        }
    }

    IEnumerator CoolDown(float cd)
    {
        canDash = false;
        yield return new WaitForSeconds(cd);
        canDash = true;
    }
}
