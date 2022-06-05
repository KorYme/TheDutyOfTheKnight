using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton : Enemies
{
    Vector2 lastPos;
    bool isHitting;
    string dir;

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

    protected void IsPlayerClose()
    {
        if (Vector3.Distance(transform.position, player.transform.position) <= 1.0f && !isHitting && !dead)
        {
            animator.SetTrigger("Hitting");
            isHitting = true;
            SendMessage("StopMovingIA");
        }
    }

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
    }

    public void HasHitted()
    {
        isHitting = false;
        SendMessage("MoveIA", enemySpeed);
    }

    public override void StartMoving()
    {
        base.StartMoving();
        if (!isHitting)
        {
            SendMessage("MoveIA", enemySpeed);
        }
    }

    public override void StopMoving()
    {
        base.StopMoving();
        SendMessage("StopMovingIA");
    }

    protected override void StopPlaying()
    {
        base.StopPlaying();
        SendMessage("StopMovingIA");
    }

    protected override void IsDying()
    {
        animator.SetBool("Dying", true);
        base.IsDying();
        SendMessage("StopMovingIA");
        AudioManager.instance.PlayClip("SkeletonDeath");
    }
}
