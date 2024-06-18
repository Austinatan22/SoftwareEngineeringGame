using System.Collections.Generic;
using NavMeshPlus.Components;
using UnityEngine;

public class Room : MonoBehaviour
{
    public int Width;
    public int Height;
    public int X;
    public int Y;
    private bool updatedDoors = false;
    public NavMeshSurface navMeshSurface;

    private static HashSet<int> bakedRooms = new HashSet<int>();
    private static HashSet<int> spawnedRooms = new HashSet<int>();
    private static Room currentRoom;

    public Room(int x, int y)
    {
        X = x;
        Y = y;
    }

    public Door leftDoor;
    public Door rightDoor;
    public Door topDoor;
    public Door bottomDoor;
    public List<Door> doors = new List<Door>();

    public List<Transform> enemySpawnPoints = new List<Transform>();
    public Door Chest;

    public GameObject enemyPrefab;

    void Start()
    {
        if (RoomController.instance == null)
        {
            Debug.LogWarning("RoomController instance is null. Exiting Start method.");
            return;
        }

        Door[] ds = GetComponentsInChildren<Door>();
        foreach (Door d in ds)
        {
            doors.Add(d);
            switch (d.doorType)
            {
                case Door.DoorType.right:
                    rightDoor = d;
                    break;
                case Door.DoorType.left:
                    leftDoor = d;
                    break;
                case Door.DoorType.top:
                    topDoor = d;
                    break;
                case Door.DoorType.bottom:
                    bottomDoor = d;
                    break;
                case Door.DoorType.chest:
                    bottomDoor = d;
                    break;
            }
        }

        RoomController.instance.RegisterRoom(this);

        GetEnemySpawnPoints(); // Ensure spawn points are populated
    }

    void Update()
    {
        if (name.Contains("End") && !updatedDoors)
        {
            RemoveUnconnectedDoors();
            updatedDoors = true;
            bossDoors();
        }
    }

    void BakeNavMesh()
    {
        if (navMeshSurface != null)
        {
            navMeshSurface.BuildNavMesh();
            Debug.Log("NavMesh baked for room at position: " + X + ", " + Y);
        }
        else
        {
            Debug.LogWarning("NavMeshSurface is not assigned.");
        }
    }

    void ClearNavMesh()
    {
        if (navMeshSurface != null)
        {
            navMeshSurface.navMeshData = null; // Clear the NavMesh data
        }
    }

    public void RemoveUnconnectedDoors()
    {
        foreach (Door door in doors)
        {
            switch (door.doorType)
            {
                case Door.DoorType.right:
                    if (GetRight() == null)
                        door.gameObject.SetActive(false);
                    door.doorCollider.SetActive(true);
                    break;
                case Door.DoorType.left:
                    if (GetLeft() == null)
                        door.gameObject.SetActive(false);
                    door.doorCollider.SetActive(true);
                    break;
                case Door.DoorType.top:
                    if (GetTop() == null)
                        door.gameObject.SetActive(false);
                    door.doorCollider.SetActive(true);
                    break;
                case Door.DoorType.bottom:
                    if (GetBottom() == null)
                        door.gameObject.SetActive(false);
                    door.doorCollider.SetActive(true);
                    break;
                case Door.DoorType.chest:
                    break;
            }
        }
    }

    public Room GetRight()
    {
        if (RoomController.instance.DoesRoomExist(X + 1, Y))
        {
            return RoomController.instance.FindRoom(X + 1, Y);
        }
        return null;
    }

    public Room GetLeft()
    {
        if (RoomController.instance.DoesRoomExist(X - 1, Y))
        {
            return RoomController.instance.FindRoom(X - 1, Y);
        }
        return null;
    }

    public Room GetTop()
    {
        if (RoomController.instance.DoesRoomExist(X, Y + 1))
        {
            return RoomController.instance.FindRoom(X, Y + 1);
        }
        return null;
    }

    public Room GetBottom()
    {
        if (RoomController.instance.DoesRoomExist(X, Y - 1))
        {
            return RoomController.instance.FindRoom(X, Y - 1);
        }
        return null;
    }

    public List<Room> FindAdjacentRoomsToEnd()
    {
        List<Room> adjacentRooms = new List<Room>();

        if (name.Contains("End"))
        {
            Room rightRoom = GetRight();
            if (rightRoom != null) adjacentRooms.Add(rightRoom);

            Room leftRoom = GetLeft();
            if (leftRoom != null) adjacentRooms.Add(leftRoom);

            Room topRoom = GetTop();
            if (topRoom != null) adjacentRooms.Add(topRoom);

            Room bottomRoom = GetBottom();
            if (bottomRoom != null) adjacentRooms.Add(bottomRoom);
        }
        return adjacentRooms;
    }

    public void bossDoors()
    {
        List<Room> adjacentRooms = FindAdjacentRoomsToEnd();
        foreach (Room room in adjacentRooms)
        {
            int deltaX = room.X - X;
            int deltaY = room.Y - Y;

            foreach (Door door in room.doors)
            {
                switch (door.doorType)
                {
                    case Door.DoorType.right:
                        if (deltaX < 0) // Adjacent room is to the right
                        {
                            door.gameObject.tag = "bossDoor"; // Change the door's tag to "bossDoor"
                            door.doorCollider.SetActive(true);
                            if (door.keyArea != null)
                            {
                                door.keyArea.SetActive(true); // Activate the keyArea
                            }
                            if (door.bossDoor != null)
                            {
                                door.bossDoor.SetActive(true);
                                door.spriteHandler.SetActive(false);
                            }
                        }
                        break;
                    case Door.DoorType.left:
                        if (deltaX > 0) // Adjacent room is to the left
                        {
                            door.gameObject.tag = "bossDoor"; // Change the door's tag to "bossDoor"
                            door.doorCollider.SetActive(true);
                            if (door.keyArea != null)
                            {
                                door.keyArea.SetActive(true); // Activate the keyArea
                            }
                            if (door.bossDoor != null)
                            {
                                door.bossDoor.SetActive(true);
                                door.spriteHandler.SetActive(false);
                            }
                        }
                        break;
                    case Door.DoorType.top:
                        if (deltaY < 0) // Adjacent room is above
                        {
                            door.gameObject.tag = "bossDoor"; // Change the door's tag to "bossDoor"
                            door.doorCollider.SetActive(true);
                            if (door.keyArea != null)
                            {
                                door.keyArea.SetActive(true); // Activate the keyArea
                            }
                            if (door.bossDoor != null)
                            {
                                door.bossDoor.SetActive(true);
                                door.spriteHandler.SetActive(false);
                            }
                        }
                        break;
                    case Door.DoorType.bottom:
                        if (deltaY > 0) // Adjacent room is below
                        {
                            door.gameObject.tag = "bossDoor"; // Change the door's tag to "bossDoor"
                            door.doorCollider.SetActive(true);
                            if (door.keyArea != null)
                            {
                                door.keyArea.SetActive(true); // Activate the keyArea
                            }
                            if (door.bossDoor != null)
                            {
                                door.bossDoor.SetActive(true);
                                door.spriteHandler.SetActive(false);
                            }
                        }
                        break;
                    case Door.DoorType.chest:
                        break;
                }
            }
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, new Vector3(Width, Height, 0));
    }

    public Vector3 GetRoomCentre()
    {
        return new Vector3(X * Width, Y * Height);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            RoomController.instance.OnPlayerEnterRoom(this);

            int roomHash = GetRoomHash();
            if (navMeshSurface != null && !bakedRooms.Contains(roomHash))
            {
                BakeNavMesh();
                bakedRooms.Add(roomHash);
            }

            if (!spawnedRooms.Contains(roomHash))
            {
                SpawnEnemies();
                spawnedRooms.Add(roomHash);
            }
            else
            {
                Debug.Log("Enemies already spawned in this room: " + name);
            }

            // if (currentRoom != null && currentRoom != this)
            // {
            //     currentRoom.ClearNavMesh(); // Clear NavMesh of the previous room
            // }

            currentRoom = this; // Set this room as the current room
        }
    }

    // void OnTriggerExit2D(Collider2D other)
    // {
    //     if (other.CompareTag("Player"))
    //     {
    //         ClearNavMesh(); // Clear NavMesh when the player exits the room
    //     }
    // }

    void GetEnemySpawnPoints()
    {
        enemySpawnPoints.Clear(); // Clear previous data to avoid duplication
        Transform[] spawnPoints = GetComponentsInChildren<Transform>();
        foreach (Transform spawnPoint in spawnPoints)
        {
            if (spawnPoint.CompareTag("EnemySpawnPoint"))
            {
                enemySpawnPoints.Add(spawnPoint);
            }
        }
        if (enemySpawnPoints.Count == 0)
        {
            Debug.LogWarning("No enemy spawn points found in room: " + name);
        }
    }

    public void SpawnEnemies()
    {
        if (enemyPrefab == null)
        {
            Debug.LogWarning("Enemy prefab is not assigned.");
            return;
        }

        if (enemySpawnPoints.Count == 0)
        {
            Debug.LogWarning("No enemy spawn points found in room: " + name);
            return;
        }

        foreach (Transform spawnPoint in enemySpawnPoints)
        {
            Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
            Debug.Log("Spawned enemy at position: " + spawnPoint.position);
        }
    }

    int GetRoomHash()
    {
        // Using a combination that spreads out values more distinctly
        int hash = 17;
        hash = hash * 23 + X.GetHashCode();
        hash = hash * 23 + Y.GetHashCode();
        return hash;
    }
}
