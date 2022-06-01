using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    [Header ("Variables FireBall")]
    public float fireBallSpeed;
    public LayerMask obstacles;
    public LayerMask targets;
    [HideInInspector] public bool playerFireball;
    [HideInInspector] public float fireBallDamage;

    private void Start()
    {
        Invoke("DestroyProjectile", 5);
    }

    private void FixedUpdate()
    {
        if (HeroMovement.instance.canPlayerMove)
        {
            transform.Translate(Vector3.right * fireBallSpeed * Time.fixedDeltaTime);
        }
    }

    public void SetDirection(Vector2 direction)
    {
        transform.eulerAngles = new Vector3(0, 0, (direction.y > 0 ? 1 : -1 ) * Vector3.Angle(Vector3.right, (Vector3)direction));
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
        else if (targets == (targets | (1 << collision.gameObject.layer)) && collision.tag != "Coordinates")
        {
            if (playerFireball && collision.tag != "Player")
            {
                collision.gameObject.SendMessage("TakeDamage", HeroStats.instance.fireDamage);
                Destroy(gameObject);
            }
            else if (!playerFireball && collision.tag == "Player")
            {
                collision.gameObject.SendMessage("TakeDamageHero", fireBallDamage);
                Destroy(gameObject);
            }
        }
    }
}
