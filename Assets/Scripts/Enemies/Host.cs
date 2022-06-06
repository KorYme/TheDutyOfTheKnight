using System.Collections;
using UnityEngine;

/// <summary>
/// Script of the host -
/// Inherit from the Enemies class
/// </summary>
public class Host : Enemies
{
    [Header ("Host Special Variables")]
    [SerializeField] private GameObject fireBall;
    [SerializeField] private Transform shotPoint;
    [SerializeField] private float fireBallSpeed;

    protected override void Start()
    {
        base.Start();
        invulnerable = true;
        isTouchDamage = false;
    }

    /// <summary>
    /// Launch a fireball in the direction of the player
    /// </summary>
    public void FireBallCast()
    {
        GameObject bulletLaunch = Instantiate(fireBall, shotPoint.position, Quaternion.identity);
        bulletLaunch.GetComponent<FireBall>().SetDirection((player.transform.position - transform.position).normalized);
        bulletLaunch.GetComponent<FireBall>().fireBallSpeed = fireBallSpeed;
        bulletLaunch.GetComponent<FireBall>().fireBallDamage = enemyDamage;
        AudioManager.instance.PlayClip("EnemyFireBall");
    }

    /// <inheritdoc/>
    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        if (invulnerable)
        {
            StartCoroutine(NoStagger());
            AudioManager.instance.PlayClip("NoDamageHost");
        }
    }

    /// <summary>
    /// Create a feedback when the enemy is touched but invicible
    /// </summary>
    /// <returns>Time of the no stagger animation</returns>
    protected IEnumerator NoStagger()
    {
        sprite.color = Color.gray;
        yield return new WaitForSeconds(0.1f);
        sprite.color = new Color(255, 255, 255);
    }

    /// <inheritdoc/>
    protected override void IsDying()
    {
        base.IsDying();
        AudioManager.instance.PlayClip("DeathSkullAndHost");
        animator.SetTrigger("Death");
    }
}
