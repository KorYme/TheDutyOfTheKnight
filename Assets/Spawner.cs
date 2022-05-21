using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject[] monsters;

    private void Start()
    {
        if (monsters.Length == 0)
        {
            monsters = Resources.LoadAll<GameObject>("Monsters");
        }
    }

    public void Spawn()
    {
        GameObject enemy = Instantiate(monsters[Random.Range(0, monsters.Length)], transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
