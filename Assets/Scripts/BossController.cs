using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BossState
{
    Idle,
    Normal,
    SpecialAttack,
    Die
};

public class BossController : MonoBehaviour
{
    public Transform player;
    public BossState currState = BossState.Idle;
    public float detectionRange = 10f;
    public float speed = 5f;
    public float specialAttackDuration = 10f;
    public int health = 100;
    private bool isSpecialAttack = false;
    private float specialAttackTimer = 0f;
    private float stateDuration = 10f; // Duration for each state
    private Vector3 moveDirection = Vector3.right;
    private float changeDirectionTime = 1f;
    public bool notInRoom = false;
    private float originalSpeed;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        originalSpeed = speed;
    }

    void Update()
    {
        switch (currState)
        {
            case BossState.Normal:
                ApproachPlayer();
                break;
            case BossState.SpecialAttack:
                SpecialAttack();
                break;
            case BossState.Die:
                // Possibly play some death animation or effects
                break;
        }
        ManageState();
        if (IsPlayerInRange(detectionRange) && currState != BossState.Die)
        {
            currState = BossState.Normal;
            speed = originalSpeed;
        }
        else
        {
            currState = BossState.Idle;
            speed = originalSpeed;
        }
    }

    private bool IsPlayerInRange(float range)
    {
        return Vector3.Distance(transform.position, player.transform.position) <= detectionRange;
    }

    void ApproachPlayer()
    {
        transform.position = Vector3.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
    }

    void SpecialAttack()
    {
        if (changeDirectionTime <= 0)
        {
            speed = originalSpeed * 2;
            // Randomly choose between horizontal and vertical movements
            if (Random.value > 0.5f)
            {
                moveDirection = moveDirection == Vector3.right ? Vector3.left : Vector3.right;
            }
            else
            {
                moveDirection = moveDirection == Vector3.up ? Vector3.down : Vector3.up;
            }
            changeDirectionTime = 2.5f; // Change direction every second
        }
        else
        {
            changeDirectionTime -= Time.deltaTime;
        }
        transform.position += moveDirection * (speed * 1.5f * Time.deltaTime);
    }

    void ManageState()
    {
        if (currState != BossState.Die)
        {
            specialAttackTimer += Time.deltaTime;
            if (specialAttackTimer >= stateDuration)
            {
                isSpecialAttack = !isSpecialAttack;
                specialAttackTimer = 0f;
                currState = isSpecialAttack ? BossState.SpecialAttack : BossState.Normal;
            }
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            currState = BossState.Die;
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Boss defeated!");
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GameController.DamagePlayer(20); // Assuming the attack damage is 20
        }
    }
}