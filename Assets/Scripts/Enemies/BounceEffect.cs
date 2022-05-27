using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceEffect : MonoBehaviour
{
    private GameObject parent;
    private ReaperMinion reaperMinion;
    private LayerMask obstacles;
    private Rigidbody2D rb;
    private Vector3 initialVelocity;
    private Vector3 lastPosition;
    private int frameNoMove;

    void Start()
    {
        parent = transform.parent.gameObject;
        reaperMinion = parent.GetComponent<ReaperMinion>();
        obstacles = reaperMinion.obstacles;
        rb = parent.GetComponent<Rigidbody2D>();
        initialVelocity = reaperMinion.direction * reaperMinion.enemySpeed;
        lastPosition = transform.position;
    }
    void FixedUpdate()
    {
        if (reaperMinion.launched)
        {
            rb.velocity = initialVelocity;
            if (lastPosition != parent.transform.position)
            {
                lastPosition = parent.transform.position;
                frameNoMove = 0;
            }
            else
            {
                frameNoMove++;
                if (frameNoMove >= 10)
                {
                    initialVelocity *= -1;
                    frameNoMove = 0;
                }
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (obstacles == (obstacles | (1 << collision.gameObject.layer)))
        {
            float speed = initialVelocity.magnitude;
            Vector3 direction = Vector3.Reflect(initialVelocity.normalized, collision.contacts[0].normal);
            initialVelocity = direction * speed;
        }
    }
}
