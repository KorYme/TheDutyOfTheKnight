using System.Collections;
using UnityEngine;

/// <summary>
/// The game manager
/// </summary>
public class GameManager : MonoBehaviour
{
    [HideInInspector] public bool gameLaunched;
    public bool victory;
    public float score;
    public int timer;
    public float nbTry;

    public int currentIndexResolutions = -1;
    public float currentVolume;

    public static GameManager instance;
    //Singleton initialization
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        nbTry = 0;
    }

    /// <summary>
    /// Check if the game starts
    /// </summary>
    public void InitGame ()
    {
        Debug.Log("The game starts !");
        timer = 0;
        score = 0f;
        nbTry++;
        gameLaunched = true;
        victory = true;
        StartCoroutine(Timer(nbTry));
    }

    /// <summary>
    /// Add one second to timer
    /// </summary>
    /// <param name="thisTry">Which try is it</param>
    /// <returns>Wait for 1 second</returns>
    IEnumerator Timer(float thisTry)
    {
        yield return new WaitForSeconds(1f);
        if (gameLaunched && nbTry == thisTry)
        {
            timer++;
            StartCoroutine(Timer(thisTry));
        }
    }

    /// <summary>
    /// Display the ending screen after dying and stops the timer
    /// </summary>
    public void Die()
    {
        gameLaunched = false;
        LevelManager.instance.EndGameMenu();
    }
}
