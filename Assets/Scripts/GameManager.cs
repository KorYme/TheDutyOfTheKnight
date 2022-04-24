using UnityEngine;

public class GameManager : MonoBehaviour
{
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

    public LevelGenerator levelGenerator;

    private void Start()
    {
        levelGenerator = GetComponent<LevelGenerator>();
    }

    /// <summary>
    /// Check if the game starts
    /// </summary>
    public void InitGame ()
    {
        Debug.Log("The game starts !");
    }
}
