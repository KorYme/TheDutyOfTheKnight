using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoolDownManager : MonoBehaviour
{
    public static CoolDownManager instance;

    HeroHits heroHits;
    HeroAbility heroAbility;

    [Header("All CoolDown Images")]
    [SerializeField] Image imageCDHit;
    [SerializeField] Image imageCDEarth;
    [SerializeField] Image imageCDWind;
    [SerializeField] Image imageCDFire;
    [SerializeField] GameObject keyButtonHit;
    [SerializeField] GameObject keyButtonEarth;
    [SerializeField] GameObject keyButtonWind;
    [SerializeField] GameObject keyButtonFire;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.Log("There is more than one CoolDownManager in the game");
            return;
        }
        instance = this;
    }

    private void Start()
    {
        InitializeAllObjects();
        InitializeCDTo0(false);
        DisplayRefreshKeyButton();
    }

    void InitializeAllObjects()
    {
        heroHits = HeroHits.instance;
        heroAbility = HeroAbility.instance;
    }

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

    public void DisplayRefreshKeyButton()
    {
        keyButtonHit.SetActive(!heroHits.isInReloadTime);
        keyButtonEarth.SetActive(!heroAbility.earthInCooldown);
        keyButtonWind.SetActive(!heroAbility.windInCooldown);
        keyButtonFire.SetActive(!heroAbility.fireInCooldown);
    }

    void FixedUpdate()
    {
        if (LevelManager.instance.pauseMenu)
            return;
        CheckCD();
    }

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
