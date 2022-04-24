using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestartGame : MonoBehaviour
{

    public RestartGame instance;

    private void Awake()
    {
        instance = null;
        if (instance != null)
        {
            Debug.LogError("More than one RestartGame instance in the game !");
        }
        instance = this;
    }
}
