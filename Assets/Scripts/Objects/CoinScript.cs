using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinScript : MonoBehaviour
{
    public ObjectsData objectsData;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            PlayerInventory.instance.nbCoins += objectsData.coinGiven;
            Debug.Log("Le joueur a " + PlayerInventory.instance.nbCoins + " golds");
            Destroy(gameObject);
        }
    }
}
