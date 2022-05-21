using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    [Header ("Variables FireBall")]
    [SerializeField] public float fireBallSpeed;
    [SerializeField] public LayerMask obstacles;
    [SerializeField] public LayerMask targets;
    [SerializeField] public Vector2 direction;

    private void Start()
    {
        Invoke("DestroyProjectile", 5);
        transform.eulerAngles = new Vector3(0, 0, (direction.y > 0 ? 1 : -1 ) * Vector3.Angle(Vector3.right, (Vector3)direction));
    }


    private void FixedUpdate()
    {
        transform.Translate(Vector3.right * fireBallSpeed * Time.fixedDeltaTime);
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
            collision.gameObject.SendMessage("TakeDamage", HeroStats.instance.fireDamage);
            Destroy(gameObject);
        }
    }
}
