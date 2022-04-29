using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TotemsData", menuName = "MyGame/TotemsData")]
public class TotemsData : ScriptableObject
{
    public string totemName, description;
    public float fireDamageBonus, windCooldownBonus, earthCooldownBonus;
}
