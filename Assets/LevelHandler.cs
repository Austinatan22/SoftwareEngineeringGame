using UnityEngine;

public class LevelHandler : MonoBehaviour
{
    public GameObject toggle; // Assign the movelevel GameObject to this field in the Unity Editor

    void Start()
    {
        // Ensure toggle is assigned
        if (toggle == null)
        {
            Debug.LogError("Toggle (movelevel GameObject) is not assigned.");
        }
    }

    void Update()
    {
        
    }

    public void ActivateMoveLevel()
    {
        if (toggle != null)
        {
            toggle.SetActive(true);
            Debug.Log("movelevel GameObject activated.");
        }
        else
        {
            Debug.LogError("Toggle (movelevel GameObject) is not assigned.");
        }
    }
}