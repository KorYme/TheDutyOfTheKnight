using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class KeepForNextScene : MonoBehaviour
{
    public GameObject[] objects;
    //private int levelNumber;

    private void Awake()
    {
        foreach (var item in objects)
        {
            DontDestroyOnLoad(item);
        }
        //levelNumber = 1;
    }

    void FixedUpdate()
    {
        //Need modifications
        /*
        if (Input.GetKeyDown(KeyCode.G))
        {
            levelNumber++;
            SceneManager.LoadScene("Etage" + levelNumber.ToString());
        }*/
    }
}
