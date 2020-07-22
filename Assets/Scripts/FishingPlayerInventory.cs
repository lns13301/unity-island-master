using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingPlayerInventory : MonoBehaviour
{
    public static FishingPlayerInventory instance;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }
    public delegate void OnFishingSlotCountChange(int val);
    public OnFishingSlotCountChange onFishingSlotCountChange;

    public delegate void OnFishingChangeItem();
    public OnFishingChangeItem onFishingChangeItem;

    private int slotCnt;
    public List<Item> items = new List<Item>();

    public DialogManager dialogManager;

    public int fishingSlotCount
    {
        get => slotCnt;
        set
        {
            slotCnt = value;
            onFishingSlotCountChange.Invoke(slotCnt);
        }
    }

    void Start()
    {
        fishingSlotCount = dialogManager.playerData.inventorySize;
    }

    public bool addItem(Item item, int count = 1)
    {
        int countLimit = item.countLimit;
        Item instanceItem = item;
        instanceItem.count = count;
        int quantity = count;
        int space = countListItems(instanceItem);
        int spaceCheckValue;

        if (instanceItem.type == ItemType.Equipment && items.Count < fishingSlotCount)
        {
            items.Add(instanceItem);

            if (onFishingChangeItem != null)
            {
                onFishingChangeItem.Invoke();
            }

            return true;
        }

        if (instanceItem.type == ItemType.Equipment)
        {
            return false;
        }

        if (items.Count <= fishingSlotCount && quantity <= findItemLeftSpace(instanceItem))
        {
            List<int> itemsIndex = getItemIndexByCodeAll(instanceItem);

            // 아이템이 존재하고 남은 공간에 아이템을 넣을 수 있는지
            for (int i = 0; i < itemsIndex.Count; i++)
            {
                if (items[itemsIndex[i]].count + quantity > 100)
                {
                    items[itemsIndex[i]].count = 100;
                    quantity = quantity - (100 - items[itemsIndex[i]].count);
                }
                else
                {
                    items[itemsIndex[i]].count += quantity;
                    quantity = 0;
                }
            }

            // 인벤토리 오류 임시 방편
            GameObject.Find("DialogManager").GetComponent<DialogManager>().saveAndLoadPlayerFishingInventoryTemp();

            if (quantity == 0)
            {
                onFishingChangeItem.Invoke();
                return true;
            }

        }

        if (space + quantity / countLimit + 1 == 0)
        {
            spaceCheckValue = 0;
        }
        else
        {
            spaceCheckValue = (space + quantity) / 101;
        }

        // 아이템이 존재하고 추가할 아이템의 일부를 기존아이템에 넣고 남는양을 새로운 슬롯에 넣을 수 있는지
        if (items.Count + spaceCheckValue < fishingSlotCount)
        {
            List<int> itemsIndex = getItemIndexByCodeAll(instanceItem);

            for (int i = 0; i < itemsIndex.Count; i++)
            {
                if (items[itemsIndex[i]].count + quantity > 100)
                {
                    quantity -= 100 - items[itemsIndex[i]].count;
                    items[itemsIndex[i]].count = 100;

                    // 인벤토리 오류 임시 방편
                    GameObject.Find("DialogManager").GetComponent<DialogManager>().saveAndLoadPlayerFishingInventoryTemp();
                }
            }
        }
        instanceItem.count = quantity;

        // 기존에 아이템이 없고 아이템을 추가할 슬롯이 있는지
        if (instanceItem.count > 0 && items.Count < fishingSlotCount && instanceItem.count <= 100)
        {
            if (onFishingChangeItem != null)
            {
                items.Add(instanceItem);

                // 인벤토리 오류 임시 방편
                GameObject.Find("DialogManager").GetComponent<DialogManager>().saveAndLoadPlayerFishingInventoryTemp();

                onFishingChangeItem.Invoke();
            }
            return true;
        }
        else
        {
            return false;
        }
    }

    public void removeItem(int slotNumber, int quantity = 1)
    {
        Item instanceItem = items[slotNumber];

        if (instanceItem.type == ItemType.Equipment)
        {
            items.RemoveAt(slotNumber);

            // 인벤토리 오류 임시 방편
            GameObject.Find("DialogManager").GetComponent<DialogManager>().saveAndLoadPlayerFishingInventoryTemp();

            onFishingChangeItem.Invoke();

            return;
        }

        if (quantity == instanceItem.count)
        {
            items.RemoveAt(slotNumber);

            // 인벤토리 오류 임시 방편
            GameObject.Find("DialogManager").GetComponent<DialogManager>().saveAndLoadPlayerFishingInventoryTemp();

            onFishingChangeItem.Invoke();

            return;
        }

        if (quantity <= findItemLeftSpace(instanceItem))
        {
            List<int> itemsIndex = getItemIndexByCodeAll(instanceItem);

            for (int i = itemsIndex.Count - 1; i >= 0; i--)
            {
                if (items[itemsIndex[i]].count - quantity < 0)
                {
                    quantity -= items[itemsIndex[i]].count;
                    items[itemsIndex[i]].count = 0;
                }
                else
                {
                    items[itemsIndex[i]].count -= quantity;
                    quantity = 0;
                }
            }
        }

        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].count <= 0)
            {
                items.RemoveAt(i);
            }
        }

        // 인벤토리 오류 임시 방편
        GameObject.Find("DialogManager").GetComponent<DialogManager>().saveAndLoadPlayerFishingInventoryTemp();

        onFishingChangeItem.Invoke();
    }

    public List<Item> findItemByCodeAll(Item item)
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].code == item.code)
            {
                items.Add(items[i]);
            }
        }
        return items.Count > 0 ? items : null;
    }

    public List<int> getItemIndexByCodeAll(Item item)
    {
        List<int> index = new List<int>();

        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].code == item.code)
            {
                index.Add(i);
            }
        }

        return index;
    }

    public int findItemLeftSpace(Item item)
    {
        int size = 0;

        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].code == item.code)
            {
                size += 100 - items[i].count;
            }
        }

        return size;
    }

    public int countListItems(Item item)
    {
        int value = 0;

        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].code == item.code)
            {
                value += 100 - items[i].count;
            }
        }

        return value;
    }

    public void showInventory()
    {
        for (int i = 0; i < items.Count; i++)
        {
            Debug.Log("아이템 슬롯 : " + i + "  아이템 이름 : " + items[i].itemName + "  아이템 개수 : " + items[i].count);
        }
    }
}
