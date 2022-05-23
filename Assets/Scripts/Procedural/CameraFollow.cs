using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public static CameraFollow instance;

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
    [SerializeField] public Vector2 playerCoordinates;

    private void Start()
    {
        mainCamera = gameObject;
        player = HeroStats.instance.gameObject;
        roomManager = RoomManager.instance;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
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
