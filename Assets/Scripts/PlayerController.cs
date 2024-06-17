using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float speed;
    private Rigidbody2D rigidbody;
    public Text collectedText;
    public static int collectedAmount = 0;

    public GameObject bulletPrefab;
    public GameObject gun;  // Reference to the gun object
    public float bulletSpeed;
    private float lastFire;
    public float fireDelay;
    public float gunDistanceFromPlayer = 1.0f;  // Distance of the gun from the player

    public Animator animator;

    private bool isFacingRight = true;

    private Vector2 lastMovementDirection; // Stores the last non-zero movement direction
    public CharacterDatabase characterDB;
    public SpriteRenderer artworkSprite;
    private int selectedOption = 0;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        if (PlayerPrefs.HasKey("selectedOption"))
        {
            selectedOption = 0;
        }
        else
        {
            Load();
        }
        UpdateCharacter(selectedOption);
    }

    void Update()
    {
        fireDelay = GameController.FireRate;
        speed = GameController.MoveSpeed;
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector2 moveInput = new Vector2(horizontal, vertical);

        if (moveInput.magnitude > 1)
        {
            moveInput.Normalize();
        }

        if (moveInput != Vector2.zero) // Check if there is any movement
        {
            lastMovementDirection = moveInput; // Update the last movement direction when moving
            animator.SetFloat("Horizontal", horizontal);
            animator.SetFloat("Vertical", vertical);
        }
        else
        {
            // Keep the last direction in the animator when stopping
            animator.SetFloat("Horizontal", lastMovementDirection.x);
            animator.SetFloat("Vertical", lastMovementDirection.y);
        }

        animator.SetFloat("Speed", rigidbody.velocity.magnitude);
        rigidbody.velocity = moveInput * speed;
        collectedText.text = "Items Collected: " + collectedAmount;

        if (horizontal > 0 && !isFacingRight || horizontal < 0 && isFacingRight)
        {
            Flip();
        }

        UpdateGunPosition();
        AimGunAtMouse();

        if (Input.GetMouseButton(0) && Time.time > lastFire + fireDelay)
        {
            ShootTowardsMouse();
            lastFire = Time.time;
        }
    }

    void UpdateGunPosition()
    {
        Vector3 mouseScreenPosition = Input.mousePosition;
        mouseScreenPosition.z = Mathf.Abs(Camera.main.transform.position.z);
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mouseScreenPosition);
        mouseWorldPosition.z = 0;

        Vector3 direction = (mouseWorldPosition - transform.position).normalized;
        gun.transform.position = transform.position + direction * gunDistanceFromPlayer;
    }

    void AimGunAtMouse()
    {
        Vector3 mouseScreenPosition = Input.mousePosition;
        mouseScreenPosition.z = Mathf.Abs(Camera.main.transform.position.z);
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mouseScreenPosition);
        mouseWorldPosition.z = 0;

        Vector3 direction = (mouseWorldPosition - gun.transform.position).normalized;
        gun.transform.up = direction;

        // Calculate the angle from the player to the mouse in degrees
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Flip the gun horizontally based on the angle
        if (angle > 90 || angle < -90)
        {
            // When the gun is to the left of the player
            gun.transform.localScale = new Vector3(-1, 1, 1); // Flip horizontally
        }
        else
        {
            // When the gun is to the right of the player
            gun.transform.localScale = new Vector3(1, 1, 1); // Normal scale
        }
    }

    void ShootTowardsMouse()
    {
        GameObject bullet = Instantiate(bulletPrefab, gun.transform.position, Quaternion.LookRotation(Vector3.forward, gun.transform.up)) as GameObject;
        bullet.AddComponent<Rigidbody2D>().gravityScale = 0;
        bullet.GetComponent<Rigidbody2D>().velocity = gun.transform.up * bulletSpeed;

        // Trigger the shooting animation
        gun.GetComponent<Animator>().SetTrigger("Shoot");
    }


    void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;

        // Ensure the gun does not flip with the player
        gun.transform.localScale = new Vector3(Mathf.Abs(gun.transform.localScale.x), gun.transform.localScale.y, gun.transform.localScale.z);
    }
    private void UpdateCharacter(int selectedOption)
    {
        CharacterSelect character = characterDB.getCharacter(selectedOption);
        artworkSprite.sprite = character.characterSprite;
    }
    private void Load()
    {
        selectedOption = PlayerPrefs.GetInt("selectedOption");
    }
}
