using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

// Class to hold information about each room
public class RoomInfo
{
    public string name; // Room name
    public int X; // X-coordinate of the room
    public int Y; // Y-coordinate of the room
}

public class RoomController : MonoBehaviour
{
    public static RoomController instance; // Singleton instance of RoomController
    private ChestSpawner chestSpawner; // Reference to the ChestSpawner component
    private List<string> possibleRooms = new List<string>() { "Empty", "Basic1", "Shop" }; // List of possible room types

    RoomInfo currentLoadRoomData; // Data for the room currently being loaded
    private bool shopRoomSpawned = false; // Tracks if a shop room has been spawned

    string currentWorldName = "Basement"; // Name of the current world/level

    Room currRoom; // Current room the player is in

    Queue<RoomInfo> loadRoomQueue = new Queue<RoomInfo>(); // Queue of rooms to be loaded

    public List<Room> loadedRooms = new List<Room>(); // List of rooms that have been loaded
    bool roomOpened = true;
    bool isLoadingRoom = false; // Tracks if a room is currently being loaded
    bool spawnedBossRoom = false; // Tracks if the boss room has been spawned
    bool updatedRooms = false; // Tracks if rooms have been updated after spawning the boss room
    public int roomAmount;
    public int roomCleared;
    private bool itemSpawned;
    private Chest chest;
    public bool isKeySpawned = false;
    void Awake()
    {
        instance = this; // Set the singleton instance
    }

    void Start()
    {
        // Initially commented out room loading code
        //LoadRoom("Start", 0, 0);
        //LoadRoom("Empty", 1, 0);
        //LoadRoom("Empty", -1, 0);
        //LoadRoom("Empty", 0, 1);
        //LoadRoom("Empty", 0, -1);
    }

    void Update()
    {
        Debug.LogError(isKeySpawned);
        UpdateRoomQueue(); // Call UpdateRoomQueue every frame
    }

    public void UpdateRoomQueue()
    {
        if (isLoadingRoom)
        {
            return; // If a room is being loaded, return early
        }

        if (loadRoomQueue.Count == 0)
        {
            if (!spawnedBossRoom)
            {
                StartCoroutine(SpawnBossRoom()); // If no rooms to load and boss room not spawned, start spawning boss room
            }
            else if (spawnedBossRoom && !updatedRooms)
            {
                foreach (Room room in loadedRooms)
                {
                    room.RemoveUnconnectedDoors(); // Remove unconnected doors from each room
                }
                TriggerBossDoors(); // Ensure boss doors are triggered
                UpdateRooms(); // Update all rooms
                updatedRooms = true; // Set updatedRooms to true after updating
            }
            return;
        }

        currentLoadRoomData = loadRoomQueue.Dequeue(); // Dequeue the next room to be loaded
        isLoadingRoom = true; // Set isLoadingRoom to true

        StartCoroutine(LoadRoomRoutine(currentLoadRoomData)); // Start loading the room
    }

    IEnumerator SpawnBossRoom()
    {
        spawnedBossRoom = true; // Set spawnedBossRoom to true
        yield return new WaitForSeconds(0.5f); // Wait for 0.5 seconds

        if (loadRoomQueue.Count == 0)
        {
            Room bossRoom = loadedRooms[loadedRooms.Count - 1]; // Get the last loaded room
            Room tempRoom = new Room(bossRoom.X, bossRoom.Y); // Create a temporary room with the same coordinates
            Destroy(bossRoom.gameObject); // Destroy the original boss room
            var roomToRemove = loadedRooms.Single(r => r.X == tempRoom.X && r.Y == tempRoom.Y); // Find and remove the room from the list of loaded rooms
            loadedRooms.Remove(roomToRemove);
            LoadRoom("End", tempRoom.X, tempRoom.Y); // Load the boss room at the same coordinates
        }
    }

    public void LoadRoom(string name, int x, int y)
    {
        if (DoesRoomExist(x, y))
        {
            return; // If room already exists at coordinates, return early
        }

        RoomInfo newRoomData = new RoomInfo();
        newRoomData.name = name; // Set room name
        newRoomData.X = x; // Set room X coordinate
        newRoomData.Y = y; // Set room Y coordinate

        loadRoomQueue.Enqueue(newRoomData); // Enqueue the new room data
    }

    IEnumerator LoadRoomRoutine(RoomInfo info)
    {
        string roomName = currentWorldName + info.name; // Create the full room name

        AsyncOperation loadRoom = SceneManager.LoadSceneAsync(roomName, LoadSceneMode.Additive); // Load the room scene additively

        while (!loadRoom.isDone)
        {
            yield return null; // Wait until the room is fully loaded
        }
    }

    public void RegisterRoom(Room room)
    {
        if (!DoesRoomExist(currentLoadRoomData.X, currentLoadRoomData.Y))
        {
            room.transform.position = new Vector3(
                currentLoadRoomData.X * room.Width,
                currentLoadRoomData.Y * room.Height,
                0
            ); // Set the room's position

            room.X = currentLoadRoomData.X; // Set room X coordinate
            room.Y = currentLoadRoomData.Y; // Set room Y coordinate
            room.name = currentWorldName + "-" + currentLoadRoomData.name + " " + room.X + ", " + room.Y; // Set room name
            room.transform.parent = transform; // Set room's parent to the RoomController

            isLoadingRoom = false; // Set isLoadingRoom to false

            if (loadedRooms.Count == 0)
            {
                CameraController.instance.currRoom = room; // Set the current room in the CameraController if this is the first room
            }

            loadedRooms.Add(room); // Add the room to the list of loaded rooms
        }
        else
        {
            Destroy(room.gameObject); // Destroy the room if it already exists
            isLoadingRoom = false; // Set isLoadingRoom to false
        }
    }

    public void ResetShopSpawned()
    {
        shopRoomSpawned = false; // Reset shopRoomSpawned to false
        if (!possibleRooms.Contains("Shop"))
        {
            possibleRooms.Add("Shop"); // Add "Shop" back to the list of possible rooms if it's not already there
        }
    }

    public bool DoesRoomExist(int x, int y)
    {
        return loadedRooms.Find(item => item.X == x && item.Y == y) != null; // Check if a room exists at the given coordinates
    }

    public Room FindRoom(int x, int y)
    {
        return loadedRooms.Find(item => item.X == x && item.Y == y); // Find and return a room at the given coordinates
    }

    public string GetRandomRoomName()
    {
        string temp = possibleRooms[Random.Range(0, possibleRooms.Count)]; // Get a random room name from the list

        if (temp == "Shop" && !shopRoomSpawned)
        {
            shopRoomSpawned = true; // Set shopRoomSpawned to true if a shop room is chosen and hasn't been spawned yet
            possibleRooms.Remove("Shop"); // Remove "Shop" from the list of possible rooms
        }
        return temp; // Return the random room name
    }

    public void OnPlayerEnterRoom(Room room)
    {
        CameraController.instance.currRoom = room; // Set the current room in the CameraController
        currRoom = room; // Set the current room

        StartCoroutine(RoomCoroutine()); // Start the RoomCoroutine
    }

    public IEnumerator RoomCoroutine()
    {
        yield return new WaitForSeconds(0.2f); // Wait for 0.2 seconds
        UpdateRooms(); // Update the rooms
    }

    public void UpdateRooms()
    {
        foreach (Room room in loadedRooms)
        {
            if (currRoom != room)
            {
                EnemyController[] enemies = room.GetComponentsInChildren<EnemyController>();
                if (enemies != null)
                {
                    foreach (EnemyController enemy in enemies)
                    {
                        enemy.notInRoom = true; // Set notInRoom to true for enemies not in the current room
                    }

                    foreach (Door door in room.GetComponentsInChildren<Door>())
                    {
                        if (door.tag != "bossDoor")
                        {
                            door.doorCollider.SetActive(false); // Deactivate door colliders in rooms not currently active
                        }
                    }
                }
                else
                {
                    foreach (Door door in room.GetComponentsInChildren<Door>())
                    {
                        if (door.tag != "bossDoor")
                        {
                            door.doorCollider.SetActive(false); // Deactivate door colliders in rooms without enemies
                        }
                    }
                }
            }
            else
            {
                EnemyController[] enemies = room.GetComponentsInChildren<EnemyController>();
                if (enemies.Length > 0)
                {
                    foreach (EnemyController enemy in enemies)
                    {
                        enemy.notInRoom = false; // Set notInRoom to false for enemies in the current room
                    }

                    foreach (Door door in room.GetComponentsInChildren<Door>())
                    {
                        if (door.tag != "bossDoor" && door.tag != "Chest")
                        {
                            var spriteHandler = door.GetComponentInChildren<Animator>();
                            if (spriteHandler != null)
                            {
                                spriteHandler.SetBool("isOpen", true);
                            }
                            door.doorCollider.SetActive(true); // Activate door colliders in the current room
                        }
                        else if (door.tag == "bossDoor")
                        {
                            door.spriteHandler.SetActive(false);
                            door.bossDoor.SetActive(true);
                            var spriteHandler = door.GetComponentInChildren<Animator>();
                            if (spriteHandler != null)
                            {
                                spriteHandler.SetBool("isOpen", true);
                            }
                            door.doorCollider.SetActive(true); // Activate door colliders in the current room
                        }
                    }
                }
                else
                {
                    roomCleared++;
                    Debug.LogError("Cleared: " + roomCleared);
                    foreach (Door door in room.GetComponentsInChildren<Door>())
                    {
                        if (door.tag == "Chest")
                        {
                            door.doorCollider.SetActive(true);
                            door.keyArea.SetActive(true);
                        }
                        else if (door.tag != "bossDoor")
                        {
                            var spriteHandler = door.GetComponentInChildren<Animator>();
                            if (spriteHandler != null)
                            {
                                spriteHandler.SetBool("isOpen", false);
                            }
                            door.doorCollider.SetActive(false); // Deactivate door colliders in rooms without enemies
                        }
                    }
                }
            }
        }
    }

    // Method to trigger boss doors in the end room
    public void TriggerBossDoors()
    {
        foreach (Room room in loadedRooms)
        {
            if (room.name.Contains("End"))
            {
                room.bossDoors(); // Call bossDoors function for the end room
            }
        }
    }
    public int GetTotalRoomCount()
    {
        return loadedRooms.Count;
    }
}