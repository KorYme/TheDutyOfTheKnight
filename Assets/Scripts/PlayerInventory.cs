using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public int nbCoins;
    public static PlayerInventory instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("There is more than one PlayerInventory in the game");
            return;
        }
        instance = this;
    }

    void Start()
    {
        nbCoins = 0;
    }

    void Update()
    {
        
    }
}
