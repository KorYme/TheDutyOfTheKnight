using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public static LevelGenerator instance;

    [Header("Level Parameters")]
    public RoomType[,] level;
    public int levelHeight;
    public int levelWidth;
    public bool spawnCentered;
    public GameObject theBigGrid;

    [Header ("Number of rooms asked")]
    public int nbShopAsked;
    public int nbAbilityAsked;
    public int nbRoomsAsked;
    public int nbChestRoomsAsked;
    [HideInInspector] public int spawnX;
    [HideInInspector] public int spawnY;

    private int totalNumberRoomsAsked;
    private int totalNumberRoomsCreated;
    private int abilityRoomCreated;
    private int shopRoomCreated;

    [HideInInspector]
    public enum RoomType
    {
        Null,
        Room,
        Spawn,
        Boss,
        Ability,
        Shop,
    }

    private List<GameObject> allRooms;
    private GameObject spawnRoom;
    private GameObject shopRoom;
    private GameObject bossRoom;
    private GameObject abilityRoom;
    private GameObject miniMapBlock;
    private Transform miniMapBlocks;
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
        miniMapBlocks = theBigGrid.transform.Find("MiniMapBlocks");
    }

    void FillRoomList()
    {
        allRooms = new List<GameObject>();
        allRooms.Clear();
        foreach (var item in Resources.LoadAll<GameObject>("Rooms"))
        {
            allRooms.Add(item);
        }
        spawnRoom = Resources.Load<GameObject>("SpecialRooms/SpawnRoom");
        shopRoom = Resources.Load<GameObject>("SpecialRooms/ShopRoom");
        bossRoom = Resources.Load<GameObject>("SpecialRooms/BossRoom");
        abilityRoom = Resources.Load<GameObject>("SpecialRooms/AbilityRoom");
        miniMapBlock = Resources.Load<GameObject>("SpecialRooms/MiniMapBlock");
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
        level = new RoomType[levelHeight, levelWidth];
        for (int i = 0; i < levelHeight; i++)
        {
            for (int y = 0; y < levelWidth; y++)
            {
                level[i, y] = RoomType.Null;
            }
        }
        level[spawnX, spawnY] = RoomType.Spawn;
        totalNumberRoomsCreated++;
        while (totalNumberRoomsCreated < totalNumberRoomsAsked)
        {
            int height = Random.Range(0,levelHeight);
            int width = Random.Range(0, levelWidth);
            if (HowManyRoundAround(height, width) > 0 && level[height, width] == RoomType.Null)
            {
                rooms.Add(new Vector2(height,width));
                level[height, width] = RoomType.Room;
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
                GameObject MiniMapBlock = Instantiate(miniMapBlock, miniMapBlocks, false);
                MiniMapBlock.transform.position = new Vector3((i - spawnX) * 20 - 0.5f, (y - spawnY) * 12, 0);
                if (level[i,y] != RoomType.Null)
                {
                    GameObject ARoom = Instantiate(ChooseTheRightRoom(level[i,y]), theBigGrid.transform, false);
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
            level[spawnX, spawnY + 1] = RoomType.Boss;
            level[spawnX + 1, spawnY] = RoomType.Ability;
            level[spawnX, spawnY - 1] = RoomType.Shop;
            shopRoomCreated++;
            abilityRoomCreated++;
        }
        else
        {
            PlacingBossRoom();
        }
    }

    /// <summary>
    /// Discover the whole minimap and undiscover it on will
    /// </summary>
    public void SeeAllMiniMap()
    {
        foreach (Transform item in miniMapBlocks)
        {
            item.gameObject.SetActive(!item.gameObject.activeSelf);
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
        if (height > 0 ? level[height - 1, width] != RoomType.Null : false)
        {
            cardinalsPoints.Add("West");
        }
        if (height < levelHeight - 1 ? level[height + 1, width] != RoomType.Null : false)
        {
            cardinalsPoints.Add("East");
        }
        if (width > 0 ? level[height, width - 1] != RoomType.Null : false)
        {
            cardinalsPoints.Add("South");
        }
        if (width < levelWidth - 1 ? level[height, width + 1] != RoomType.Null : false)
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
    GameObject ChooseTheRightRoom(RoomType room)
    {
        switch (room)
        {
            case RoomType.Spawn:
                return spawnRoom;
            case RoomType.Shop:
                return shopRoom;
            case RoomType.Boss:
                return bossRoom;
            case RoomType.Ability:
                return abilityRoom;
            default:
                return ChooseAmongAllRoomsInit();
        }
    }

    /// <summary>
    /// Choose a room among all the one who haven't already been chosen
    /// </summary>
    /// <returns>The new room</returns>
    GameObject ChooseAmongAllRoomsInit()
    {
        if (allRooms.Count == 0)
        {
            allRooms.Clear();
            foreach (var item in Resources.LoadAll<GameObject>("Rooms"))
            {
                allRooms.Add(item);
            }
        }
        GameObject thisRoom = allRooms[Random.Range(0, allRooms.Count)];
        allRooms.Remove(thisRoom);
        return thisRoom;
    }

    void PlacingBossRoom()
    {
        List<Vector2> coordinates = new List<Vector2>();
        int lessconnexion = 4;
        for (int i = 0; i < levelHeight; i++)
        {
            for (int y = 0; y < levelWidth; y++)
            {
                if (level[i,y] == RoomType.Room)
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
        level[(int)bossRoom.x,(int)bossRoom.y] = RoomType.Boss;
    }

    void PlacingOtherRooms()
    {
        while (nbAbilityAsked>abilityRoomCreated)
        {
            int height = Random.Range(0, levelHeight);
            int width = Random.Range(0, levelWidth);
            if (level[height, width] == RoomType.Room)
            {
                rooms.Remove(new Vector2(height, width));
                level[height, width] = RoomType.Ability;
                abilityRoomCreated++;
            }
        }
        while (nbShopAsked>shopRoomCreated)
        {
            int height = Random.Range(0, levelHeight);
            int width = Random.Range(0, levelWidth);
            if (level[height, width] == RoomType.Room)
            {
                rooms.Remove(new Vector2(height, width));
                level[height, width] = RoomType.Shop;
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