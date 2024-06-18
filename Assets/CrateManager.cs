using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrateManager : MonoBehaviour
{
    private System.Random randnum = new System.Random();
    public GameObject coinPrefab;
    public float crateHealth = 2f;

    // Update is called once per frame
    void Update()
    {
        // Animation Logic
    }

    public void damageCrate()
    {
        crateHealth -= 1;

        if (crateHealth <= 0)
        {
            double randomNumber = randnum.NextDouble();

            // 30% chance to drop a coin
            if (randomNumber <= 0.7)
            {
                Instantiate(coinPrefab, transform.position, Quaternion.identity);
            }
            Destroy(gameObject);
        }
    }
}
