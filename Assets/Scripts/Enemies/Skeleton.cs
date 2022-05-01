using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton : Enemies
{
    Vector2 lastPos;
    bool isHitting;

    protected override void Start()
    {
        base.Start();
        //Code nécessaire à rajouter
        SendMessage("Move", enemySpeed);
        lastPos = transform.position;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        ChooseDirection();
        IsPlayerClose();
    }

    protected void ChooseDirection()
    {
        if (Mathf.Abs(lastPos.y - transform.position.y) < Mathf.Abs(lastPos.x - transform.position.x))
        {
            if (lastPos.x > transform.position.x)
            {
                animator.SetFloat("LookX",-1f);
            }
            else if (lastPos.x < transform.position.x)
            {
                animator.SetFloat("LookX", 1f);
            }
            animator.SetFloat("LookY",0f);
        }
        else if (Mathf.Abs(lastPos.y - transform.position.y) > Mathf.Abs(lastPos.x - transform.position.x))
        {
            if (lastPos.y > transform.position.y)
            {
                animator.SetFloat("LookY", -1f);
            }
            else if (lastPos.y < transform.position.y)
            {
                animator.SetFloat("LookY", 1f);
            }
            animator.SetFloat("LookX", 0);
        }
        lastPos = transform.position;
    }

    protected void IsPlayerClose()
    {
        if (Vector3.Distance(transform.position, player.transform.position) <= 1 && !isHitting && !dead)
        {
            animator.SetTrigger("Hitting");
            isHitting = true;
            SendMessage("StopMoving");
        }
    }

    public void Hit()
    {

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, 5f);
    }

    public void HasHitted()
    {
        isHitting = false;
        SendMessage("Move", enemySpeed);
    }

    protected override void StopPlaying()
    {
        base.StopPlaying();
        SendMessage("StopMoving");
    }

    protected override void IsDying()
    {
        animator.SetBool("Dying", true);
        base.IsDying();
        SendMessage("StopMoving");
    }
}
