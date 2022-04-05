using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashingDamage : MonoBehaviour
{
    public HeroStats heroStats;

    private void Start()
    {
        heroStats = GameObject.FindGameObjectWithTag("Player").GetComponent<HeroStats>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemies")
        {
            collision.SendMessage("TakeDamage", heroStats.dashDamage);
        }
    }

}
