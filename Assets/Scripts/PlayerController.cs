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

    public Animator animator;

    private bool isFacingRight = true; // To keep track of the player's facing direction

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

        Vector2 moveInput = new Vector2(horizontal, vertical);

        // Normalize the movement vector if it's longer than 1
        if (moveInput.magnitude > 1)
        {
            moveInput.Normalize();
        }

        // Set animator parameters
        animator.SetFloat("Horizontal", horizontal);
        animator.SetFloat("Vertical", vertical);
        animator.SetFloat("Speed", rigidbody.velocity.magnitude);

        // Update movement
        rigidbody.velocity = moveInput * speed;
        collectedText.text = "Items Collected: " + collectedAmount;

        // Flip the player sprite based on movement direction
        if (horizontal > 0 && !isFacingRight)
        {
            Flip();
        }
        else if (horizontal < 0 && isFacingRight)
        {
            Flip();
        }

        // Check if left mouse button was clicked and if the firing delay has passed
        if (Input.GetMouseButton(0) && Time.time > lastFire + fireDelay)
        {
            ShootTowardsMouse();
            lastFire = Time.time;
        }
    }

    void ShootTowardsMouse()
    {
        Vector3 mouseScreenPosition = Input.mousePosition;
        mouseScreenPosition.z = Mathf.Abs(Camera.main.transform.position.z);
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mouseScreenPosition);
        mouseWorldPosition.z = 0;
        Vector3 shootingDirection = (mouseWorldPosition - transform.position).normalized;

        GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.LookRotation(Vector3.forward, shootingDirection)) as GameObject;
        bullet.AddComponent<Rigidbody2D>().gravityScale = 0;
        bullet.GetComponent<Rigidbody2D>().velocity = shootingDirection * bulletSpeed;
    }

    void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}
