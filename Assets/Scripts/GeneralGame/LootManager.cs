using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script managing all the loot in the chest to have the right number of items
/// </summary>
public class LootManager : MonoBehaviour
{
    public static LootManager instance;
    //Singleton initialization
    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("There is more than one LootManager in the game");
            return;
        }
        instance = this;
    }

    private List<GameObject> lootsChest;
    private GameObject bossKey, refreshPotion;
    private GameObject[] otherLoots;

    [Header("Only refresh potion in chest")]
    [SerializeField] private bool onlyRPInside;

    private void Start()
    {
        bossKey = Resources.Load<GameObject>("Objects/BossKey");
        refreshPotion = Resources.Load<GameObject>("Objects/OtherObjects/RefreshPotion");
        otherLoots = Resources.LoadAll<GameObject>("Objects/OtherObjects");
        InitializeTheLootList();
    }

    /// <summary>
    /// Create the loot's list
    /// </summary>
    void InitializeTheLootList()
    {
        lootsChest = new List<GameObject>();
        lootsChest.Clear();
        for(int i = 0; i < LevelGenerator.instance.nbChestRoomsAsked; i++)
        {
            if (i<3)
            {
                lootsChest.Add(bossKey);
            }
            else
            {
                if (onlyRPInside)
                {
                    lootsChest.Add(refreshPotion);
                }
                else
                {
                    lootsChest.Add(otherLoots[Random.Range(0,otherLoots.Length)]);
                }
            }
        }
    }

    /// <summary>
    /// Take a random loot in the loot's list
    /// </summary>
    /// <returns>An available loot</returns>
    public GameObject GetALoot()
    {
        GameObject loot = lootsChest[Random.Range(0,lootsChest.Count)];
        lootsChest.Remove(loot);
        return loot;
    }
}
