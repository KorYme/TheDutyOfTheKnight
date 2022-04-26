using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [field:HideInInspector]
    public bool gameLaunched;
    public float score;
    public float timer;

    //Create the Singleton
    public static GameManager instance;
    private void Awake()
    {
        instance = null;
        if (instance != null)
        {
            Debug.LogError("More than one GameManager instance in the game !");
        }
        instance = this;
    }

    /// <summary>
    /// Check if the game starts
    /// </summary>
    public void InitGame ()
    {
        Debug.Log("The game starts !");
        timer = 0f;
        score = 0f;
        gameLaunched = true;
        
        StartCoroutine(Timer());
    }

    /// <summary>
    /// Add 1 second to the timer each second
    /// </summary>
    /// <returns></returns>
    IEnumerator Timer()
    {
        yield return new WaitForSeconds(1f);
        if (gameLaunched)
        {
            timer++;
            StartCoroutine(Timer());
        }
    }

    /// <summary>
    /// Go back to the menu after 
    /// </summary>
    public void Die()
    {
        gameLaunched = false;
        LevelManager.instance.EndGameMenu();
    }
}
