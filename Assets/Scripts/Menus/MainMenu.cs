using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        if (GameController.firstTime)
        {
            // Set firstTime to false after the first run
            GameController.firstTime = false;
            // Load the Animation scene first
            SceneManager.LoadScene("Animation");
        }
        else
        {
            // Load the BasementMain scene directly
            SceneManager.LoadScene("BasementMain");
        }
    }

    public void Hidden()
    {
        // Load the LoadingScreenHidden scene
        SceneManager.LoadScene("LoadingScreenHidden");
    }

    public void QuitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
