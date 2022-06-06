using UnityEngine;

/// <summary>
/// Script managing the tracking of the player in the camera
/// </summary>
public class CameraFollow : MonoBehaviour
{
    public static CameraFollow instance;
    //Singleton initilization
    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("There is more than one CameraFollow instance in the game !");
            return;
        }
        instance = this;
    }

    private GameObject mainCamera;
    private GameObject player;
    private RoomManager roomManager;
    [HideInInspector] public Vector2 playerCoordinates;    

    private void Start()
    {
        mainCamera = gameObject;
        player = HeroStats.instance.gameObject;
        roomManager = RoomManager.instance;
    }

    /// <summary>
    /// Change the position of the camera when the player gets out of its range and teleport the player inside of the new room
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            HeroAbility.instance.EndDash();
            if (Mathf.Abs(player.transform.position.x-mainCamera.transform.position.x)> Mathf.Abs(player.transform.position.y - mainCamera.transform.position.y))
            {
                if (player.transform.position.x - mainCamera.transform.position.x<0)
                {
                    //Right
                    player.transform.position = new Vector3(player.transform.position.x-3, player.transform.position.y,0);
                    mainCamera.transform.position = new Vector3(mainCamera.transform.position.x - 20, mainCamera.transform.position.y, -10);
                    playerCoordinates.x--;
                }
                else
                {
                    //Left
                    player.transform.position = new Vector3(player.transform.position.x + 3, player.transform.position.y, 0);
                    mainCamera.transform.position = new Vector3(mainCamera.transform.position.x + 20, mainCamera.transform.position.y, -10);
                    playerCoordinates.x++;
                }
            }
            else
            {
                if (player.transform.position.y - mainCamera.transform.position.y < 0)
                {
                    //Bottom
                    player.transform.position = new Vector3(player.transform.position.x , player.transform.position.y - 3.5f, 0);
                    mainCamera.transform.position = new Vector3(mainCamera.transform.position.x, mainCamera.transform.position.y - 12, -10);
                    playerCoordinates.y--;
                }
                else
                {
                    //Top
                    player.transform.position = new Vector3(player.transform.position.x, player.transform.position.y + 2.5f, 0);
                    mainCamera.transform.position = new Vector3(mainCamera.transform.position.x, mainCamera.transform.position.y + 12, -10);
                    playerCoordinates.y++;
                }
            }
            roomManager.ChangingRoom();
        }
    }
}
