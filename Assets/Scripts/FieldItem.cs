using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldItem : MonoBehaviour
{
    public Item item;
    public SpriteRenderer image;

    public void setItem(Item item)
    {
        this.item = item;

        image.sprite = item.sprite;
    }

    public Item getItem()
    {
        return item;
    }

    public void destroyItem()
    {
        Destroy(gameObject);
    }
}
