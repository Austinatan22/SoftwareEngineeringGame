using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Sprites;
public class item
{
    public string name;
    public string description;
    public Sprite itemImage;
}


public class CollectionController : MonoBehaviour
{
    public item item;

    public int healthChange;

    public float moveSpeedChange;

    public float attackSpeedChange;

    public float bulletSizeChange;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<SpriteRenderer>().sprite = item.itemImage;
        Destroy(GetComponent<PolygonCollider2D>());
        gameObject.AddComponent<PolygonCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            PlayerContoller.collectedAmount++;
            GameController.HealPlayer(healthChange);
            GameController.MoveSpeedChange(moveSpeedChange);
            GameController.FireRateChange(attackSpeedChange);
            GameController.BulletSizeChange(bulletSizeChange);
            Destroy(gameObject);
        }
    }
}