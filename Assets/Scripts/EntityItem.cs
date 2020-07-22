using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityItem : MonoBehaviour
{
    private Animator playerAnimator;
    public DialogManager dialogManager;
    private Player playerScript;
    private bool picked = false;
    private bool isPlayerIn = false;
    private bool isRemoval = false;

    public Item item;
    public int count;
    public float weight;
    public SpriteRenderer image;
    public int itemDBCode;
    void Start()
    {
        playerAnimator = GameObject.Find("Player").GetComponent<Animator>();
        playerScript = GameObject.Find("Player").GetComponent<Player>();
        dialogManager = GameObject.Find("DialogManager").GetComponent<DialogManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if ((picked || isPlayerIn) && playerScript.pickUpAnimationProgress())
        {
            if (PlayerInventory.instance.addItem(item, count))
            {
                dialogManager.pickUpMessage(item.itemName);
                playerScript.isPickedItem = true;
                dialogManager.playerData.moisture -= weight;
                Destroy(gameObject);
            }
            else
            {
                dialogManager.pickUpMessageFull();
                Instantiate(this, new Vector2(GetComponent<Transform>().position.x, GetComponent<Transform>().position.y), Quaternion.identity);
                Destroy(gameObject);
            }
        }

        if (isRemoval)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            isPlayerIn = true;
        }

        if (collision.gameObject.tag == "Removal")
        {
            isRemoval = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            isPlayerIn = false;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && playerAnimator.GetBool("isPicking"))
        {
            picked = true;
        }
    }

    public void setItem(Item item, int itemCount = 1)
    {
        this.item = ItemDatabase.instance.makeItem(item);

        this.item.sprite = item.sprite;
        image.sprite = item.sprite;
    }
}
