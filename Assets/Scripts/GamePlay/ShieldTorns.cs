using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldTorns : MonoBehaviour
{
    private HeroAbility heroAbility;
    private HeroStats heroStats;

    private void Start()
    {
        heroAbility = HeroAbility.instance;
        heroStats = HeroStats.instance;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (heroAbility.damagingShield && collision.gameObject.GetComponent<Enemies>() != null)
        {
            collision.gameObject.GetComponent<Enemies>().TakeDamage(heroStats.shieldDamage * Time.fixedDeltaTime);
        }
    }
}