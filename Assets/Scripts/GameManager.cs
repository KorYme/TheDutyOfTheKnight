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
    //Check if the game starts
    public void InitGame ()
    {
        Debug.Log("The game starts !");
    }


}
