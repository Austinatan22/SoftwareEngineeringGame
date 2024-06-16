using UnityEngine;
using System.Collections.Generic;

public class RandomItem : MonoBehaviour
{
    public List<GameObject> itemPrefabs; // Use List instead of Array for easier manipulation
    public GameObject keyItemPrefab; // Prefab for the key item
    public Transform spawnPoint; // Assign this in the Unity Editor

    public static RandomItem instance;
    private bool keyDropped = false;

    private int totalRooms;
    private int nonDropRooms = 2; // Adjust based on your game's design
    private int currentRoom = 1;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        // Ensure RoomController is initialized before this.
        totalRooms = RoomController.instance.GetTotalRoomCount();
    }

    public void SpawnItems()
    {
        int droppableRooms = totalRooms - nonDropRooms;

        GameObject itemToSpawn;

        if (currentRoom == droppableRooms && !keyDropped)
        {
            itemToSpawn = keyItemPrefab; // Spawn the key item
            keyDropped = true;
        }
        else
        {
            int itemIndex = Random.Range(0, itemPrefabs.Count);
            itemToSpawn = itemPrefabs[itemIndex];

            if (itemToSpawn == keyItemPrefab)
            {
                keyDropped = true; // Mark the key as dropped
            }
        }

        // Remove the key item from the list once it's dropped
        if (keyDropped)
        {
            itemPrefabs.Remove(keyItemPrefab);
        }

        // Use the assigned spawn point to instantiate the item
        Instantiate(itemToSpawn, spawnPoint.position, Quaternion.identity);

        Debug.Log($"Room {currentRoom}: Spawned {itemToSpawn.name} at {spawnPoint.position}");
        currentRoom++;
    }
}