using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    public float enemyHP = 100;
    public float enemyDamage = 10;
    protected bool dead = false;
    protected RoomManager roomManager;

    protected virtual void Start()
    //void Start()
    {
        roomManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<RoomManager>();
    }   

    protected virtual void FixedUpdate()
    //private void FixedUpdate()
    {
        if (enemyHP <= 0 && !dead)
        {
            dead = true;
            GetComponentInChildren<Animator>().SetBool("Dying", true);
            GetComponent<CapsuleCollider2D>().enabled = false;
            SendMessage("StopMoving");
            Destroy(this.gameObject, 2.5f);
            roomManager.CheckEnemiesStillIn();
        }
        if (this.GetComponent<Collider2D>().IsTouching(GameObject.FindGameObjectWithTag("Player").GetComponent<Collider2D>()))
        {
            GameObject.FindGameObjectWithTag("Player").SendMessage("TakeDamageHero", enemyDamage);
        }
    }

    public void TakeDamage(int damage)
    {
        enemyHP -= damage;
        StartCoroutine(Stagger());
    }

    protected IEnumerator Stagger()
    {
        this.transform.Find("Graphics").GetComponent<SpriteRenderer>().color = new Color(255, 0, 0);
        yield return new WaitForSeconds(0.1f);
        this.transform.Find("Graphics").GetComponent<SpriteRenderer>().color = new Color(255, 255, 255);
    }
}
