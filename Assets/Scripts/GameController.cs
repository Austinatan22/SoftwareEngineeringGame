﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public static GameController instance;

    private static float health = 4;
    private static int maxHealth = 6;
    private static float moveSpeed = 5f;
    private static float fireRate = 0.5f;
    private static float bulletSize = 0.5f;
    private static int levelCount = 1;

    private bool bootCollected = false;
    private bool screwCollected = false;
    public static bool firstTime = true;

    public List<string> collectedNames = new List<string>();
    public GameOverManager gameOverManager;

    public static float Health { get => health; set => health = value; }
    public static int MaxHealth { get => maxHealth; set => maxHealth = value; }
    public static float MoveSpeed { get => moveSpeed; set => moveSpeed = value; }
    public static float FireRate { get => fireRate; set => fireRate = value; }
    public static float BulletSize { get => bulletSize; set => bulletSize = value; }

    public GameObject lowHealthCanvas; // Canvas to be shown at low health
    public CanvasGroup lowHealthCanvasGroup;

    public Text healthText;

    // Start is called before the first frame update
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        lowHealthCanvas.SetActive(true);
        lowHealthCanvasGroup.alpha = 0;
    }

    // Update is called once per frame
    void Update()
    {
        healthText.text = "Health: " + health;

        if (Health == 1)
        {
            lowHealthCanvasGroup.alpha = 1; // Fully visible
        }
        else if (Health > 1)
        {
            lowHealthCanvasGroup.alpha = Mathf.Max(0, lowHealthCanvasGroup.alpha - Time.deltaTime * 0.5f); // Fade out
        }

    }

    public static void DamagePlayer(int damage)
    {
        health -= damage;

        if (Health <= 0)
        {
            KillPlayer();
        }
    }

    public static void HealPlayer(float healAmount)
    {
        health = Mathf.Min(maxHealth, health + healAmount);
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

    public void IncrementLevelCount()
    {
        levelCount++;
        Debug.Log("Level Count: " + levelCount);
    }
    public void UpdateCollectedItems(CollectionController item)
    {
        collectedNames.Add(item.item.name);

        foreach (string i in collectedNames)
        {
            switch (i)
            {
                case "Boot":
                    bootCollected = true;
                    break;
                case "Screw":
                    screwCollected = true;
                    break;
            }
        }

        if (bootCollected && screwCollected)
        {
            FireRateChange(0.25f);
        }
    }

    private static void KillPlayer()
    {
        if (instance != null && instance.gameOverManager != null)
        {
            instance.gameOverManager.OnPlayerDeath();
        }
        else
        {
            Debug.LogWarning("GameOverManager is not set or not found!");
        }
    }

    public static void ResetPlayer()
    {
        health = maxHealth;
        Debug.Log("Player health reset to max health: " + maxHealth);
    }
}
