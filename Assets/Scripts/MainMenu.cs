using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        // Load the LoadingScreen scene first
        SceneManager.LoadScene("BasementMain");
    }

    public void Hidden()
    {
        // Load the LoadingScreen scene first with different target
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
