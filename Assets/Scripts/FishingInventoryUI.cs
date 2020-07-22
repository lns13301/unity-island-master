using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FishingInventoryUI : MonoBehaviour
{
    private FishingPlayerInventory fishingPlayerInventory;
    public GameObject fishingInventorySet;

    public FishingSlot[] slots;
    public Transform slotHolder;

    void Start()
    {
        fishingPlayerInventory = FishingPlayerInventory.instance;
        slots = slotHolder.GetComponentsInChildren<FishingSlot>();
        fishingPlayerInventory.onFishingSlotCountChange += slotChange;
        fishingPlayerInventory.onFishingChangeItem += redrawSlotUI;
        fishingInventorySet.SetActive(false);
    }

    private void slotChange(int val)
    {
        for (int i = 1; i < slots.Length; i++)
        {
            slots[i].slotNumber = i;

            if (i < fishingPlayerInventory.fishingSlotCount)
            {
                slots[i].GetComponent<Button>().interactable = true;
            }
            else
            {
                slots[i].GetComponent<Button>().interactable = false;
            }
        }
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            uiOnOff();
        }
    }

    public void uiOnOff()
    {
        if (GameObject.Find("Canvas").GetComponent<ItemMenuSet>().isReinforceProgressing)
        {
            return;
        }
        GameObject.Find("DialogManager").GetComponent<DialogManager>().saveAndLoadPlayerFishingInventoryTemp();

        if (fishingInventorySet.activeSelf)
        {
            fishingInventorySet.SetActive(false);
        }
        else
        {

            if (GetComponent<StatUI>().statSet.activeSelf)
            {
                GetComponent<StatUI>().statSet.SetActive(false);
            }
            if (GetComponent<EquipmentUI>().equipmentSet.activeSelf)
            {
                GetComponent<EquipmentUI>().equipmentSet.SetActive(false);
            }
            if (FishingUI.instance.fishingStartSet.activeSelf)
            {
                FishingUI.instance.fishingStartSet.SetActive(false);
            }

            fishingInventorySet.SetActive(true);
        }
    }

    public void addSlot()
    {
        fishingPlayerInventory.fishingSlotCount++;
    }

    void redrawSlotUI()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].removeSlotUI();
        }
        for (int i = 0; i < fishingPlayerInventory.items.Count; i++)
        {
            slots[i].item = fishingPlayerInventory.items[i];
            slots[i].updateSlotUI();
        }
    }
}
