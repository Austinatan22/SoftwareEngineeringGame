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
    public static CollectionController instance;
    private bool playerInRange = false;
    public static bool isKeyAcquired = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        GetComponent<SpriteRenderer>().sprite = item.itemImage;

        PolygonCollider2D polygonCollider = GetComponent<PolygonCollider2D>();
        if (polygonCollider != null)
        {
            Destroy(polygonCollider);
        }

        polygonCollider = gameObject.AddComponent<PolygonCollider2D>();
        polygonCollider.isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            playerInRange = false;
        }
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.F))
        {
            if (gameObject.tag == "Dropped")
            {
                PickUpItem();
            }
            else if (gameObject.tag == "Key")
            {
                isKeyAcquired = true;
                Destroy(gameObject);
            }
            else if (gameObject.tag == "Coin")
            {
                CurrencyManager.instance.AddCurrency(1);
                Destroy(gameObject);
            }
        }
    }

    private void PickUpItem()
    {
        PlayerController.collectedAmount++;
        GameController.HealPlayer(healthChange);
        GameController.MoveSpeedChange(moveSpeedChange);
        GameController.FireRateChange(attackSpeedChange);
        GameController.BulletSizeChange(bulletSizeChange);
        GameController.instance.UpdateCollectedItems(this);
        Destroy(gameObject);
    }

    public void buyShopItem()
    {
        if (CurrencyManager.instance.currencyAmount >= cost)
        {
            CurrencyManager.instance.SpendCurrency(cost);
            PickUpItem();
        }
        else
        {
            Debug.Log("Not enough currency.");
        }
    }
}
