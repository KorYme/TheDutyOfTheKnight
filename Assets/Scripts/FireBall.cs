using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    public Vector2 direction;
    public float fireBallSpeed;
    public HeroStats heroStats;

    private void Start()
    {
        heroStats = GameObject.FindGameObjectWithTag("Player").GetComponent<HeroStats>();
        Invoke("DestroyProjectile", 5);
        transform.Find("Graphics").eulerAngles = new Vector3(0,0,Vector3.Angle(Vector3.right,(Vector3)direction));
    }


    private void FixedUpdate()
    {
        transform.Translate(direction * fireBallSpeed * Time.fixedDeltaTime);
    }

    private void DestroyProjectile()
    {
        Destroy(gameObject);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 6 || collision.gameObject.layer == 8 || collision.gameObject.layer == 9)
        {
            Destroy(gameObject);
        }
        else if (collision.gameObject.layer == 7)
        {
            collision.gameObject.SendMessage("TakeDamage", heroStats.fireDamage);
            Destroy(gameObject);
        }
    }
}
