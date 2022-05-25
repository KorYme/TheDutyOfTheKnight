using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoolDownManager : MonoBehaviour
{

    Image imageCDHit;
    Image imageCDEarth;
    Image imageCDWind;
    Image imageCDFire;
    Text textCDEarth;
    Text textCDWind;
    Text textCDFire;
    private HeroHits heroHits;


    [Header("All CoolDown Images")]
    public GameObject CDHitUI;
    public GameObject CDEarthUI;
    public GameObject CDWindUI;
    public GameObject CDFireUI;


    private void Start()
    {
        InitializeAllObjects();
        InitializeCD();
    }

    void InitializeAllObjects()
    {
        heroHits = HeroHits.instance;
        imageCDHit = CDHitUI.transform.Find("CoolDown").GetComponent<Image>();
        imageCDEarth = CDEarthUI.transform.Find("CoolDown").GetComponent<Image>();
        imageCDWind = CDWindUI.transform.Find("CoolDown").GetComponent<Image>();
        imageCDFire = CDFireUI.transform.Find("CoolDown").GetComponent<Image>();
        textCDEarth = CDEarthUI.transform.Find("Text").GetComponent<Text>();
        textCDWind = CDWindUI.transform.Find("Text").GetComponent<Text>();
        textCDFire = CDFireUI.transform.Find("Text").GetComponent<Text>();
    }

    void InitializeCD()
    {
        imageCDHit.fillAmount = 0;
        imageCDEarth.fillAmount = 0;
        imageCDWind.fillAmount = 0;
        imageCDFire.fillAmount = 0;
    }

    void FixedUpdate()
    {
        CheckCD();
    }

    void CheckCD()
    {
        if (heroHits.isInReloadTime)
        {
            imageCDHit.fillAmount -= 1 / (heroHits.reloadTime + 0.6f) * Time.fixedDeltaTime;
        }
        if (HeroAbility.instance.earthInCooldown)
        {
            imageCDEarth.fillAmount -= 1 / (HeroAbility.instance.cooldownEarth + HeroStats.instance.shieldDuration) * Time.fixedDeltaTime;
            textCDEarth.text = Mathf.Floor((HeroAbility.instance.cooldownEarth + HeroStats.instance.shieldDuration) * imageCDEarth.fillAmount).ToString();
            if (imageCDEarth.fillAmount <= 0)
            {
                HeroAbility.instance.earthInCooldown = false;
            }
        }
        else
        {
            textCDEarth.text = "";
        }
        if (HeroAbility.instance.windInCooldown)
        {
            imageCDWind.fillAmount -= 1 / HeroAbility.instance.cooldownWind * Time.fixedDeltaTime;
            textCDWind.text = Mathf.Floor(HeroAbility.instance.cooldownWind * imageCDWind.fillAmount).ToString();
            if (imageCDWind.fillAmount <= 0)
            {
                HeroAbility.instance.windInCooldown = false;
            }
        }
        else
        {
            textCDWind.text = "";
        }
        if (HeroAbility.instance.fireInCooldown)
        {
            imageCDFire.fillAmount -= 1 / HeroAbility.instance.cooldownFire * Time.fixedDeltaTime;
            textCDFire.text = Mathf.Floor(HeroAbility.instance.cooldownFire * imageCDFire.fillAmount).ToString();
            if (imageCDFire.fillAmount <= 0)
            {
                HeroAbility.instance.fireInCooldown = false;
            }
        }
        else
        {
            textCDFire.text = "";
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
