using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public GameObject[] itemPrefabs; // Array to hold item prefabs
    public Transform[] spawnPoints; // Array to hold possible spawn points

    void Start()
    {
        SpawnItems();
    }

    void SpawnItems()
    {
        // Check if the number of spawn points is less than the number of items to spawn
        if (spawnPoints.Length < 3)
        {
            Debug.LogError("Not enough spawn points to spawn items");
            return;
        }

        // Shuffle the array of spawn points to get random positions
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            Transform temp = spawnPoints[i];
            int randomIndex = Random.Range(i, spawnPoints.Length);
            spawnPoints[i] = spawnPoints[randomIndex];
            spawnPoints[randomIndex] = temp;
        }

        // Spawn 3 random items at the first 3 shuffled spawn points
        for (int i = 0; i < 3; i++)
        {
            int itemIndex = Random.Range(0, itemPrefabs.Length);
            Instantiate(itemPrefabs[itemIndex], spawnPoints[i].position, Quaternion.identity);
        }
    }
}
