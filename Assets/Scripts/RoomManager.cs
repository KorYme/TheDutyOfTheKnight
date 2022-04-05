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
    private GameObject[] allEnemies;
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

    void Start()
    {
        heroAbility = GameObject.FindGameObjectWithTag("Player").GetComponent<HeroAbility>();
        InitiateValues();
        ActivateOrDesactivateEnemies(false);
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
        camSize = new Vector2(19.1207638f, 10.7610741f);
        player = GameObject.FindGameObjectWithTag("Player");
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        spawnCamera = GameObject.FindGameObjectWithTag("SpawnCamera");
        spawnCamera.SetActive(true);
        mainCamera.SetActive(false);
        player.transform.position = new Vector3(-100, 101.5059f, 0);
        allEnemies = GameObject.FindGameObjectsWithTag("Enemies");
        spawnRoomOut = false;
        cameraFollow = mainCamera.GetComponent<CameraFollow>();
        levelGenerator = GameObject.FindGameObjectWithTag("GameManager").GetComponent<LevelGenerator>();
    }

    /// <summary>
    /// Activate every enemy if true else desactivate them
    /// </summary>
    /// <param name="that">Activate the monsters if true or not if false</param>
    public void ActivateOrDesactivateEnemies(bool that)
    {
        foreach (var enemy in allEnemies)
        {
            if (enemy != null)
            {
                enemy.SetActive(that);
            }
        }
    }

    /// <summary>
    /// Open or close the doors
    /// </summary>
    /// <param name="that">If true open the doors else close them</param>
    private void OpenOrCloseTheDoors(bool that)
    {
        //Take all the doors in this room
        theOpenedDoorsHere = Physics2D.OverlapBoxAll(camPos, camSize, 0f, openedDoorsMask);
        theClosedDoorsHere = Physics2D.OverlapBoxAll(camPos, camSize, 0f, closedDoorsMask);
        //And desactivate the chosen ones
        List<string> cardinalsPoints = levelGenerator.WhichRoundAround((int)CameraFollow.playerCoordinates.x, (int)CameraFollow.playerCoordinates.y);
        if (that)
        {
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
                Debug.Log(item.name);
                item.gameObject.SetActive(false);
            }
            foreach (var item in theClosedDoorsHere)
            {
                item.gameObject.SetActive(true);
            }
        }
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
                AreEnemiesIn();
            }
        }
    }

    /// <summary>
    /// Check if at least one enemy is still in this room
    /// </summary>
    public void CheckEnemiesStillIn()
    {
        camPos = mainCamera.transform.position;
        enemiesIn = Physics2D.OverlapBox(camPos, camSize, 0f, enemyLayer);
        if (enemiesIn == null)
        {
            //Ouvrir les portes
            OpenOrCloseTheDoors(true);
            Debug.Log("DOORS OPEN");
            //Add a opening doors sound
        }
        else
        {
            //Else uniquement utile pour debug
            Debug.Log("ENEMIES REMAINING");
        }
    }

    /// <summary>
    /// Check if at least one enemy is in this new room
    /// </summary>
    public void AreEnemiesIn()
    {
        camPos = mainCamera.transform.position;
        ActivateOrDesactivateEnemies(true);
        insideEnemies = Physics2D.OverlapBoxAll(camPos, camSize, 0f, enemyLayer);
        ActivateOrDesactivateEnemies(false);
        if (insideEnemies.Length == 0)
        {
            //Debug.Log("NO ENEMY DETECTED");
            //Debug.Log("DOORS OPEN");
            OpenOrCloseTheDoors(true);
        }
        else
        {
            //Debug.Log("ENEMY DETECTED");
            foreach (var item in insideEnemies)
            {
                item.gameObject.SetActive(true);
            }
            OpenOrCloseTheDoors(false);
        }
    }
}
