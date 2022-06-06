using UnityEngine;

/// <summary>
/// Script of the skeleton -
/// Inherit from the Enemies class
/// </summary>
public class Skeleton : Enemies
{
    private Vector2 lastPos;
    private bool isHitting;
    private string dir;

    protected override void Start()
    {
        base.Start();
        SendMessage("MoveIA", enemySpeed);
        lastPos = transform.position;
        dir = "Bottom";
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        ChooseDirectionRun();
        IsPlayerClose();
    }

    /// <summary>
    /// Send to the animator the direction in which the skeleton is going
    /// </summary>
    protected void ChooseDirectionRun()
    {
        if (Mathf.Abs(lastPos.y - transform.position.y) < Mathf.Abs(lastPos.x - transform.position.x))
        {
            if (lastPos.x > transform.position.x)
            {
                animator.SetFloat("LookX",-1f);
                dir = "Left";
            }
            else if (lastPos.x < transform.position.x)
            {
                animator.SetFloat("LookX", 1f);
                dir = "Right";
            }
            animator.SetFloat("LookY",0f);
        }
        else if (Mathf.Abs(lastPos.y - transform.position.y) > Mathf.Abs(lastPos.x - transform.position.x))
        {
            if (lastPos.y > transform.position.y)
            {
                animator.SetFloat("LookY", -1f);
                dir = "Bottom";
            }
            else if (lastPos.y < transform.position.y)
            {
                animator.SetFloat("LookY", 1f);
                dir = "Top";
            }
            animator.SetFloat("LookX", 0);
        }
        lastPos = transform.position;
    }

    /// <summary>
    /// Check if the player is close enough from the skeleton
    /// </summary>
    protected void IsPlayerClose()
    {
        if (Vector3.Distance(transform.position, player.transform.position) <= 1.0f && !isHitting && !dead)
        {
            animator.SetTrigger("Hitting");
            isHitting = true;
            SendMessage("StopMovingIA");
        }
    }

    /// <summary>
    /// Create the damaging zone of the sword with the direction in which the skeleton is looking at
    /// </summary>
    public void Hit()
    {
        if (dir == "Top")
        {
            if (Physics2D.OverlapCircle(transform.position + new Vector3(-0.1f, 0.4f, 0), .6f, playerLayer) != null)
            {
                player.SendMessage("TakeDamageHero", enemyDamage);
            }
        }
        else if (dir == "Bottom")
        {
            if (Physics2D.OverlapCircle(transform.position + new Vector3(0.1f, -0.35f, 0), .6f, playerLayer) != null)
            {
                player.SendMessage("TakeDamageHero", enemyDamage);
            }
        }
        else if (dir == "Left")
        {
            if (Physics2D.OverlapCircle(transform.position + new Vector3(-0.4f, .1f, 0), .6f, playerLayer) != null)
            {
                player.SendMessage("TakeDamageHero", enemyDamage);
            }
        }
        else if (dir == "Right")
        {
            if (Physics2D.OverlapCircle(transform.position + new Vector3(0.4f, .1f, 0), .6f, playerLayer) != null)
            { 
                player.SendMessage("TakeDamageHero", enemyDamage);
            }
        }
        AudioManager.instance.PlayClip("EnemySwing" + Random.Range(1, 6));
    }

    /// <summary>
    /// Allow the skeleton to hit again
    /// </summary>
    public void HasHitted()
    {
        isHitting = false;
        SendMessage("MoveIA", enemySpeed);
    }

    /// <inheritdoc />
    public override void StartMoving()
    {
        base.StartMoving();
        if (!isHitting)
        {
            SendMessage("MoveIA", enemySpeed);
        }
    }

    /// <inheritdoc />
    public override void StopMoving()
    {
        base.StopMoving();
        SendMessage("StopMovingIA");
    }

    /// <inheritdoc />
    protected override void StopPlaying()
    {
        base.StopPlaying();
        SendMessage("StopMovingIA");
    }

    /// <inheritdoc />
    protected override void IsDying()
    {
        animator.SetBool("Dying", true);
        base.IsDying();
        SendMessage("StopMovingIA");
        AudioManager.instance.PlayClip("SkeletonDeath");
    }
}
