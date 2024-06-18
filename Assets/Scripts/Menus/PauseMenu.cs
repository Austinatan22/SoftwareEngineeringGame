using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    public static bool isPaused;
    public static bool Available = true;
    public PlayerController playerController;

    void Start()
    {
        pauseMenu.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && Available)
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
    public void SavePlayerPosition()
    {
        if (playerController != null)
        {
            // Convert Vector3 to a string
            string position = playerController.transform.position.x + "," + playerController.transform.position.y + "," + playerController.transform.position.z;
            PlayerPrefs.SetString("PlayerPosition", position);
            PlayerPrefs.Save(); // Don't forget to save PlayerPrefs changes
            Debug.Log("Player position saved: " + position);
        }
        else
        {
            Debug.LogError("PlayerController is not assigned in PauseMenu.");
        }
    }
    public void LoadPlayerPosition()
    {
        if (playerController != null)
        {
            string position = PlayerPrefs.GetString("PlayerPosition", "0,0,0");
            string[] values = position.Split(',');
            if (values.Length == 3)
            {
                Vector3 loadedPosition = new Vector3(float.Parse(values[0]), float.Parse(values[1]), float.Parse(values[2]));
                playerController.transform.position = loadedPosition;
                Debug.Log("Player position loaded: " + loadedPosition);
            }
            else
            {
                Debug.LogError("Error in loading position data.");
            }
        }
        else
        {
            Debug.LogError("PlayerController is not assigned in PauseMenu.");
        }
    }
}