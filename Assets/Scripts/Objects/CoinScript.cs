using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinScript : MonoBehaviour
{
    public ObjectsData objectsData;
    public float timeToDestroying;
    private float flashDelay;
    private float coinRadius;

    private void Start()
    {
        GetComponent<CircleCollider2D>().enabled = false;
        StartCoroutine(WaitToStartToggle());
        flashDelay = 0.2f;
        coinRadius = GetComponent<CircleCollider2D>().radius;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            PlayerInventory.instance.AddToInventory(objectsData);
            Destroy(gameObject);
        }
    }

    bool CanPopCoins()
    {
        Physics2D.OverlapCircleAll(Camera.main.ScreenToWorldPoint(Input.mousePosition), coinRadius);
        foreach (Collider2D item in Physics2D.OverlapCircleAll(Camera.main.ScreenToWorldPoint(Input.mousePosition), coinRadius))
        {
            if (item.gameObject.layer == 6 || item.gameObject.layer == 8 || item.gameObject.layer == 9)
            {
                return false;
            }
        }
        return true;
    }

    void ActivateCoin()
    {
        GetComponent<CircleCollider2D>().enabled = true;
    }
    
    IEnumerator WaitToStartToggle()
    {
        yield return new WaitForSeconds(timeToDestroying / 2);
        StartCoroutine(IsToggling());
        yield return new WaitForSeconds(timeToDestroying / 4);
        flashDelay /= 2;
        yield return new WaitForSeconds(timeToDestroying / 4);
        Destroy(gameObject);
    }

    IEnumerator IsToggling()
    {
        GetComponent<SpriteRenderer>().enabled = !GetComponent<SpriteRenderer>().enabled;
        yield return new WaitForSeconds(flashDelay);
        StartCoroutine(IsToggling());
    }
}
