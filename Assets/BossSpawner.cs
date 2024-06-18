using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BossSpawner : MonoBehaviour
{
    public GameObject bossPrefab;  // Boss prefab to spawn
    public Transform spawnLocation;  // Location where the boss will spawn

    void OnCollisionEnter2D(Collision2D other)
    {
        // Check if the collider is the player and the boss has not been spawned yet
        if (other.gameObject.CompareTag("Player"))
        {
            SpawnBoss();  // Prevents spawning the boss multiple times
        }
    }

    void SpawnBoss()
    {
        if (bossPrefab != null)
        {
            // Instantiate the boss at the spawn location
            Instantiate(bossPrefab, spawnLocation.position, Quaternion.identity);
            Debug.Log("Boss spawned!");

            Destroy(gameObject);
        }
        else
        {
            Debug.LogError("Boss prefab is not assigned!");
        }
    }
}