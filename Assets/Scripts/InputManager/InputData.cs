using UnityEngine;

[CreateAssetMenu(fileName = "InputData", menuName = "MyGame/InputData")]
public class InputData : ScriptableObject
{
    public KeyCode menu;
    public KeyCode interact;
    public KeyCode accept;
    public KeyCode swordHit;
    public KeyCode abilityEarth;
    public KeyCode abilityWind;
    public KeyCode abilityFire;
    public KeyCode abilityDash;
    public KeyCode abilityExplosion;
    public KeyCode abilityDamagingShield;
    public KeyCode inventory;
}
