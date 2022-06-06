using UnityEngine;

/// <summary>
/// Script managing the main menu scene
/// </summary>
public class MenuManager : MonoBehaviour
{
    void Start()
    {
        AudioManager.instance.PlayClip("TitleTheme");
    }
}
