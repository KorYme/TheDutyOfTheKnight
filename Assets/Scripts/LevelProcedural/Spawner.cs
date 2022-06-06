using UnityEngine;

/// <summary>
/// Script managing the spawn of monsters
/// </summary>
public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject[] monsters;

    private void Start()
    {
        if (monsters.Length == 0)
        {
            monsters = Resources.LoadAll<GameObject>("Monsters");
        }
    }

    /// <summary>
    /// Choose a random monster among the list and instantiate it
    /// </summary>
    public void Spawn()
    {
        Instantiate(monsters[Random.Range(0, monsters.Length)], transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
