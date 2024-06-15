using UnityEngine;

public class FollowMouse : MonoBehaviour
{
    void Start()
    {
        // Hide the cursor when the game runs
        Cursor.visible = false;
    }

    void Update()
    {
        // Get the current mouse position in screen coordinates
        Vector3 mouseScreenPosition = Input.mousePosition;

        // Set the z-coordinate to the camera's z position; this is crucial for proper depth in 3D games, 
        // For 2D games, you might want to adjust this depending on your camera setup.
        mouseScreenPosition.z = Mathf.Abs(Camera.main.transform.position.z);

        // Convert the screen position of the mouse to world coordinates
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mouseScreenPosition);

        // For 2D games, you might need to reset the z position to 0 if it's set to something else by the camera
        mouseWorldPosition.z = 0;

        // Update this object's position to the mouse world position
        transform.position = mouseWorldPosition;
    }
}
