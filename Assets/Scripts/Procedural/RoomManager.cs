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
    private CameraFollow cameraFollow;
    private LevelGenerator levelGenerator;
    private GameObject player;
    private GameObject spawnCamera;
    private GameObject mainCamera;
    private GameObject[] allMonsters;
    private Collider2D[] allSpawner;


    [Header ("Filling values")]
    public InputData inputData;
    public GameObject[] allBosses;

    [Header ("Layers to fill")]
    public LayerMask enemyLayer;
    public LayerMask closedDoorsMask;
    public LayerMask openedDoorsMask;
    public LayerMask spawner;   
    public LayerMask shopLayer;

    private Collider2D[] theClosedDoorsHere;
    private Collider2D[] theOpenedDoorsHere;
    private Collider2D[] insideEnemies;
    private Collider2D enemiesIn;
    private Collider2D ladder1;
    [HideInInspector]public bool spawnRoomOut;
    private Vector2 camPos;
    private Vector2 camSize;

    void Start()
    {
        InitiateValues();
    }

    /// <summary>
    /// Initialize all the main values
    /// </summary>
    public void InitiateValues()
    {
        allMonsters = Resources.LoadAll<GameObject>("Monsters");
        player = GameObject.FindGameObjectWithTag("Player");
        heroAbility = player.GetComponent<HeroAbility>();
        levelGenerator = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelGenerator>();
        camSize = new Vector2(17.8162708f, 9.53441048f);
        cameraFollow = Camera.main.GetComponent<CameraFollow>();
        player.transform.position = new Vector3(0, 0, 0);
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
        ActivateEnemies();
        AreEnemiesIn();
    }


    public void ActivateEnemies()
    {
        allSpawner = Physics2D.OverlapBoxAll(camPos, camSize, 0f, spawner);
        foreach (var item in allSpawner)
        {
            if (item.tag == "SpawnBoss")
            {
                GameObject Boss = Instantiate(allBosses[Random.Range(0, allBosses.Length)], item.transform.position, Quaternion.identity);
                Destroy(item.gameObject);
            }
            else
            {
                GameObject enemy = Instantiate(allMonsters[Random.Range(0, allMonsters.Length)], item.transform.position, Quaternion.identity);
                Destroy(item.gameObject);
            }
        }
    }

    /// <summary>
    /// Open or close the doors
    /// </summary>
    /// <param name="that">If true open the doors else close them</param>
    private void OpenOrCloseTheDoors(bool that)
    {
        //And desactivate the chosen ones
        List<string> cardinalsPoints = levelGenerator.WhichRoundAround((int)CameraFollow.playerCoordinates.x, (int)CameraFollow.playerCoordinates.y);
        if (that)
        {
            //Door Sound
            //Ouvre seulement les portes découlants vers d'autres salles
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
            //Open the doors
            OpenOrCloseTheDoors(true);
            //Add Opening door sound
        }
    }

    /// <summary>
    /// Check if at least one enemy is in this new room
    /// </summary>
    public void AreEnemiesIn()
    {
        camPos = Camera.main.transform.position;
        //ActivateOrDesactivateEnemies(true);
        insideEnemies = Physics2D.OverlapBoxAll(camPos, camSize, 0f, enemyLayer);
        //ActivateOrDesactivateEnemies(false);
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

    public bool IsItShop()
    {
        return Physics2D.OverlapBox(camPos, camSize, 0f, shopLayer);
    }
}
