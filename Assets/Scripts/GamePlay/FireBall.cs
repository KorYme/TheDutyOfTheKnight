using UnityEngine;

/// <summary>
/// Fireball's script
/// </summary>
public class FireBall : MonoBehaviour
{
    [Header ("Variables FireBall")]
    [SerializeField] private LayerMask obstacles;
    [SerializeField] private LayerMask targets;
    [HideInInspector] public float fireBallSpeed;
    [HideInInspector] public bool playerFireball;
    [HideInInspector] public float fireBallDamage;

    private void Start()
    {
        Invoke("DestroyProjectile", 5);
    }

    private void FixedUpdate()
    {
        //Give the movement to the fireball
        if (HeroMovement.instance.canPlayerMove)
        {
            transform.Translate(Vector3.right * fireBallSpeed * Time.fixedDeltaTime);
        }
    }

    /// <summary>
    /// Set the direction of the fireball
    /// </summary>
    /// <param name="direction">Direction of the fireball</param>
    public void SetDirection(Vector2 direction)
    {
        transform.eulerAngles = new Vector3(0, 0, (direction.y > 0 ? 1 : -1 ) * Vector3.Angle(Vector3.right, (Vector3)direction));
    }

    /// <summary>
    /// Destroy the fireball - 
    /// Called when the fireball meets a target or an obstacle
    /// </summary>
    private void DestroyProjectile()
    {
        Destroy(gameObject);
    }

    /// <summary>
    /// Check if the fireball meets an obstacle or a target
    /// </summary>
    /// <param name="collision"></param>
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
