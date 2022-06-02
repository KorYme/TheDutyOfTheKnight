using UnityEngine;

public class MenuManager : MonoBehaviour
{
    void Start()
    {
        AudioManager.instance.PlayClip("TitleTheme");
    }
}
