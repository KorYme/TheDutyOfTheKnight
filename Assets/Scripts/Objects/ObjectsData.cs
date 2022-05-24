using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="ObjectsData", menuName = "MyGame/ObjectsData")]
public class ObjectsData : ScriptableObject
{
    public string objectName;
    public string description;
    public Sprite sprite;
    public int coinCost;
    public int coinGiven;
    public int refreshPotionGiven;
    public int keyBossGiven;
    public float healthGiven;
    public float maxHealthGiven;
    public float speedGiven;
    public float attackGiven;
    public bool addToInventory;
}
