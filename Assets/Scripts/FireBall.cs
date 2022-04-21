using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    private HeroStats heroStats;

    [Header ("Variables FireBall")]
    public float fireBallSpeed;
    public LayerMask obstacles;
    public LayerMask targets;
    public Vector2 direction;

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
        if (obstacles == (obstacles | (1 << collision.gameObject.layer)))
        {
            Destroy(gameObject);
        }
        else if (targets == (targets | (1 << collision.gameObject.layer)))
        {
            collision.gameObject.SendMessage("TakeDamage", heroStats.fireDamage);
            Destroy(gameObject);
        }
    }
}
