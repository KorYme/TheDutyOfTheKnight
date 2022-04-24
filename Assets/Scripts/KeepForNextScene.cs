using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class KeepForNextScene : MonoBehaviour
{
    public static KeepForNextScene instance;
    public GameObject[] objects;

    private void Awake()
    {
        instance = null;
        if (instance != null)
        {
            Debug.LogError("More than one KeepForNextScene instance in the game !");
        }
        instance = this;

        foreach (var item in objects)
        {
            DontDestroyOnLoad(item);
        }
    }

    void RemoveFromDontDestroyOnLoad()
    {
        foreach (var item in objects)
        {
            SceneManager.MoveGameObjectToScene(item, SceneManager.GetActiveScene());
        }
    }
}
