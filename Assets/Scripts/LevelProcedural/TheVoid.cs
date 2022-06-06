using UnityEngine;

/// <summary>
/// Void behaviour
/// </summary>
public class TheVoid : MonoBehaviour
{
    /// <summary>
    /// Check if the player has touched the void
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Coordinates") && HeroMovement.instance.canPlayerMove)
        {
            HeroMovement.instance.IsFalling();
        }
    }

    /// <summary>
    /// Check if a coin spawned in the void
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Coin"))
        {
            Destroy(collision.gameObject);
        }
    }
}
