using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    public float transitionDuration = 3.0f; // Customize the duration in the inspector
    public string targetScene = "BasementMain"; // Name of the target scene

    private void Start()
    {
        StartCoroutine(TransitionToScene());
    }

    private IEnumerator TransitionToScene()
    {
        yield return new WaitForSeconds(transitionDuration);
        SceneManager.LoadScene(targetScene);
    }
}
