using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private GameObject mainCamera;
    private GameObject player;
    private HeroStats heroStats;
    static public Vector2 playerCoordinates;
    public RoomManager roomManager;

    private void Start()
    {
        mainCamera = this.gameObject;
        player = GameObject.FindGameObjectWithTag("Player");
        heroStats = player.GetComponent<HeroStats>();
        roomManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<RoomManager>();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (Mathf.Abs(player.transform.position.x-mainCamera.transform.position.x)> Mathf.Abs(player.transform.position.y - mainCamera.transform.position.y))
            {
                if (player.transform.position.x - mainCamera.transform.position.x<0)
                {
                    //Droite
                    player.transform.position = new Vector3(player.transform.position.x-3, player.transform.position.y,0);
                    mainCamera.transform.position = new Vector3(mainCamera.transform.position.x - 20, mainCamera.transform.position.y, -10);
                    playerCoordinates.x--;
                }
                else
                {
                    //Gauche
                    player.transform.position = new Vector3(player.transform.position.x + 3, player.transform.position.y, 0);
                    mainCamera.transform.position = new Vector3(mainCamera.transform.position.x + 20, mainCamera.transform.position.y, -10);
                    playerCoordinates.x++;
                }
            }
            else
            {
                if (player.transform.position.y - mainCamera.transform.position.y < 0)
                {
                    //Bas
                    player.transform.position = new Vector3(player.transform.position.x , player.transform.position.y - 3.5f, 0);
                    mainCamera.transform.position = new Vector3(mainCamera.transform.position.x, mainCamera.transform.position.y - 12, -10);
                    playerCoordinates.y--;
                }
                else
                {
                    //Haut
                    player.transform.position = new Vector3(player.transform.position.x, player.transform.position.y + 2.5f, 0);
                    mainCamera.transform.position = new Vector3(mainCamera.transform.position.x, mainCamera.transform.position.y + 12, -10);
                    playerCoordinates.y++;
                }
            }
            roomManager.ChangingRoom();
        }
    }
}
