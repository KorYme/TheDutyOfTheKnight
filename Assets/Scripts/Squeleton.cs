using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Squeleton : Enemies
{
    protected override void Start()
    {
        base.Start();
        //Code n�cessaire � rajouter
        SendMessage("Move", enemySpeed);
    }

    protected override void IsDying()
    {
        animator.SetBool("Dying", true);
        base.IsDying();
        SendMessage("StopMoving");
    }
}
