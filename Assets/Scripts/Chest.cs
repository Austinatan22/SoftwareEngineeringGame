using UnityEngine;

public class Chest : MonoBehaviour
{
    public GameObject[] itemPrefabs;  // Array of item prefabs to spawn
    public GameObject keyPrefab;      // Prefab for the key item
    public Transform spawnPoint;      // The point where items will spawn
    private RoomController roomController;  // Reference to the RoomController
    private bool playerInRange = false;     // Flag to check if the player is in range
    private bool itemSpawned = false;       // Flag to check if an item has been spawned
    public bool isKeySpawned = false;

    void Start()
    {
        roomController = RoomController.instance;  // Get the RoomController instance
    }

    void Update()
    {
        if (playerInRange && !itemSpawned)
        {
            TrySpawnItem();
        }
    }

    public void TrySpawnItem()
    {
        if (roomController == null)
        {
            Debug.LogError("RoomController not found!");
            return;
        }

        int totalRooms = roomController.GetTotalRoomCount();
        Debug.LogError(totalRooms);
        Debug.LogError(roomController.roomCleared);

        // Check if the pity system should trigger
        if (roomController.roomCleared >= totalRooms - 4 && roomController.isKeySpawned == false)
        {
            SpawnKey();
            roomController.isKeySpawned = true;
        }
        else
        {
            Debug.LogError(totalRooms);
            SpawnRandomItem();
        }
        Debug.LogError("KeySpawned: " + isKeySpawned);
        itemSpawned = true;  // Set the itemSpawned flag to true after spawning an item
    }

    void SpawnRandomItem()
    {
        if (itemPrefabs.Length == 0)
        {
            Debug.LogError("No item prefabs assigned!");
            return;
        }

        int index = Random.Range(0, itemPrefabs.Length);
        Instantiate(itemPrefabs[index], spawnPoint.position, Quaternion.identity);
    }

    void SpawnKey()
    {
        if (keyPrefab != null)
        {
            Instantiate(keyPrefab, spawnPoint.position, Quaternion.identity);
        }
        else
        {
            Debug.LogError("Key prefab is not assigned!");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }
}
