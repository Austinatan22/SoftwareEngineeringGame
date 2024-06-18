using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float lifeTime;
    public bool isEnemyBullet = false;

    private Vector2 lastPos;
    private Vector2 curPos;
    private Vector2 playerPos;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DeathDelay());
        if (!isEnemyBullet)
        {
            transform.localScale = new Vector2(GameController.BulletSize, GameController.BulletSize);
        }
    }

    void Update()
    {
        // Restrict z-axis rotation
        transform.rotation = Quaternion.Euler(0, 0, 0);

        if (isEnemyBullet)
        {
            curPos = transform.position;
            transform.position = Vector2.MoveTowards(transform.position, playerPos, 5f * Time.deltaTime);
            if (curPos == lastPos)
            {
                Destroy(gameObject);
            }
            lastPos = curPos;
        }
    }

    public void GetPlayer(Transform player)
    {
        playerPos = player.position;
    }

    IEnumerator DeathDelay()
    {
        yield return new WaitForSeconds(lifeTime);
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Enemy" && !isEnemyBullet)
        {
            EnemyController enemyController = col.GetComponent<EnemyController>();
            EnemyBombController enemyBombController = col.GetComponent<EnemyBombController>();
            if (enemyController != null)
            {
                enemyController.DamageEnemy(1);
            }
            if (enemyBombController != null)
            {
                enemyBombController.DamageEnemy(1);
            }
            Destroy(gameObject);
        }

        if (col.tag == "Boss" && !isEnemyBullet)
        {
            BossController bossController = col.GetComponent<BossController>();
            bossController.TakeDamage(5);
            Destroy(gameObject);
        }

        if (col.tag == "Player" && isEnemyBullet)
        {
            GameController.DamagePlayer(1);
            Destroy(gameObject);
        }

        if (col.gameObject.name.Contains("Wall") || col.gameObject.name.Contains("Door"))
        {
            Destroy(gameObject); // Destroy the bullet
        }

        if (col.tag == "Crate" && !isEnemyBullet)
        {
            CrateManager crateManager = col.GetComponent<CrateManager>();
            crateManager.damageCrate();
            Destroy(gameObject);
        }
    }

    
}
