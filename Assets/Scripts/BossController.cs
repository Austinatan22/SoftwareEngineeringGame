using System.Collections;
using UnityEngine;

public class BossController : MonoBehaviour
{
    public enum BossState { Idle, Moving, Attack }

    public float moveSpeed = 5f;
    public float attackSpeed = 8f;

    public float health;
    public float detectionRange = 15f;
    public Transform player;
    private Vector2 moveDirection = new Vector2(1, 1); // Initial diagonal movement
    private BossState currState = BossState.Idle;
    private Rigidbody2D rb;
    private Vector2 lastPlayerPosition;
    private float attackTimer = 10f;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        StartCoroutine(TrackPlayerPosition());
    }

    private void Update()
    {
        float distance = Vector3.Distance(transform.position, player.position);

        if (distance > detectionRange)
        {
            currState = BossState.Idle;
            rb.velocity = Vector2.zero;
        }
        else if (distance <= detectionRange && currState != BossState.Attack)
        {
            currState = BossState.Moving;
            MoveDiagonally();
        }
    }

    private void MoveDiagonally()
    {
        rb.velocity = moveDirection.normalized * moveSpeed;
    }

    private IEnumerator TrackPlayerPosition()
    {
        while (true)
        {
            yield return new WaitForSeconds(attackTimer);
            if (currState == BossState.Moving)
            {
                // Record last position and initiate attack
                lastPlayerPosition = player.position;
                StartCoroutine(MoveToPlayerPosition());
            }
        }
    }

    private IEnumerator MoveToPlayerPosition()
    {
        currState = BossState.Attack;
        float timeToReach = Vector2.Distance(transform.position, lastPlayerPosition) / attackSpeed;
        float elapsedTime = 0;

        while (elapsedTime < timeToReach)
        {
            rb.velocity = (lastPlayerPosition - (Vector2)transform.position).normalized * attackSpeed;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the boss reaches the exact spot (if needed, to handle floating-point imprecision)
        transform.position = lastPlayerPosition;
        currState = BossState.Moving; // Return to moving state
        MoveDiagonally(); // Continue moving diagonally
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    // private void OnCollisionEnter2D(Collision2D collision)
    // {
    //     if ((collision.gameObject.CompareTag("Wall") || collision.gameObject.name.Contains("Wall")) && currState == BossState.Moving)
    //     {
    //         // Reflect the movement direction based on the collision normal
    //         ContactPoint2D contact = collision.contacts[0]; // Get the first contact point
    //         moveDirection = Vector2.Reflect(moveDirection, contact.normal);
    //         rb.velocity = moveDirection.normalized * moveSpeed;
    //     }
    // }
    private void OnCollisionEnter2D(Collision2D collider)
    {
        // Check for collision with walls to reflect the movement direction, only when in Moving state
        if ((collider.gameObject.CompareTag("Wall") || collider.gameObject.name.Contains("Wall")) && currState == BossState.Moving)
        {
            // This needs a different handling since OnTrigger doesn't have contact points for normal calculation
            // For now, let's just invert the direction or handle it in another way suited to your game design
            // moveDirection = -moveDirection;
            // rb.velocity = moveDirection.normalized * moveSpeed;
            ContactPoint2D contact = collider.contacts[0]; // Get the first contact point
            moveDirection = Vector2.Reflect(moveDirection, contact.normal);
            rb.velocity = moveDirection.normalized * moveSpeed;
        }

        // Check for collision with player
        if (collider.gameObject.CompareTag("Player"))
        {
            GameController.DamagePlayer(1);  // Call damage function

            // Ensure shark continues moving in its current direction after 'hitting' the player
            if (currState == BossState.Attack || currState == BossState.Moving)
            {
                rb.velocity = moveDirection.normalized * moveSpeed;  // Continue moving in the original direction
            }
        }
    }

}
