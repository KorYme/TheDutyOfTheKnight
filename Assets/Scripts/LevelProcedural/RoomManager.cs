using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RoomManager : MonoBehaviour
{
    public static RoomManager instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("There is more than one RoomManager in this game");
            return;
        }
        instance = this;
    }

    private HeroAbility heroAbility;
    private GameObject player;

    [Header ("Filling values")]
    [SerializeField] private InputData inputData;
    [SerializeField] private LevelGenerator levelGenerator;
    [SerializeField] private GameObject[] allBosses;
    [SerializeField] private float voidDamage;

    [Header ("Layers to fill")]
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private LayerMask bossLayer;
    [SerializeField] private LayerMask closedDoorsMask;
    [SerializeField] private LayerMask openedDoorsMask;
    [SerializeField] private LayerMask spawner;
    [SerializeField] private LayerMask shopLayer;
    [SerializeField] private LayerMask respawnLayer;
    [SerializeField] private LayerMask chestLayer;
    [SerializeField] private LayerMask miniMapBlocks;

    private Collider2D[] theClosedDoorsHere, theOpenedDoorsHere, insideEnemies, allSpawner, theClosedDoorsBoss;
    private Collider2D enemiesIn;
    private Vector2 camPos, camSize;

    void Start()
    {
        InitiateValues();
    }

    /// <summary>
    /// Initialize all the main values
    /// </summary>
    public void InitiateValues()
    {
        player = HeroAbility.instance.gameObject;
        heroAbility = player.GetComponent<HeroAbility>();
        camSize = new Vector2(17.8162708f, 9.53441048f);
        player.transform.position = new Vector3(-0.5f, 0, 0);
        ChangingRoom();
        heroAbility.earthUnlocked = true;
        heroAbility.windUnlocked = true;
        heroAbility.fireUnlocked = true;
    }

    public void ChangingRoom()
    {
        camPos = Camera.main.transform.position;
        theOpenedDoorsHere = Physics2D.OverlapBoxAll(camPos, camSize, 0f, openedDoorsMask);
        theClosedDoorsHere = Physics2D.OverlapBoxAll(camPos, camSize, 0f, closedDoorsMask);
        Collider2D thisRoomBlock = Physics2D.OverlapBox(camPos, camSize, 0f, miniMapBlocks);
        if (thisRoomBlock != null)
        {
            Destroy(thisRoomBlock.gameObject);
        }
        ActivateEnemies();
        AreEnemiesIn();
    }

    public void ActivateEnemies()
    {
        allSpawner = Physics2D.OverlapBoxAll(camPos, camSize, 0f, spawner);
        foreach (var item in allSpawner)
        {
            item.SendMessage("Spawn");
        }
    }

    /// <summary>
    /// Open or close the doors
    /// </summary>
    /// <param name="that">If true open the doors else close them</param>
    private void OpenOrCloseTheDoors(bool that)
    {
        //And desactivate the chosen ones
        List<string> cardinalsPoints = levelGenerator.WhichRoundAround((int)CameraFollow.instance.playerCoordinates.x, (int)CameraFollow.instance.playerCoordinates.y);
        if (levelGenerator.level[(int)CameraFollow.instance.playerCoordinates.x, (int)CameraFollow.instance.playerCoordinates.y] == "Boss")
        {
            theClosedDoorsHere = theClosedDoorsBoss;
        }
        if (that)
        {
            //Door Sound
            //Only open the doors with other doors behind
            foreach (var item in theOpenedDoorsHere)
            {
                item.gameObject.SetActive(cardinalsPoints.Contains(item.tag));
            }
            foreach (var item in theClosedDoorsHere)
            {
                item.gameObject.SetActive(!cardinalsPoints.Contains(item.tag));
            }
        }
        else
        {
            //Close Everything
            foreach (var item in theOpenedDoorsHere)
            {
                item.gameObject.SetActive(false);
            }
            foreach (var item in theClosedDoorsHere)
            {
                item.gameObject.SetActive(true);
            }
        }
    }


    /// <summary>
    /// Check if at least one enemy is still in this room 
    /// </summary>
    public void CheckEnemiesStillIn()
    {
        enemiesIn = Physics2D.OverlapBox(camPos, camSize, 0f, enemyLayer);
        if (enemiesIn == null)
        {
            OpenOrCloseTheDoors(true);
            OpenTheChestInTheRoom();
            //Add Opening door sound
        }
    }

    public bool IsNotInFight()
    {
        return Physics2D.OverlapBox(camPos, camSize, 0f, enemyLayer) == null;
    }

    /// <summary>
    /// Check if there is a chest in the room and then open it if it exists
    /// </summary>
    void OpenTheChestInTheRoom()
    {
        foreach (var item in Physics2D.OverlapBoxAll(camPos, camSize, 0f, chestLayer))
        {
            if (item.GetComponent<Chest>())
            {
                item.GetComponent<Chest>().CanBeOpen(true);
            }
        }
    }

    /// <summary>
    /// Check if at least one enemy is in this new room
    /// </summary>
    public void AreEnemiesIn(bool noBoss=true)
    {
        camPos = Camera.main.transform.position;
        if (theClosedDoorsBoss == null && levelGenerator.level[(int)CameraFollow.instance.playerCoordinates.x, (int)CameraFollow.instance.playerCoordinates.y] == "Boss")
        {
            theClosedDoorsBoss = theClosedDoorsHere;
        }
        if (noBoss)
        {
            insideEnemies = Physics2D.OverlapBoxAll(camPos, camSize, 0f, enemyLayer);
        }
        else
        {
            insideEnemies = Physics2D.OverlapBoxAll(camPos, camSize, 0f, bossLayer);
            enemyLayer |= (1 << LayerMask.NameToLayer("BossLayer"));
        }
        if (insideEnemies.Length == 0)
        {
            OpenOrCloseTheDoors(true);
        }
        else
        {
            foreach (var item in insideEnemies)
            {
                item.gameObject.SetActive(true);
            }
            OpenOrCloseTheDoors(false);
        }
    }

    /// <summary>
    /// Allow the enemies in the room to move or not
    /// </summary>
    /// <param name="that">True if you want them to move and else false</param>
    public void EnemiesMoveEnable(bool that)
    {
        insideEnemies = Physics2D.OverlapBoxAll(camPos, camSize, 0f, enemyLayer);
        if (that)
        {
            foreach (var item in insideEnemies)
            {
                item.gameObject.SendMessage("StartMoving");
            }
        }
        else
        {
            foreach (var item in insideEnemies)
            {
                item.gameObject.SendMessage("StopMoving");
            }
        }
    }


    /// <summary>
    /// Check if the current room is a shop or not
    /// </summary>
    /// <returns>True if it is a shop</returns>
    public bool IsItShop()
    {
        return Physics2D.OverlapBox(camPos, camSize, 0f, shopLayer);
    }

    public GameObject GiveMeMerchant()
    {
        return Physics2D.OverlapBox(camPos, camSize, 0f, shopLayer).gameObject;
    }

    public void RespawnInRoom()
    {
        float dist = 2000f;
        Vector3 tpPlace = new Vector3();
        foreach (var item in Physics2D.OverlapBoxAll(camPos, camSize, 0f, respawnLayer))    
        {
            if (Vector2.Distance(item.transform.position, player.transform.position) < dist)
            {
                dist = Vector2.Distance(item.transform.position, player.transform.position);
                tpPlace = item.transform.position;
            }
        }
        player.transform.position = tpPlace;
        HeroMovement.instance.AllowMovement(true);
        HeroStats.instance.invicibility = false;
        HeroStats.instance.TakeDamageHero(voidDamage);
        EnemiesMoveEnable(true);
    }
}
