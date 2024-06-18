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
    public AudioClip pickupSound; // Sound effect for picking up the item

    private bool playerInRange = false;
    private AudioSource audioSource; // AudioSource component

    public static bool isKeyAcquired = false;

    void Start()
    {
        GetComponent<SpriteRenderer>().sprite = item.itemImage;

        // Ensure there is an AudioSource component and assign it
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null) {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Setup the AudioSource properties
        audioSource.clip = pickupSound;
        audioSource.playOnAwake = false;

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
                PlaySoundEffect();
                Destroy(gameObject);
            }
            else if (gameObject.tag == "Coin")
            {
                CurrencyManager.instance.AddCurrency(1);
                PlaySoundEffect();
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
        PlaySoundEffect();
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

    private void PlaySoundEffect()
    {
        if (audioSource && pickupSound)
        {
            audioSource.Play();
        }
    }
}
