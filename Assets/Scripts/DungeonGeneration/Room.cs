using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public int Width;
    public int Height;
    public int X;
    public int Y;
    private bool updatedDoors = false;

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

    void Start()
    {
        if (RoomController.instance == null)
        {
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
            }
        }

        RoomController.instance.RegisterRoom(this);
    }

    void Update()
    {
        if (name.Contains("End") && !updatedDoors)
        {
            RemoveUnconnectedDoors();
            updatedDoors = true;
            bossDoors(); // Call the bossDoors method here
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
                        // Enable left door collider if adjacent room is to the right of the end room
                        if (deltaX < 0)
                            door.gameObject.tag = "bossDoor"; // Change the door's tag to "bossDoor"
                            door.doorCollider.SetActive(true);
                        break;
                    case Door.DoorType.left:
                        // Enable right door collider if adjacent room is to the left of the end room
                        if (deltaX > 0)
                            door.gameObject.tag = "bossDoor"; // Change the door's tag to "bossDoor"
                            door.doorCollider.SetActive(true);
                        break;
                    case Door.DoorType.top:
                        // Enable bottom door collider if adjacent room is below the end room
                        if (deltaY < 0)
                            door.gameObject.tag = "bossDoor"; // Change the door's tag to "bossDoor"
                            door.doorCollider.SetActive(true);
                        break;
                    case Door.DoorType.bottom:
                        // Enable top door collider if adjacent room is above the end room
                        if (deltaY > 0)
                            door.gameObject.tag = "bossDoor"; // Change the door's tag to "bossDoor"
                            door.doorCollider.SetActive(true);
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
        if (other.tag == "Player")
        {
            RoomController.instance.OnPlayerEnterRoom(this);
        }
    }
}