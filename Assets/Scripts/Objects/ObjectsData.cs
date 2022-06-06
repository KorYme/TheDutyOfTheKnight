using UnityEngine;

[CreateAssetMenu(fileName ="ObjectsData", menuName = "MyGame/ObjectsData")]
public class ObjectsData : ScriptableObject
{
    public Sprite sprite;
    public string objectName;
    [TextArea (4,10)]
    public string description;
    public string clipToPlay;
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
