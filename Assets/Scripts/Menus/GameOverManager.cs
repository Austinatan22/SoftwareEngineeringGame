using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverManager : MonoBehaviour
{
    public GameObject gameOverPanel; // Drag the panel to this field in the inspector
    public Button retryButton; // Drag the retry button here
    public Button quitButton; // Drag the quit button here
    private PauseMenu pauseMenu; // Reference to the PauseMenu script

    void Start()
    {
        // Hide game over panel at start
        gameOverPanel.SetActive(false);

        // Add listeners to buttons
        retryButton.onClick.AddListener(RetryGame);
        quitButton.onClick.AddListener(QuitGame);

        // Find the PauseMenu script in the scene
        pauseMenu = FindObjectOfType<PauseMenu>();

        if (pauseMenu == null)
        {
            Debug.LogWarning("PauseMenu script not found in the scene.");
        }
    }

    // Call this function when the player dies
    public void OnPlayerDeath()
    {
        Debug.Log("Player has died.");
        gameOverPanel.SetActive(true);

        // Optionally pause the game if not using a time scale-independent menu
        Time.timeScale = 0;

        // Set PauseMenu availability to false
        if (pauseMenu != null)
        {
            PauseMenu.Available = false;
        }
    }

    void RetryGame()
    {
        Time.timeScale = 1; // Resume the game time
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Reload the current scene

        GameController.ResetPlayer(); 
        // Set PauseMenu availability to true
        if (pauseMenu != null)
        {
            PauseMenu.Available = true;
        }
    }

    void QuitGame()
    {
        GameController.ResetPlayer(); 
        
        Time.timeScale = 1; // Resume the game time
        SceneManager.LoadScene("MainMenu"); // Change "MainMenu" to your main menu scene name
        GameController.firstTime = true;

        // Set PauseMenu availability to true
        if (pauseMenu != null)
        {
            PauseMenu.Available = true;
        }
    }
}
