using UnityEngine;

[System.Serializable]
public class Item
{
    public string name;
    public string description;
    public Sprite itemImage;
}

public class CollectionController : MonoBehaviour
{
    public Item item;
    public float healthChange;
    public float moveSpeedChange;
    public float attackSpeedChange;
    public float bulletSizeChange;
    public int cost;

    private bool playerInRange = false;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<SpriteRenderer>().sprite = item.itemImage;

        // Destroy the existing PolygonCollider2D if it exists and add a new one
        PolygonCollider2D polygonCollider = GetComponent<PolygonCollider2D>();
        if (polygonCollider != null)
        {
            Destroy(polygonCollider);
        }

        polygonCollider = gameObject.AddComponent<PolygonCollider2D>();
        polygonCollider.isTrigger = true; // Set the collider to be a trigger
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            playerInRange = true;  // Player enters the trigger zone
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            playerInRange = false;  // Player leaves the trigger zone
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.F) && gameObject.tag == "Dropped")  // Check if 'F' is pressed while player is in range
        {
            PlayerController.collectedAmount++;
            GameController.HealPlayer(healthChange);
            GameController.MoveSpeedChange(moveSpeedChange);
            GameController.FireRateChange(attackSpeedChange);
            GameController.BulletSizeChange(bulletSizeChange);
            GameController.instance.UpdateCollectedItems(this);
            Destroy(gameObject);  // Destroy item after pickup
        }
        else if (playerInRange && Input.GetKeyDown(KeyCode.F) && gameObject.tag == "Shop")
        {
            // CurrencyManager.instance.SpendCurrency
            if (CurrencyManager.instance.currencyAmount >= cost)
            {
                PlayerController.collectedAmount++;
                GameController.HealPlayer(healthChange);
                GameController.MoveSpeedChange(moveSpeedChange);
                GameController.FireRateChange(attackSpeedChange);
                GameController.BulletSizeChange(bulletSizeChange);
                GameController.instance.UpdateCollectedItems(this);
                CurrencyManager.instance.SpendCurrency(cost);
                Destroy(gameObject);  // Destroy item after pickup
            }
        }
        else if (playerInRange && gameObject.tag == "Key")
        {
            GameController.isKeyAcquired = true;
            Destroy(gameObject);
        }
        else if (playerInRange && gameObject.tag == "Coin")
        {
            CurrencyManager.instance.AddCurrency(1);
            Destroy(gameObject);
        }
    }
}