using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("CharacterSelection");
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
