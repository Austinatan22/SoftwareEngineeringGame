using UnityEngine;
using System.Collections; // Needed for IEnumerator
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        StartCoroutine(DelayedLoad("CharacterSelection", 0.6f)); // Start the coroutine with a 1 second delay
    }

    IEnumerator DelayedLoad(string sceneName, float delay)
    {
        yield return new WaitForSeconds(delay); // Wait for the specified delay
        SceneManager.LoadScene(sceneName); // Load the scene after the delay
    }

    public void QuitGame()
    {
        StartCoroutine(DelayedQuit(0.6f)); // Start the coroutine with a 1 second delay
    }

    IEnumerator DelayedQuit(float delay)
    {
        yield return new WaitForSeconds(delay); // Wait for 1 second
        Application.Quit(); // Quit the application
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // Stop playing in the Unity Editor
#endif
    }
}
