using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    private HeroAbility heroAbility;
    private CameraFollow cameraFollow;
    private LevelGenerator levelGenerator;
    private GameObject player;
    private GameObject spawnCamera;
    private GameObject mainCamera;
    public GameObject[] allBosses;
    private GameObject[] allMonsters;
    private Collider2D[] allSpawner;
    private Collider2D[] theClosedDoorsHere;
    private Collider2D[] theOpenedDoorsHere;
    private Collider2D[] insideEnemies;
    private Collider2D enemiesIn;
    public bool spawnRoomOut;
    private Vector2 camPos;
    private Vector2 camSize;
    public LayerMask enemyLayer;
    public LayerMask closedDoorsMask;
    public LayerMask openedDoorsMask;
    public LayerMask spawner;

    void Start()
    {
        heroAbility = GameObject.FindGameObjectWithTag("Player").GetComponent<HeroAbility>();
        InitiateValues();
        //ActivateOrDesactivateEnemies(false);
        heroAbility.fireUnlocked = false;
        heroAbility.earthUnlocked = false;
        heroAbility.windUnlocked = false;
    }

    private void FixedUpdate()
    {
        if (!spawnRoomOut)
        {
            LadderScript();
        }
    }

    /// <summary>
    /// Initialize all the main values
    /// </summary>
    public void InitiateValues()
    {
        camSize = new Vector2(17.8162708f, 9.53441048f);
        player = GameObject.FindGameObjectWithTag("Player");
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        spawnCamera = GameObject.FindGameObjectWithTag("SpawnCamera");
        spawnCamera.SetActive(true);
        Camera.main.gameObject.SetActive(false);
        player.transform.position = new Vector3(-100, 101.5059f, 0);
        spawnRoomOut = false;
        cameraFollow = mainCamera.GetComponent<CameraFollow>();
        levelGenerator = GameObject.FindGameObjectWithTag("GameManager").GetComponent<LevelGenerator>();
        allMonsters = Resources.LoadAll<GameObject>("Monsters");
    }

    /// <summary>
    /// Check if the player is touching the ladder and is pressing E
    /// </summary>
    public void LadderScript()
    {
        if (GameObject.FindGameObjectWithTag("Ladder1").GetComponent<Collider2D>().IsTouching(GameObject.FindGameObjectWithTag("Player").GetComponent<Collider2D>()))
        {
            if (Input.GetKey(KeyCode.E))
            {
                mainCamera.SetActive(true);
                heroAbility.windUnlocked = true;
                heroAbility.earthUnlocked = true;
                heroAbility.fireUnlocked = true;
                spawnCamera.SetActive(false);
                player.transform.position = new Vector3(0, 0, 0);
                spawnRoomOut = true;
                ChangingRoom();
            }
        }
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
            //Ouvrir les portes
            OpenOrCloseTheDoors(true);
        }
        else
        {
            //Else only useful to debug
            Debug.Log("ENEMIES REMAINING");
        }
    }

    /// <summary>
    /// Check if at least one enemy is in this new room
    /// </summary>
    public void AreEnemiesIn()
    {
        camPos = mainCamera.transform.position;
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
}
