using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public enum EnemyState
{
    Idle,
    Wander,
    Follow,
    Die,
    Attack
};

public enum EnemyType
{
    Melee,
    Ranged
};

public class EnemyController : MonoBehaviour
{
    [SerializeField] Transform target;
    NavMeshAgent agent;
    GameObject player;
    public EnemyState currState = EnemyState.Idle;
    public EnemyType enemyType;
    public float range;
    public float speed;
    public float attackRange;
    public float bulletSpeed;
    public float coolDown;
    public float health = 3;
    public float Health { get => health; set => health = value; }
    private bool chooseDir = false;
    private bool dead = false;
    private bool coolDownAttack = false;
    public bool notInRoom = false;
    private Vector3 randomDir;
    public GameObject bulletPrefab;
    private float originalSpeed; // Store the original speed
    public GameObject coinPrefab; // Assign the coin prefab in the inspector
    private System.Random randnum = new System.Random();

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        player = GameObject.FindGameObjectWithTag("Player");
        originalSpeed = speed; // Initialize the original speed
    }
    void Update()
    {
        if (Health <= 0) return; // Early exit if the enemy is dead
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        switch (currState)
        {
            case EnemyState.Idle:
                if (distanceToPlayer <= range)
                    currState = EnemyState.Follow;
                break;
            case EnemyState.Follow:
                Follow();
                if (distanceToPlayer <= attackRange)
                    currState = EnemyState.Attack;
                else if (distanceToPlayer > range)
                    currState = EnemyState.Wander;
                break;
            case EnemyState.Attack:
                Attack();
                if (distanceToPlayer > attackRange)
                    currState = EnemyState.Follow;
                break;
            case EnemyState.Wander:
                Wander();
                if (distanceToPlayer <= range)
                    currState = EnemyState.Follow;
                break;
        }

        if (!notInRoom)
        {
            if (IsPlayerInRange(range) && currState != EnemyState.Die)
            {
                currState = EnemyState.Follow;
                speed = originalSpeed; // Restore speed when following the player
            }
            else if (!IsPlayerInRange(range) && currState != EnemyState.Die)
            {
                currState = EnemyState.Wander;
            }
            if (Vector3.Distance(transform.position, player.transform.position) <= attackRange)
            {
                currState = EnemyState.Attack;
            }
        }
        else
        {
            currState = EnemyState.Idle;
        }
    }

    private bool IsPlayerInRange(float range)
    {
        float sqrRange = range * range;  // Square the range for comparison
        return (transform.position - player.transform.position).sqrMagnitude <= sqrRange;
    }

    private IEnumerator ChooseDirection()
{
    chooseDir = true;
    yield return new WaitForSeconds(Random.Range(2f, 8f));
    chooseDir = false;
}

    void Wander()
    {
        if (!chooseDir)
        {
            StartCoroutine(ChooseDirection());
        }

        transform.position += -transform.right * speed * Time.deltaTime;
        if (IsPlayerInRange(range))
        {
            currState = EnemyState.Follow;
            speed = originalSpeed; // Restore speed when following the player
        }
    }

    void Follow()
    {
        if (player != null)
            agent.SetDestination(player.transform.position);
    }

    void Attack()
    {
        if (!coolDownAttack)
        {
            switch (enemyType)
            {
                case (EnemyType.Melee):
                    GameController.DamagePlayer(1);
                    StartCoroutine(CoolDown());
                    break;
                case (EnemyType.Ranged):
                    GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity) as GameObject;
                    bullet.GetComponent<BulletController>().GetPlayer(player.transform);
                    bullet.AddComponent<Rigidbody2D>().gravityScale = 0;
                    bullet.GetComponent<BulletController>().isEnemyBullet = true;
                    StartCoroutine(CoolDown());
                    break;
            }
        }
    }

    private IEnumerator CoolDown()
    {
        coolDownAttack = true;
        yield return new WaitForSeconds(coolDown);
        coolDownAttack = false;
    }
    public void DamageEnemy(int damage)
    {
        health -= damage;

        if (Health <= 0)
        {
            double randomNumber = randnum.NextDouble();

            // 30% chance to drop a coin
            if (randomNumber <= 0.3)
            {
                Instantiate(coinPrefab, transform.position, Quaternion.identity);
            }
            RoomController.instance.StartCoroutine(RoomController.instance.RoomCoroutine());
            Destroy(gameObject);
        }
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            // Stop the enemy's movement
            speed = 0;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Wall"))
        {
            // Stop the enemy's movement
            speed = 0;
        }
    }
}
