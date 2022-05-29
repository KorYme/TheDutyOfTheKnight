using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheVoid : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Coordinates")
        {
            HeroMovement.instance.IsFalling();
        }
        else if (collision.tag == "Coin")
        {
            Destroy(collision.gameObject);
        }
    }
}
