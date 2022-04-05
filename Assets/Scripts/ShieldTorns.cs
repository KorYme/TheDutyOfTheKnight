using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldTorns : MonoBehaviour
{
    private HeroAbility heroAbility;
    private HeroStats heroStats;
    private List<GameObject> enemyAlreadyHit;


    private void Start()
    {
        heroAbility = GameObject.FindGameObjectWithTag("Player").GetComponent<HeroAbility>();
        heroStats = GameObject.FindGameObjectWithTag("Player").GetComponent<HeroStats>();
        enemyAlreadyHit = new List<GameObject>();
    }

    private void FixedUpdate()
    {
        if (!heroAbility.damagingShield)
        {
            enemyAlreadyHit.Clear();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!enemyAlreadyHit.Contains(collision.gameObject) && heroAbility.damagingShield && collision.gameObject.tag == "Enemies")
        {
            enemyAlreadyHit.Add(collision.gameObject);
            collision.gameObject.SendMessage("TakeDamage", heroStats.shieldDamage);
            Debug.Log(collision.gameObject.name);
        }
    }
}