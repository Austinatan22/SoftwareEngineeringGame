using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyArea : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            unlock();
        }
    }

    void unlock()
    {
        // Perform the actions to unlock doors
        // For example, find specific doors and set their colliders to active
        Door[] doors = FindObjectsOfType<Door>(); // Find all doors in the scene

        foreach (Door door in doors)
        {
            if (door.tag == "bossDoor")
            {
                door.gameObject.tag = "Untagged";
                RoomController.instance.UpdateRooms();
            }
        }

        // Optionally, you can change the door's tag or perform other actions
        // such as playing an animation or sound effect
    }
}
