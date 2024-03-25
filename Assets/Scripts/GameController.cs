using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public static GameController instance;
    private static int health = 6;
    private static int maxHealth = 6;
    private static float moveSpeed = 5f;
    public static float fireRate = 0.5f;
    private static float bulletSize = 0.5f;
    static GameObject playerObj;


    public static int Health { get => health; set => health = value; }
    public static int MaxHealth { get => maxHealth; set => maxHealth = value; }
    public static float MoveSpeed { get => moveSpeed; set => moveSpeed = value; }
    public static float FireRate { get => fireRate; set => fireRate = value; }
    public static float BulletSize { get => bulletSize; set => bulletSize = value; }

    public Text healthText;

    private void Awake()
    {
        playerObj = GameObject.Find("Player");

        if (instance == null)
        {
            instance = this;
        }
    }

    void Update()
    {
        healthText.text = "Health: " + health;
    }

    public static void DamagePlayer(int damage)
    {
        health -= damage;

        if (Health <= 0)
        {
            KillPlayer();
        }
    }

    private static void KillPlayer()
    {
        Destroy(playerObj);
    }
    public static void HealPlayer(int healAmount)
    {
        health = Mathf.Min(MaxHealth, health + healAmount);
    }
    public static void MoveSpeedChange(float speed)
    {
        moveSpeed += speed;
    }
    public static void FireRateChange(float rate)
    {
        fireRate -= rate;
    }
    public static void BulletSizeChange(float size)
    {
        bulletSize += size;
    }

}