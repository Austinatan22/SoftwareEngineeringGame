using UnityEngine;
using System.Collections; // Add this line

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

    public float floatSpeed = 1f; // Speed of floating up and down
    public float floatAmount = 0.2f; // Amount of floating up and down

    private Vector3 initialPosition;

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

        initialPosition = transform.position;
        StartCoroutine(FloatAnimation());
    }

    private IEnumerator FloatAnimation()
    {
        while (true)
        {
            float newY = Mathf.Sin(Time.time * floatSpeed) * floatAmount;
            transform.position = new Vector3(initialPosition.x, initialPosition.y + newY, initialPosition.z);
            yield return null;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            PlayerController.collectedAmount++;
            GameController.HealPlayer(healthChange);
            GameController.MoveSpeedChange(moveSpeedChange);
            GameController.FireRateChange(attackSpeedChange);
            GameController.BulletSizeChange(bulletSizeChange);
            GameController.instance.UpdateCollectedItems(this);
            Destroy(gameObject);
        }
    }
}
