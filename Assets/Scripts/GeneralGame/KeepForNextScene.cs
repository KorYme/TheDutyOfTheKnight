using UnityEngine.SceneManagement;
using UnityEngine;

/// <summary>
/// Script used to keep usefuls objects between sceens
/// </summary>
public class KeepForNextScene : MonoBehaviour
{
    public static KeepForNextScene instance;

    [SerializeField] private GameObject[] objects;

    //Singleton initialization
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
    
    /// <summary>
    /// Remove all GameObjects of the DontDestroyOnLoad array
    /// </summary>
    void RemoveFromDontDestroyOnLoad()
    {
        foreach (var item in objects)
        {
            SceneManager.MoveGameObjectToScene(item, SceneManager.GetActiveScene());
        }
    }
}
