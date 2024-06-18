using UnityEditor;
using UnityEngine;

public class BossController : MonoBehaviour
{
    public enum BossState { Idle, Attack, Die }

    public float moveSpeed = 5f;
    public float attackRange = 10f;
    public Transform player;
    public int health = 100;

    private Vector3 moveDirection = new Vector3(1, 1, 0); // Initial diagonal movement
    private BossState currState = BossState.Idle;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer; // Reference to the sprite renderer

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>(); // Get the SpriteRenderer component
    }

    private void Update()
    {
        switch (currState)
        {
            case BossState.Idle:
                CheckPlayerInRange();
                break;
            case BossState.Attack:
                MoveDiagonally();
                FlipSprite();
                break;
            case BossState.Die:
                // Handle death state, possibly do nothing
                break;
        }
    }

    private void CheckPlayerInRange()
    {
        if (Vector3.Distance(transform.position, player.position) <= attackRange && currState != BossState.Die)
        {
            StartAttack();
        }
    }

    private void StartAttack()
    {
        currState = BossState.Attack;
        // Additional attack initialization can go here
    }

    private void MoveDiagonally()
    {
        // We're setting the velocity directly for continuous movement
        rb.velocity = moveDirection.normalized * moveSpeed;
    }

    private void FlipSprite()
    {
        // Flip the sprite vertically when moving left
        if (moveDirection.x < 0)
            spriteRenderer.flipY = true;
        else
            spriteRenderer.flipY = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall") || collision.gameObject.name.Contains("Wall"))
        {
            // Reflect the move direction based on the collision normal
            moveDirection = Vector3.Reflect(moveDirection, collision.contacts[0].normal);
            rb.velocity = moveDirection.normalized * moveSpeed; // Reapply the new velocity
        }
    }

    public void TakeDamage(int damage)
    {
        if (currState != BossState.Die)
        {
            health -= damage;
            if (health <= 0)
            {
                currState = BossState.Die;
                Die();
            }
        }
    }

    private void Die()
    {
        // Handle the boss's death (play animation, destroy object, etc.)
        Destroy(gameObject);
    }
}
