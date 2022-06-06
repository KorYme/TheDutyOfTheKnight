using System.Collections;
using UnityEngine;

public class CoinScript : MonoBehaviour
{
    [SerializeField] private ObjectsData objectsData;

    [Header ("Parameters")]
    [SerializeField] private float timeToDestroying;

    private float flashDelay = 0.2f;

    private void Start()
    {
        GetComponent<CircleCollider2D>().enabled = false;
        GetComponent<SpriteRenderer>().enabled = false;
        StartCoroutine(WaitToStartToggle());
    }

    /// <summary>
    /// Check if the coin has been taken
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            AudioManager.instance.PlayClip(objectsData.clipToPlay);
            PlayerInventory.instance.AddToInventory(objectsData);
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Display the coin
    /// </summary>
    void ActivateCoinSprite()
    {
        GetComponent<SpriteRenderer>().enabled = true; 
    }

    /// <summary>
    /// Make it tangible
    /// </summary>
    void ActivateCoinCollider()
    {
        GetComponent<CircleCollider2D>().enabled = true;
    }

    /// <summary>
    /// Manage the effects of the coin disappearance
    /// </summary>
    /// <returns></returns>
    IEnumerator WaitToStartToggle()
    {
        yield return new WaitForSeconds(timeToDestroying / 2);
        StartCoroutine(IsToggling());
        yield return new WaitForSeconds(timeToDestroying / 4);
        flashDelay /= 2;
        yield return new WaitForSeconds(timeToDestroying / 4);
        Destroy(gameObject);
    }

    /// <summary>
    /// Create the blinking effect on the screen
    /// </summary>
    /// <returns></returns>
    IEnumerator IsToggling()
    {
        GetComponent<SpriteRenderer>().enabled = !GetComponent<SpriteRenderer>().enabled;
        yield return new WaitForSeconds(flashDelay);
        StartCoroutine(IsToggling());
    }
}
