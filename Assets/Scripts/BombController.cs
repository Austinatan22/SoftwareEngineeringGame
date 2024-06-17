using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombController : MonoBehaviour
{
    public float initialSize = 0.1f;
    public float maxSize = 3f;
    public float growthDuration = 5f;
    public GameObject explosionPrefab; // Reference to the explosion effect prefab
    public float explosionRadius = 2f; // Base radius of the explosion
    public int explosionDamage = 1;    // Damage caused by the explosion

    private Vector2 moveDirection;
    private bool hasExploded = false;  // Flag to check if the bomb has already exploded

    private void Start()
    {
        transform.localScale = Vector3.one * initialSize;
        StartCoroutine(GrowAndExplode());
    }

    public void Initialize(Vector2 direction)
    {
        moveDirection = direction.normalized;
    }

    private IEnumerator GrowAndExplode()
    {
        float elapsed = 0f;
        Vector3 initialScale = transform.localScale;
        Vector3 targetScale = Vector3.one * maxSize;

        while (elapsed < growthDuration)
        {
            transform.localScale = Vector3.Lerp(initialScale, targetScale, elapsed / growthDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        // Ensure it reaches the max size
        transform.localScale = targetScale;

        Explode();
    }

    private void Explode()
    {
        if (hasExploded) return;  // Check if the bomb has already exploded

        hasExploded = true;  // Set the flag to true to prevent multiple explosions

        // Instantiate the explosion effect
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);

        // Find all colliders in the explosion radius
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius * transform.localScale.x);

        foreach (Collider2D nearbyObject in colliders)
        {
            // Damage enemies in the explosion radius
            if (nearbyObject.tag == "Enemy")
            {
                EnemyController enemyController = nearbyObject.GetComponent<EnemyController>();
                if (enemyController != null)
                {
                    enemyController.DamageEnemy(explosionDamage);
                }
            }

            // Damage player in the explosion radius
            if (nearbyObject.tag == "Player")
            {
                GameController.DamagePlayer(explosionDamage);
            }
        }

        // Destroy the bomb after explosion
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        // Immediate explosion upon collision with player or enemy
        if (!hasExploded && (col.tag == "Player" || col.tag == "Enemy"))
        {
            Explode();
        }
    }
}