using System.Collections;
using UnityEngine;

public class BossController : MonoBehaviour
{
    public enum BossState { Idle, Moving, Attack }

    public float moveSpeed = 5f;
    public float attackSpeed = 8f;
    public float health;
    public float detectionRange = 17f;
    public Transform player;
    private Vector2 moveDirection = new Vector2(1, 1); // Initial diagonal movement
    private BossState currState = BossState.Moving;
    private Rigidbody2D rb;
    private Vector2 lastPlayerPosition;
    private float attackTimer = 10f;
    public float attackCooldown = 5f;
    private float lastAttackTime = -10f;


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        StartCoroutine(TrackPlayerPosition());
    }

    void Update()
    {
        float distance = Vector3.Distance(transform.position, player.position);
        Debug.Log($"Current State: {currState}, Distance: {distance}");

        switch (currState)
        {
            case BossState.Moving:
                MoveDiagonally();
                break;

            case BossState.Attack:
                if (distance > detectionRange || Time.time >= lastAttackTime + attackCooldown)
                {
                    currState = distance <= detectionRange ? BossState.Moving : BossState.Idle;
                    rb.velocity = currState == BossState.Moving ? moveDirection.normalized * moveSpeed : Vector2.zero;
                }
                break;
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

        transform.position = lastPlayerPosition;
        currState = BossState.Moving;
        MoveDiagonally();
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collider)
    {
        if ((collider.gameObject.CompareTag("Wall") || collider.gameObject.name.Contains("Wall")) && (currState == BossState.Moving || currState == BossState.Attack))
        {
            ContactPoint2D contact = collider.contacts[0];
            moveDirection = Vector2.Reflect(moveDirection, contact.normal);
            rb.velocity = moveDirection.normalized * moveSpeed;
        }

        if (collider.gameObject.CompareTag("Player"))
        {
            GameController.DamagePlayer(1);
            rb.velocity = moveDirection.normalized * moveSpeed;
        }
    }
}
