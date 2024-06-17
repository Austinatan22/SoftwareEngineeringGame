using UnityEngine;

public class ChestSpawner : MonoBehaviour
{
    public GameObject chestPrefab;  // Assign your chest prefab in the Inspector
    public Vector3 roomCenter;  // Manually assign the center of the room or calculate it

    void Start()
    {
        SpawnChestAtCenter();
    }

    void SpawnChestAtCenter()
    {
        if (chestPrefab != null)
        {
            // Instantiate the chest at the room center
            Instantiate(chestPrefab, roomCenter, Quaternion.identity);
        }
        else
        {
            Debug.Log("Chest prefab is not assigned!");
        }
    }
}
