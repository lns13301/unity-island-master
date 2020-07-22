using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DropItem
{
    public Item item;
    public float dropChance;
    public int count;

    public DropItem(Item item, float dropChance = 100, int count = 1)
    {
        this.item = item;
        this.dropChance = dropChance;
        this.count = count;
    }

    public bool calculateDropChance(float value)
    {
        return Random.Range(0, 10000) <= value * 100;
    }
}
