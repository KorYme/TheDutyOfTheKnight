using System.Collections;
using UnityEngine;

/// <summary>
/// Script of the skull -
/// Inherit from the Enemies class
/// </summary>
public class Skull : Enemies
{
    [SerializeField] private bool canDash;
    public float coolDownDashSkull;
    public float distanceToDetect;
    public Rigidbody2D rb;
    private Vector2 direction;
    public LayerMask obstacles;
    private Vector2 initialVelocity;

    protected override void Start()
    {
        base.Start();
        StartCoroutine(CoolDown(Random.Range(1f, 2f)));
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if (canDash && canMove)
        {
            Dash();
        }
        if (rb.velocity.magnitude != 0)
        {
            rb.velocity *= 1 - (Time.fixedDeltaTime / (coolDownDashSkull - 2));
        }
    }

    /// <summary>
    /// Make the skull dash in the direction of the player
    /// </summary>
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

    /// <inheritdoc />
    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
    }

    /// <summary>
    /// Wait before giving the permission to the skull to dash again
    /// </summary>
    /// <param name="cd">Time to wait</param>
    /// <returns>Time to wait</returns>
    IEnumerator CoolDown(float cd)
    {
        canDash = false;
        if (slider.maxValue / 2 >= enemyHP)
            yield return new WaitForSeconds(cd/2);
        else
            yield return new WaitForSeconds(cd);
        canDash = true;
    }

    /// <inheritdoc />
    protected override void IsDying()
    {
        base.IsDying();
        rb.velocity = Vector2.zero;
        rb.isKinematic = true;
        animator.SetTrigger("Dying");
    }

    /// <inheritdoc />
    public override void StartMoving()
    {
        base.StartMoving();
        rb.velocity = initialVelocity;
    }

    /// <inheritdoc />
    public override void StopMoving()
    {
        base.StopMoving();
        initialVelocity = rb.velocity;
        rb.velocity = Vector2.zero;
    }

    /// <summary>
    /// I tried to make it bounce against wall
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (obstacles == (obstacles | (1 << collision.gameObject.layer)))
        {
            rb.velocity = Vector2.Reflect(rb.velocity, collision.contacts[0].normal).normalized * rb.velocity.magnitude;
        }
    }
}
