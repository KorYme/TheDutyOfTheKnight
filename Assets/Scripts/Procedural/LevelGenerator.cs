using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public string[,] level;
    [Header("Level Parameters")]
    public int levelHeight;
    public int levelWidth;
    public bool spawnCentered;
    public int nbShopAsked;
    public int nbAbilityAsked;
    public int nbRoomsAsked;
    public static int spawnX;
    public static int spawnY;

    private int totalNumberRoomsAsked;
    private int totalNumberRoomsCreated;
    private int abilityRoomCreated;
    private int shopRoomCreated;
    private GameObject[] allRooms;
    private GameObject spawnRoom;
    private GameObject shopRoom;
    private GameObject bossRoom;
    private GameObject abilityRoom;


    [Header ("Test Mode")]
    public bool testMode;


    private void Awake()
    {
        InitializingValue();
        CenteringSpawn(spawnCentered);
        CameraFollow.playerCoordinates = new Vector2(LevelGenerator.spawnX, LevelGenerator.spawnY);
        FillRoomList();
    }

    void InitializingValue()
    {
        totalNumberRoomsAsked = 2 + nbRoomsAsked + nbShopAsked + nbAbilityAsked;
        while (totalNumberRoomsAsked > levelHeight * levelWidth)
        {
            levelHeight++;
            levelWidth++;
        }
        totalNumberRoomsCreated = 0;
        abilityRoomCreated = 0;
        shopRoomCreated = 0;
    }

    void FillRoomList()
    {
        allRooms = Resources.LoadAll<GameObject>("Rooms");
        spawnRoom = Resources.Load<GameObject>("SpecialRooms/SpawnRoom");
        shopRoom = Resources.Load<GameObject>("SpecialRooms/ShopRoom");
        bossRoom = Resources.Load<GameObject>("SpecialRooms/BossRoom");
        abilityRoom = Resources.Load<GameObject>("SpecialRooms/AbilityRoom");
    }

    void CenteringSpawn(bool isCentered)
    {
        if (isCentered)
        {
            spawnX = Mathf.FloorToInt(levelWidth / 2);
            spawnY = Mathf.FloorToInt(levelHeight / 2);
        }
        else
        {
            spawnX = Random.Range(0, levelWidth);
            spawnY = Random.Range(0, levelHeight);
        }
    }

    public void CreatingLevel()
    {
        level = new string[levelHeight, levelWidth];
        for (int i = 0; i < levelHeight; i++)
        {
            for (int y = 0; y < levelWidth; y++)
            {
                level[i, y] = "Null";
            }
        }
        level[spawnX, spawnY] = "Spawn";
        totalNumberRoomsCreated++;
        while (totalNumberRoomsCreated < totalNumberRoomsAsked)
        {
            int height = Random.Range(0,levelHeight);
            int width = Random.Range(0, levelWidth);
            if (HowManyRoundAround(height, width) > 0 && level[height, width] == "Null")
            {
                level[height, width] = "Room";
                totalNumberRoomsCreated++;
            }
        }
        //Tests Room
        TestMode();
        //To Modify
        PlacingOtherRooms();
        for (int i = 0; i < levelHeight; i++)
        {
            for (int y = 0; y < levelWidth; y++)
            {
                if (level[i,y] != "Null")
                {
                    GameObject ARoom = Instantiate(ChooseRandomRoom(level[i,y]), GameObject.FindGameObjectWithTag("TheBigGrid").transform, false);
                    ARoom.name = level[i,y] + " [" + (i - spawnX).ToString() + "," + (y - spawnY).ToString() + "]";
                    ARoom.transform.position = new Vector3((i - spawnX) * 20 - 0.5f, (y - spawnY) * 12, 0);
                }
            }
        }
        AstarPath.active.Scan();
    }

    void TestMode()
    {
        if (testMode)
        {
            level[spawnX, spawnY + 1] = "Boss";
            level[spawnX + 1, spawnY] = "Ability";
            level[spawnX, spawnY - 1] = "Shop";
            shopRoomCreated++;
            abilityRoomCreated++;
        }
        else
        {
            PlacingBossRoom();
        }
    }

    int HowManyRoundAround(int height, int width)
    {
        int count = 0;
        if (height > 0 ? level[height-1,width] != "Null" : false)
        {
            count++;
        }
        if (height < levelHeight - 1 ? level[height + 1, width] != "Null" : false)
        {
            count++;
        }
        if (width > 0 ? level[height, width-1] != "Null" : false)
        {
            count++;
        }
        if (width < levelWidth - 1 ? level[height, width + 1] != "Null" : false)
        {
            count++;
        }
        return count;
    }

    public List<string> WhichRoundAround(int height, int width)
    {
        List<string> cardinalsPoints = new List<string>();
        if (height > 0 ? level[height - 1, width] != "Null" : false)
        {
            cardinalsPoints.Add("West");
        }
        if (height < levelHeight - 1 ? level[height + 1, width] != "Null" : false)
        {
            cardinalsPoints.Add("East");
        }
        if (width > 0 ? level[height, width - 1] != "Null" : false)
        {
            cardinalsPoints.Add("South");
        }
        if (width < levelWidth - 1 ? level[height, width + 1] != "Null" : false)
        {
            cardinalsPoints.Add("North");
        }
        return cardinalsPoints;
    }

    GameObject ChooseRandomRoom(string name)
    {
        if (name == "Spawn")
        {
            return spawnRoom;
        }
        else if (name == "Shop")
        {
            return shopRoom;
        }
        else if (name == "Boss")
        {
            return bossRoom;
        }
        else if (name == "Ability")
        {
            return abilityRoom;
        }
        else
        {
            return allRooms[Random.Range(0, allRooms.Length)];
        }
    }

    void PlacingBossRoom()
    {
        List<Vector2> coordinates = new List<Vector2>();
        int lessconnexion = 4;
        for (int i = 0; i < levelHeight; i++)
        {
            for (int y = 0; y < levelWidth; y++)
            {
                if (level[i,y] == "Room")
                {
                    if (HowManyRoundAround(i, y) < lessconnexion)
                    {
                        coordinates.Clear();
                        coordinates.Add(new Vector2(i, y));
                        lessconnexion = HowManyRoundAround(i, y);

                    }
                    else if (HowManyRoundAround(i, y) == lessconnexion)
                    {
                        coordinates.Add(new Vector2(i, y));
                    }
                }
            }
        }
        Vector2 bossRoom = coordinates[Random.Range(0,coordinates.Count)];
        level[(int)bossRoom.x,(int)bossRoom.y] = "Boss";
    }

    void PlacingOtherRooms()
    {
        while (nbAbilityAsked>abilityRoomCreated)
        {
            int height = Random.Range(0, levelHeight);
            int width = Random.Range(0, levelWidth);
            if (level[height, width] == "Room")
            {
                level[height, width] = "Ability";
                abilityRoomCreated++;
            }
        }
        while (nbShopAsked>shopRoomCreated)
        {
            int height = Random.Range(0, levelHeight);
            int width = Random.Range(0, levelWidth);
            if (level[height, width] == "Room")
            {
                level[height, width] = "Shop";
                shopRoomCreated++;
            }
        }
    }
}