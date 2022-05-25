using UnityEngine;

[CreateAssetMenu(fileName = "InputData", menuName = "MyGame/InputData")]
public class InputData : ScriptableObject
{
    [Header ("General Inputs")]
    public KeyCode menu;
    public KeyCode interact;
    public KeyCode close;
    public KeyCode inventory;
    public KeyCode miniMap;
    public KeyCode useItem;

    [Header ("Abilities Inputs")]
    public KeyCode swordHit;
    public KeyCode abilityEarth;
    public KeyCode abilityWind;
    public KeyCode abilityFire;
    public KeyCode abilityExplosion;
    public KeyCode abilityDash;
    public KeyCode abilityDamagingShield;
}
