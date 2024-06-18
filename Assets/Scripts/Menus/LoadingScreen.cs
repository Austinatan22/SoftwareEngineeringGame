using System.Collections;
using UnityEngine;

public class LoadingScreen : MonoBehaviour
{
    public GameObject loadingImage;
    public float loadingTime = 2.5f; // Adjust as needed
    public Transform playerTransform; // Reference to the player's transform

    private Vector3 originalPlayerPosition; // Store the original player position before loading
    private PauseMenu pauseMenu; // Reference to the PauseMenu script
    private bool inputEnabled; // Flag to control if input should be processed
    private bool isLoading = false; // Flag to check if loading is currently active

    private void Start()
    {
        // Check if the player transform is assigned
        if (playerTransform == null)
        {
            return;
        }

        // Store the original player position
        originalPlayerPosition = playerTransform.position;

        // Find the PauseMenu script in the scene
        pauseMenu = FindObjectOfType<PauseMenu>();

        // Start the loading process only if it's not already loading
        if (!isLoading)
        {
            StartLoading();
        }
    }

    private void StartLoading()
    {
        isLoading = true; // Set the loading flag
        StartCoroutine(LoadSceneAsync());
    }

    IEnumerator LoadSceneAsync()
    {
        inputEnabled = false;

        if (pauseMenu != null)
        {
            PauseMenu.Available = false;
        }

        if (playerTransform != null)
        {
            playerTransform.position = Vector3.zero;
        }

        // Simulate loading process
        float timer = 0;
        while (timer < loadingTime)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        if (playerTransform != null)
        {
            playerTransform.position = originalPlayerPosition;
        }

        inputEnabled = true;
        isLoading = false; // Reset the loading flag

        if (pauseMenu != null)
        {
            PauseMenu.Available = true;
        }

        loadingImage.SetActive(false); // Deactivate the loading screen GameObject
    }

    void Update()
    {
    }
}
