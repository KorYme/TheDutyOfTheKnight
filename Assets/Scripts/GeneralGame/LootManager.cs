using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootManager : MonoBehaviour
{
    public static LootManager instance;

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
    [Header("Activate if you don't want other loots inside the box")]
    public bool onlyRPInside;

    private void Start()
    {
        bossKey = Resources.Load<GameObject>("Objects/BossKey");
        refreshPotion = Resources.Load<GameObject>("Objects/OtherObjects/RefreshPotion");
        otherLoots = Resources.LoadAll<GameObject>("Objects/OtherObjects");
        InitializeTheLootList();
    }

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

    public GameObject GetALoot()
    {
        GameObject loot = lootsChest[Random.Range(0,lootsChest.Count)];
        lootsChest.Remove(loot);
        return loot;
    }
}
