using UnityEngine;

[CreateAssetMenu(fileName = "TotemsData", menuName = "MyGame/TotemsData")]
public class TotemsData : ScriptableObject
{
    public string totemName;
    [TextArea(4,10)]
    public string description;
    [TextArea(4,10)]
    public string textForPrayer;
    public float fireCooldownBonus, fireDamageBonus, explosionDamageBonus, windCooldownBonus, earthCooldownBonus, earthDurationBonus;
}
