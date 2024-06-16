using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyArea : MonoBehaviour
{
    private bool playerInArea = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && CollectionController.isKeyAcquired == true)
        {
            playerInArea = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            playerInArea = false;
        }
    }

    void Update()
    {
        if (playerInArea && Input.GetKeyDown(KeyCode.F))
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
                door.doorCollider.SetActive(false); // Unlock the door by deactivating the collider
            }
        }

        // Call the UpdateRooms method from RoomController
        RoomController.instance.UpdateRooms();

        // Optionally, you can change the door's tag or perform other actions
        // such as playing an animation or sound effect
    }
}
