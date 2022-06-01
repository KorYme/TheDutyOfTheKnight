using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldTorns : MonoBehaviour
{
    private HeroAbility heroAbility;
    private HeroStats heroStats;
    private CoolDownManager coolDownManager;

    private void Start()
    {
        heroAbility = HeroAbility.instance;
        heroStats = HeroStats.instance;
        coolDownManager = CoolDownManager.instance;
    }

    public void InvicibilityPlayer()
    {
        heroStats.invincibility = true;
    }

    public void NoInvicibilityPlayer()
    {
        heroStats.invincibility = false;
    }

    public void ShieldIsClosed()
    {
        heroAbility.shieldOpen = false;
        coolDownManager.DisplayRefreshKeyButton();
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (heroAbility.damagingShield && collision.gameObject.GetComponent<Enemies>() != null)
        {
            collision.gameObject.GetComponent<Enemies>().TakeDamage(heroStats.shieldDamage * Time.fixedDeltaTime);
        }
    }
}