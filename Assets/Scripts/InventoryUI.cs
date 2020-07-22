﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    private PlayerInventory playerInventory;
    public GameObject inventorySet;

    public JoystickValue value;

    public Slot[] slots;
    public Transform slotHolder;

    public Animator buttonMenuAnimator;

    public GameObject itemMenuSet;

    void Start()
    {
        playerInventory = PlayerInventory.instance;
        slots = slotHolder.GetComponentsInChildren<Slot>();
        playerInventory.onSlotCountChange += slotChange;
        playerInventory.onChangeItem += redrawSlotUI;
        inventorySet.SetActive(false);
    }

    private void slotChange(int val)
    {
        for (int i = 1; i < slots.Length; i++)
        {
            slots[i].slotNumber = i;

            if (i < playerInventory.slotCount)
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
        if (Input.GetKeyDown(KeyCode.I))
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
        GameObject.Find("DialogManager").GetComponent<DialogManager>().saveAndLoadPlayerInventoryTemp();
        GameObject.Find("DialogManager").GetComponent<DialogManager>().saveAndLoadPlayerEquipmentTemp();

        itemMenuSet.SetActive(false);

        if (inventorySet.activeSelf)
        {
            inventorySet.SetActive(false);
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

            inventorySet.SetActive(true);
            buttonMenuAnimator.SetBool("isUIOn", true);
        }
    }

    public void addSlot()
    {
        if (GameObject.Find("DialogManager").GetComponent<DialogManager>().playerData.inventorySize >= 25)
        {
            Debug.Log("더이상 늘릴 수 없습니다.");
            return;
        }

        playerInventory.slotCount++;
    }

    void redrawSlotUI()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].removeSlotUI();
        }
        for (int i = 0; i < playerInventory.items.Count; i++)
        {
            slots[i].item = playerInventory.items[i];
            slots[i].updateSlotUI();
        }
    }

    public void testSetItemPowerUp()
    {
        playerInventory.items[0].power = 999;
    }
}
