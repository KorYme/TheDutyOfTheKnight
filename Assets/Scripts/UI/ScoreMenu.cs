using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

public class ScoreMenu : MonoBehaviour
{
    public TMP_Text victoryOrNot;
    public TMP_Text recapText;

    public void UpdateTheScore()
    {
        victoryOrNot.text = GameManager.instance.victory ? "Victory" : "Defeat";
        recapText.text = (GameManager.instance.victory ? "You've survived to the dungeon!" : "You died in the donjon!") + " Your journey lasts " + (GameManager.instance.timer / 60).ToString() + " minutes and " + (GameManager.instance.timer % 60).ToString() + " seconds, you also dealt " + GameManager.instance.score.ToString() + " damages to enemies.";
    }
}
