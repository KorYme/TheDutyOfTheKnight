using UnityEngine;
using UnityEngine.UI;

public class CoolDownManager : MonoBehaviour
{
    public static CoolDownManager instance;

    //Singleton initialization
    private void Awake()
    {
        if (instance != null)
        {
            Debug.Log("There is more than one CoolDownManager in the game");
            return;
        }
        instance = this;
    }

    private HeroHits heroHits;
    private HeroAbility heroAbility;

    [Header("All CoolDown Images")]
    [SerializeField] Image imageCDHit;
    [SerializeField] Image imageCDEarth;
    [SerializeField] Image imageCDWind;
    [SerializeField] Image imageCDFire;
    [SerializeField] GameObject keyButtonHit;
    [SerializeField] GameObject keyButtonEarth;
    [SerializeField] GameObject keyButtonWind;
    [SerializeField] GameObject keyButtonFire;

    private void Start()
    {
        InitializeAllObjects();
        InitializeCDTo0(false);
        DisplayRefreshKeyButton();
    }

    /// <summary>
    /// Initialize all the references
    /// </summary>
    void InitializeAllObjects()
    {
        heroHits = HeroHits.instance;
        heroAbility = HeroAbility.instance;
    }

    /// <summary>
    /// Set up all cooldowns to 0
    /// </summary>
    /// <param name="notStart"></param>
    public void InitializeCDTo0(bool notStart = true)
    {
        if (!notStart)
        {
            imageCDHit.fillAmount = 0;
        }
        imageCDEarth.fillAmount = 0;
        imageCDWind.fillAmount = 0;
        imageCDFire.fillAmount = 0;
        heroAbility.earthInCooldown = false;
        heroAbility.windInCooldown = false;
        heroAbility.fireInCooldown = false;
    }

    /// <summary>
    /// Check if it can display the key of each ability
    /// </summary>
    public void DisplayRefreshKeyButton()
    {
        keyButtonHit.SetActive(!heroHits.isInReloadTime);
        keyButtonEarth.SetActive(!heroAbility.earthInCooldown && !heroAbility.shieldOpen);
        keyButtonWind.SetActive(!heroAbility.windInCooldown);
        keyButtonFire.SetActive(!heroAbility.fireInCooldown);
    }

    void FixedUpdate()
    {
        if (LevelManager.instance.pauseMenu)
            return;
        CheckCD();
    }

    /// <summary>
    /// Check if an ability is on cooldown and decrease its remaining time 
    /// </summary>
    void CheckCD()
    {
        if (heroHits.isInReloadTime)
        {
            imageCDHit.fillAmount -= 1 / (heroHits.reloadTime + 0.6f) * Time.fixedDeltaTime;
            if (imageCDHit.fillAmount <= 0)
            {
                heroHits.isInReloadTime = false;
                DisplayRefreshKeyButton();
            }
        }
        if (heroAbility.earthInCooldown)
        {
            imageCDEarth.fillAmount -= 1 / (heroAbility.cooldownEarth + HeroStats.instance.shieldDuration) * Time.fixedDeltaTime;
            if (imageCDEarth.fillAmount <= 0)
            {
                heroAbility.earthInCooldown = false;
                DisplayRefreshKeyButton();
            }
        }
        if (heroAbility.windInCooldown)
        {
            imageCDWind.fillAmount -= 1 / heroAbility.cooldownWind * Time.fixedDeltaTime;
            if (imageCDWind.fillAmount <= 0)
            {
                heroAbility.windInCooldown = false;
                DisplayRefreshKeyButton();
            }
        }
        if (heroAbility.fireInCooldown)
        {
            imageCDFire.fillAmount -= 1 / heroAbility.cooldownFire * Time.fixedDeltaTime;
            if (imageCDFire.fillAmount <= 0)
            {
                heroAbility.fireInCooldown = false;
                DisplayRefreshKeyButton();
            }
        }
    }

    /// <summary>
    /// Set up a cooldown to 1 -
    /// Called after using an ability
    /// </summary>
    /// <param name="name">Ability's name</param>
    public void ResetCoolDown(string name)
    {
        switch (name)
        {
            case "Hit":
                imageCDHit.fillAmount = 1;
                return;
            case "Earth":
                imageCDEarth.fillAmount = 1;
                return;
            case "Wind":
                imageCDWind.fillAmount = 1;
                return;
            case "Fire":
                imageCDFire.fillAmount = 1;
                return;
        }
    }
}
