using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public static LevelGenerator instance;

    [Header("Level Parameters")]
    public string[,] level;
    public int levelHeight;
    public int levelWidth;
    public bool spawnCentered;
    public GameObject theBigGrid;

    [Header ("Number of rooms asked")]
    public int nbShopAsked;
    public int nbAbilityAsked;
    public int nbRoomsAsked;
    public int nbChestRoomsAsked;
    public int spawnX;
    public int spawnY;

    private int totalNumberRoomsAsked;
    private int totalNumberRoomsCreated;
    private int abilityRoomCreated;
    private int shopRoomCreated;

    private GameObject[] allRooms;
    private GameObject spawnRoom;
    private GameObject shopRoom;
    private GameObject bossRoom;
    private GameObject abilityRoom;
    [HideInInspector] public List<Vector2> rooms;

    [Header ("Test Mode")]
    public bool testMode;


    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("There is more than one LevelGenerator in the game");
            return;
        }
        instance = this;
        InitializingValue();
        CenteringSpawn(spawnCentered);
        CameraFollow.instance.playerCoordinates = new Vector2(spawnX, spawnY);
        FillRoomList();
    }

    void InitializingValue()
    {
        totalNumberRoomsAsked = 2 + nbRoomsAsked + nbShopAsked + nbAbilityAsked;
        nbChestRoomsAsked = nbChestRoomsAsked > nbRoomsAsked ? nbRoomsAsked : nbChestRoomsAsked;
        while (totalNumberRoomsAsked > levelHeight * levelWidth)
        {
            levelHeight++;
            levelWidth++;
        }
        totalNumberRoomsCreated = 0;
        abilityRoomCreated = 0;
        shopRoomCreated = 0;
        rooms = new List<Vector2>();
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
        if (isCentered || testMode)
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
        rooms.Clear();
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
                rooms.Add(new Vector2(height,width));
                level[height, width] = "Room";
                totalNumberRoomsCreated++;
            }
        }
        TestMode();
        PlacingOtherRooms();
        PlaceChestRooms();
        for (int i = 0; i < levelHeight; i++)
        {
            for (int y = 0; y < levelWidth; y++)
            {
                if (level[i,y] != "Null")
                {
                    GameObject ARoom = Instantiate(ChooseRandomRoom(level[i,y]), theBigGrid.transform, false);
                    ARoom.name = level[i,y] + " [" + (i - spawnX).ToString() + "," + (y - spawnY).ToString() + "]";
                    ARoom.transform.position = new Vector3((i - spawnX) * 20 - 0.5f, (y - spawnY) * 12, 0);
                    if (rooms.Contains(new Vector2(i,y)))
                    {
                        ARoom.name += " CHEST IN";
                    }
                    else if (ARoom.name[0] == 'R')
                    {
                        Destroy(ARoom.transform.Find("Chest").gameObject);
                    }
                }
            }
        }
        AstarPath.active.Scan();
    }

    /// <summary>
    /// Test Mode
    /// Place rooms in very specificate places
    /// </summary>
    void TestMode()
    {
        if (testMode)
        {
            rooms.Remove(new Vector2(spawnX, spawnY + 1));
            rooms.Remove(new Vector2(1+spawnX, spawnY));
            rooms.Remove(new Vector2(spawnX, spawnY-1));
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

    /// <summary>
    /// Check how many rooms are around this very room
    /// </summary>
    /// <param name="height"></param>
    /// <param name="width"></param>
    /// <returns></returns>
    int HowManyRoundAround(int height, int width)
    {
        return WhichRoundAround(height, width).Count;
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

    /// <summary>
    /// Find which room is at this very place
    /// </summary>
    /// <param name="room">The string of the room</param>
    /// <returns>The room at this place</returns>
    GameObject ChooseRandomRoom(string room)
    {
        switch (room)
        {
            case "Spawn":
                return spawnRoom;
            case "Shop":
                return shopRoom;
            case "Boss":
                return bossRoom;
            case "Ability":
                return abilityRoom;
            default:
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
        rooms.Remove(bossRoom);
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
                rooms.Remove(new Vector2(height, width));
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
                rooms.Remove(new Vector2(height, width));
                level[height, width] = "Shop";
                shopRoomCreated++;
            }
        }
    }

    void PlaceChestRooms()
    {
        while (rooms.Count > nbChestRoomsAsked)
        {
            rooms.Remove(rooms[Random.Range(0, rooms.Count-1)]);
        }
    }
}