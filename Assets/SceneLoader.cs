using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class SceneLoader : MonoBehaviour
{
    public string sceneToLoad;
    public Slider progressBar;

    void Start()
    {
        StartCoroutine(LoadSceneAsync());
    }

    IEnumerator LoadSceneAsync()
    {
        // Start loading the scene asynchronously
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneToLoad);
        asyncOperation.allowSceneActivation = false;

        // While the scene is loading, update the progress bar
        while (!asyncOperation.isDone)
        {
            // Progress ranges from 0.0 to 0.9, as it doesn't reach 1.0 until the scene is activated
            float progress = Mathf.Clamp01(asyncOperation.progress / 0.9f);
            progressBar.value = progress;

            // Check if the load has finished
            if (asyncOperation.progress >= 0.9f)
            {
                // Optionally wait for user input or add a delay here
                asyncOperation.allowSceneActivation = true;
            }

            yield return null;
        }
    }
}
