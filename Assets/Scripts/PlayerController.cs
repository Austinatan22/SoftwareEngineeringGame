using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float speed;
    private Rigidbody2D rigidbody;
    public Text collectedText;
    public static int collectedAmount = 0;

    public GameObject bulletPrefab;
    public float bulletSpeed;
    private float lastFire;
    public float fireDelay;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        fireDelay = GameController.FireRate;
        speed = GameController.MoveSpeed;
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // Update movement
        rigidbody.velocity = new Vector3(horizontal * speed, vertical * speed, 0);
        collectedText.text = "Items Collected: " + collectedAmount;

        // Check if left mouse button was clicked and if the firing delay has passed
        if (Input.GetMouseButton(0) && Time.time > lastFire + fireDelay)
        {
            ShootTowardsMouse();
            lastFire = Time.time;
        }
    }

    void ShootTowardsMouse()
<<<<<<< Updated upstream
    {
        // Get the mouse position in screen coordinates
        Vector3 mouseScreenPosition = Input.mousePosition;

        // Adjust mouse position z to match the distance to the camera
        // This should be the absolute value if the camera is looking at the origin from a negative z
        mouseScreenPosition.z = Mathf.Abs(Camera.main.transform.position.z);

        // Convert the mouse position to world coordinates
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mouseScreenPosition);

        // Ensure the bullet doesn't move along the z-axis
        mouseWorldPosition.z = 0;

        // Calculate the shooting direction
        Vector3 shootingDirection = (mouseWorldPosition - transform.position).normalized;

        // Instantiate the bullet at the player's position and orient it towards the shooting direction
        GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.LookRotation(Vector3.forward, shootingDirection)) as GameObject;
        bullet.AddComponent<Rigidbody2D>().gravityScale = 0;
        bullet.GetComponent<Rigidbody2D>().velocity = shootingDirection * bulletSpeed;
    }
}
=======
{
    // Get the mouse position in screen coordinates
    Vector3 mouseScreenPosition = Input.mousePosition;

    // Adjust mouse position z to match the distance to the camera
    // This should be the absolute value if the camera is looking at the origin from a negative z
    mouseScreenPosition.z = Mathf.Abs(Camera.main.transform.position.z);

    // Convert the mouse position to world coordinates
    Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mouseScreenPosition);

    // Ensure the bullet doesn't move along the z-axis
    mouseWorldPosition.z = 0;

    // Calculate the shooting direction
    Vector3 shootingDirection = (mouseWorldPosition - transform.position).normalized;

    // Instantiate the bullet at the player's position and orient it towards the shooting direction
    GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.LookRotation(Vector3.forward, shootingDirection)) as GameObject;
    bullet.AddComponent<Rigidbody2D>().gravityScale = 0;
    bullet.GetComponent<Rigidbody2D>().velocity = shootingDirection * bulletSpeed;
}

}
>>>>>>> Stashed changes
