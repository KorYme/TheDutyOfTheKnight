using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

public class ScoreMenu : MonoBehaviour
{
    public GameObject entryMenu;
    public TMP_Text variableTimer;
    public TMP_Text variableDamage;

    public void Start()
    {
        Debug.Log("Ca marche jusque la " + GameManager.instance.firstGame);
        if (true)
        {
            variableTimer.text = GameManager.instance.timer.ToString();
            variableDamage.text = GameManager.instance.score.ToString();
            entryMenu.SetActive(false);
        }
        else
        {
            //entryMenu.SetActive(true);
            //this.gameObject.SetActive(false);
        }
    }

    public void CloseSettings()
    {
        gameObject.SetActive(false);
        entryMenu.SetActive(true);
    }
}
