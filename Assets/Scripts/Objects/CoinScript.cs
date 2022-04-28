using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinScript : MonoBehaviour
{
    public ObjectsData objectsData;
    public float timeToDestroying;

    private void Start()
    {
        GetComponent<CircleCollider2D>().enabled = false;        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            PlayerInventory.instance.AddToInventory(objectsData);
            Destroy(gameObject);
        }
    }

    void ActivateCoin()
    {
        GetComponent<CircleCollider2D>().enabled = true;
        Destroy(gameObject, timeToDestroying);
    }
    
}
