using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinScript : MonoBehaviour
{
    [SerializeField] private ObjectsData objectsData;
    [SerializeField] private float timeToDestroying;
    private float flashDelay;
    Vector3 currentPosition;


    private void Start()
    {
        GetComponent<CircleCollider2D>().enabled = false;
        GetComponent<SpriteRenderer>().enabled = false;
        StartCoroutine(WaitToStartToggle());
        flashDelay = 0.2f;
        currentPosition = transform.position;
    }

    private void Update()
    {
        if (transform.position != currentPosition)
        {
            Debug.Log("LA");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            AudioManager.instance.PlayClip(objectsData.clipToPlay);
            PlayerInventory.instance.AddToInventory(objectsData);
            Destroy(gameObject);
        }
    }

    void ActivateCoinSprite()
    {
        GetComponent<SpriteRenderer>().enabled = true; 
    }

    void ActivateCoinCollider()
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
