using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
    public Slider loadingBar; // Assign this in the inspector
    public float loadingTime = 3.0f; // Adjust as needed
    public Transform playerTransform; // Reference to the player's transform
    private PauseMenu pauseMenu; // Reference to the PauseMenu script

    private Vector3 originalPlayerPosition; // Store the original player position before loading
    private bool inputEnabled = true; // Flag to control if input should be processed

    private void Start()
    {
        // Get reference to the player's transform
        if (playerTransform == null)
        {
            Debug.LogError("Player Transform is not assigned!");
            return;
        }

        // Store the original player position
        originalPlayerPosition = playerTransform.position;

        // Start the loading process
        StartLoading();

        // Find the PauseMenu script in the scene
        pauseMenu = FindObjectOfType<PauseMenu>();
    }

    private void StartLoading()
    {
        StartCoroutine(LoadSceneAsync());
    }

    IEnumerator LoadSceneAsync()
    {
        // Disable input
        inputEnabled = false;

        // Set the player's position to (0, 0, 0)
        if (playerTransform != null)
        {
            playerTransform.position = Vector3.zero;
        }

        // Reset loading bar
        loadingBar.value = 0;

        // Start loading timer
        float timer = 0;
        while (timer < loadingTime)
        {
            timer += Time.deltaTime;
            loadingBar.value = Mathf.Clamp01(timer / loadingTime);
            yield return null;
        }

        // Restore the original player position
        if (playerTransform != null)
        {
            playerTransform.position = originalPlayerPosition;
        }

        // Enable input
        inputEnabled = true;

        // Deactivate the loading screen GameObject
        gameObject.SetActive(false);
    }

    void Update()
    {
        if (inputEnabled && Input.GetKeyDown(KeyCode.Escape))
        {
            if (PauseMenu.isPaused)
            {
                pauseMenu.ResumeGame();
            }
            else
            {
                pauseMenu.PauseGame();
            }
        }
    }
}