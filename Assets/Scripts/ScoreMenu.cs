using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

public class ScoreMenu : MonoBehaviour
{
    public TMP_Text variableTimer;
    public TMP_Text variableDamage;

    public void Start()
    {
        variableTimer.text = GameManager.instance.timer.ToString();
        variableDamage.text = GameManager.instance.score.ToString();
    }
}
