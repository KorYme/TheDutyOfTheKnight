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
    public LayerMask obstacles;

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
        if (rb.velocity.magnitude != 0)
        {
            rb.velocity *= 1 - (Time.fixedDeltaTime / (coolDownDashSkull - 2));
        }
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
    }

    protected override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
    }

    IEnumerator CoolDown(float cd)
    {
        canDash = false;
        if (slider.maxValue / 2 >= enemyHP)
            yield return new WaitForSeconds(cd/2);
        else
            yield return new WaitForSeconds(cd);
        canDash = true;
    }

    protected override void IsDying()
    {
        base.IsDying();
        rb.velocity = Vector2.zero;
        rb.isKinematic = true;
        animator.SetTrigger("Dying");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (obstacles == (obstacles | (1 << collision.gameObject.layer)))
        {
            rb.velocity = Vector2.Reflect(rb.velocity, collision.contacts[0].normal).normalized * rb.velocity.magnitude;
        }
    }
}
