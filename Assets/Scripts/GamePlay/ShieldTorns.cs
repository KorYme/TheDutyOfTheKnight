using UnityEngine;

/// <summary>
/// Script managing the damaging shield's ability
/// </summary>
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

    /// <summary>
    /// Make the hero invincible
    /// </summary>
    public void InvicibilityPlayer()
    {
        heroStats.invincibility = true;
    }

    /// <summary>
    /// Make the hero vulnerable
    /// </summary>
    public void NoInvicibilityPlayer()
    {
        heroStats.invincibility = false;
    }

    /// <summary>
    /// Display the key button of the shield's ability
    /// </summary>
    public void ShieldIsClosed()
    {
        heroAbility.shieldOpen = false;
        coolDownManager.DisplayRefreshKeyButton();
    }

    /// <summary>
    /// Check if the shield is touching an enemy and deal him damage
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (heroAbility.damagingShield && collision.gameObject.GetComponent<Enemies>() != null)
        {
            collision.gameObject.GetComponent<Enemies>().TakeDamage(heroStats.shieldDamage * Time.fixedDeltaTime);
        }
    }
}