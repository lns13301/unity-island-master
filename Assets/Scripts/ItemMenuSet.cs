﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ItemMenuSet : MonoBehaviour
{
    public static ItemMenuSet instance;

    public string NEW_LINE = "\n";
    public GameObject itemMenuSet;
    public Text itemName;
    public Text itemRating;
    public Text itemStats;
    public Text itemContents;
    public GameObject infoTag;
    public int optionSize;
    public RectTransform itemMenuSetRect;
    public RectTransform itemInfomationRect;

    public List<GameObject> buttons = new List<GameObject>();
    public Image itemInfomationUI;

    public Item item;
    public int slotNumber;

    //reinforce
    public ReinforceSlot reinforceSlot;
    public bool isReinforceProgressing = false;

    public bool isPlayerInventory;

    public Text inputField;
    public int inputCount;
    public string touchPanelNumberString;

    //QuickSlot
    public QuickSlot[] quickSlot;
    public bool isQuickSlotDataChanged;
    public bool isSetSlotOn;

    // Start is called before the first frame update
    void Start()
    {
        isSetSlotOn = false;
        isQuickSlotDataChanged = false;
        instance = this;

        itemMenuSet.SetActive(false);
        itemInfomationUI.gameObject.SetActive(false);
        infoTag.SetActive(false);
        isPlayerInventory = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isQuickSlotDataChanged)
        {
            closeQuickSlots();
        }
    }

    public void setButton(Item item, Vector2 position)
    {
        itemMenuSet.transform.position = position;
    }

    public void doAction()
    {
        if (isPlayerInventory)
        {
            if (item.equip(item))
            {
                GameObject.Find("DialogManager").GetComponent<PlayerEquipment>().addItem(item, slotNumber);
            }
            else if (item.type == ItemType.Consumable || item.type == ItemType.Fish)
            {
                if (!GameObject.Find("Player").GetComponent<Player>().eatAnimationDone())
                {
                    return;
                }

                item.itemEffect.useItem();
                PlayerInventory.instance.removeItem(slotNumber);
            }
        }
        else
        {
            if (PlayerInventory.instance.addItem(item))
            {
                FishingPlayerInventory.instance.removeItem(slotNumber);

                // 중복 작용시 발생하는 오류 임시방편
                GameObject.Find("Canvas").GetComponent<FishingInventoryUI>().fishingInventorySet.SetActive(false);
            }
        }

        GameObject.Find("Canvas").GetComponent<StatUI>().isDataChanged = true;

        try
        {
            itemMenuSet.SetActive(false);
        }
        catch (ArgumentOutOfRangeException)
        {
            item = null;
            slotNumber = -1;
            Debug.Log("빈 슬롯");
            itemMenuSet.SetActive(false);
        }
    }

    public void setQuickSlot()
    {
        openQuickSlots();

    }

    public void openQuickSlots()
    {
        for (int i = 0; i < quickSlot.Length; i++)
        {
            quickSlot[i].gameObject.SetActive(true);
            quickSlot[i].isSetSlot = true;
        }
        isSetSlotOn = true;
        GameObject.Find("Canvas").GetComponent<InventoryUI>().uiOnOff();
    }

    public void closeQuickSlots()
    {
        for (int i = 0; i < quickSlot.Length; i++)
        {
            quickSlot[i].isSetSlot = false;

            if (quickSlot[i].item == null || quickSlot[i].item.itemName.Length < 1)
            {
                quickSlot[i].gameObject.SetActive(false);
                isQuickSlotDataChanged = false;
            }
        }

        isSetSlotOn = false;
    }

    public void showInformation()
    {
        if (item == null || item.itemName.Length < 1 || isReinforceProgressing)
        {
            return;
        }

        itemInfomationUI.gameObject.SetActive(true);

        itemName.text = item.itemName;

        itemRating.text = "등급 : " + item.rating;
        itemContents.text = item.itemInfo;

        if (item.type == ItemType.Fish)
        {
            itemRating.text = "등급 : " + item.rating + "    길이 : " + item.size + "Cm";
            itemContents.text = item.itemInfo;
        }

        if (item.type == ItemType.Equipment)
        {
            if (item.reinforce > 0)
            {
                itemName.text += "  ( +" + item.reinforce + ")";
            }

            itemRating.text += "         타입 : " + PlayerEquipment.instance.getEquipmentTypeName(item.equipmentType);
            //itemStats.text = "================ 아이템 정보 ==============" + NEW_LINE;
            infoTag.SetActive(true);
            optionSize = 0;

            if (item.levelLimit != 0)
            {
                itemStats.text = "레벨제한 : " + item.levelLimit + NEW_LINE;
                optionSize++;
            }
            if (item.power != 0)
            {
                itemStats.text += "공격력 : " + (item.power + item.powerReinforce) + "( + " + item.powerReinforce + ")" + NEW_LINE;
                optionSize++;
            }
            if (item.armor != 0)
            {
                itemStats.text += "방어력 : " + (item.armor + item.armorReinforce) + "( + " + item.armorReinforce + ")" + NEW_LINE;
                optionSize++;
            }
            if (item.accuracy != 0)
            {
                itemStats.text += "명중률 : " + (item.accuracy + item.accuracyReinforce) + "( + " + item.accuracyReinforce + ")" + NEW_LINE;
                optionSize++;
            }
            if (item.avoid != 0)
            {
                itemStats.text += "회피율 : " + (item.avoid + item.avoidReinforce) + "( + " + item.avoidReinforce + ")" + NEW_LINE;
                optionSize++;
            }
            if (item.critRate != 0)
            {
                itemStats.text += "치명율 : " + Mathf.Round((item.critRate + item.critRateReinforce) * 10) / 10 + "%" + "( + " + Mathf.Round(item.critRateReinforce * 10) / 10 + "%)" + NEW_LINE;
                optionSize++;
            }
            if (item.critDam != 0)
            {
                itemStats.text += "치명피해 : " + Mathf.Round((item.critDam + item.critDamReinforce) * 10) / 10 + "%" + "( + " + Mathf.Round(item.critDamReinforce * 10) / 10 + "%)" + NEW_LINE;
                optionSize++;
            }
            if (item.satietyPoint != 0)
            {
                itemStats.text += "포만감 : " + (item.satietyPoint + item.satietyPointReinforce) + "( + " + item.satietyPointReinforce + ")" + NEW_LINE;
                optionSize++;
            }
            if (item.moisturePoint != 0)
            {
                itemStats.text += "갈증 : " + (item.moisturePoint + item.moisturePointReinforce) + "( + " + item.moisturePointReinforce + ")" + NEW_LINE;
                optionSize++;
            }
            if (item.catharsisPoint != 0)
            {
                itemStats.text += "배변 : " + (item.catharsisPoint + item.catharsisPointReinforce) + "( + " + item.catharsisPointReinforce + ")" + NEW_LINE;
                optionSize++;
            }
            if (item.fatiguePoint != 0)
            {
                itemStats.text += "피로도 : " + (item.fatiguePoint + item.fatiguePointReinforce) + "( + " + item.fatiguePointReinforce + ")" + NEW_LINE;
                optionSize++;
            }
            if (item.toolEff != 0)
            {
                itemStats.text += "도구 효율 : " + Mathf.Round((item.toolEff + item.toolEffReinforce) * 10) / 10 + "%" + "( + " + Mathf.Round(item.toolEffReinforce * 10) / 10 + "%)" + NEW_LINE;
                optionSize++;
            }
            if (item.skillEff != 0)
            {
                itemStats.text += "스킬 효율 : " + Mathf.Round((item.skillEff + item.skillEffReinforce) * 10) / 10 + "%" + "( + " + Mathf.Round(item.skillEffReinforce * 10) / 10 + "%)" + NEW_LINE;
                optionSize++;
            }
            if (item.expEff != 0)
            {
                itemStats.text += "경험치 보너스 : " + Mathf.Round((item.expEff + item.expEffReinforce) * 10) / 10 + "%" + "( + " + Mathf.Round(item.expEffReinforce * 10) / 10 + "%)" + NEW_LINE;
                optionSize++;
            }

            itemInfomationRect.sizeDelta = new Vector2(800, 580 + optionSize * 30);
        }
        else if (item.type == ItemType.Consumable || item.type == ItemType.Fish)
        {
            //itemStats.text = "================ 아이템 정보 ==============" + NEW_LINE;
            infoTag.SetActive(true);
            optionSize = 0;

            itemStats.text = "";

            if (item.itemEffect.saturationPoint != 0)
            {
                itemStats.text += "포만감 : " + item.itemEffect.saturationPoint + NEW_LINE;
                optionSize++;
            }
            if (item.itemEffect.moisturePoint != 0)
            {
                itemStats.text += "수분 : " + item.itemEffect.moisturePoint + NEW_LINE;
                optionSize++;
            }
            if (item.itemEffect.catharsisPoint != 0)
            {
                itemStats.text += "배변 : " + item.itemEffect.catharsisPoint + NEW_LINE;
                optionSize++;
            }
            if (item.itemEffect.fatiguePoint != 0)
            {
                itemStats.text += "피로도 : " + item.itemEffect.fatiguePoint + NEW_LINE;
                optionSize++;
            }

            itemInfomationRect.sizeDelta = new Vector2(800, 580 + optionSize * 30);
        }
        else
        {
            infoTag.SetActive(false);
            itemStats.text = null;
            itemInfomationRect.sizeDelta = new Vector2(800, 400);
        }

        itemMenuSet.SetActive(false);
    }

    public void offInformation()
    {
        itemInfomationUI.gameObject.SetActive(false);
    }

    public void throwAway()
    {
        if (!isPlayerInventory)
        {
            return;
        }

        Vector2 position = GameObject.Find("Player").gameObject.transform.position;
        position.y -= 1;
        ItemDatabase.instance.spawnItemByItem(position, item);
        PlayerInventory.instance.removeItem(slotNumber, item.count);
        GetComponent<StatUI>().isDataChanged = true;

        itemMenuSet.SetActive(false);
    }

    public void buttonsOnOff()
    {
        itemMenuSet.SetActive(true);

        if (item.type == ItemType.Equipment)
        {
            buttons[0].SetActive(true);
            buttons[2].SetActive(true);
            buttons[3].SetActive(true);
            buttons[4].SetActive(true);
            buttons[1].SetActive(false);
            buttons[5].SetActive(false);
            itemMenuSetRect.sizeDelta = new Vector2(320, 500);
        }
        else if (item.type == ItemType.Consumable)
        {
            buttons[1].SetActive(true);
            buttons[2].SetActive(true);
            buttons[3].SetActive(true);
            buttons[5].SetActive(true);
            buttons[0].SetActive(false);
            buttons[4].SetActive(false);
            itemMenuSetRect.sizeDelta = new Vector2(320, 500);
        }
        else if (item.type == ItemType.Etc)
        {
            buttons[2].SetActive(true);
            buttons[3].SetActive(true);
            buttons[0].SetActive(false);
            buttons[1].SetActive(false);
            buttons[4].SetActive(false);
            buttons[5].SetActive(false);
            itemMenuSetRect.sizeDelta = new Vector2(320, 280);
        }
        else if (item.type == ItemType.Fish)
        {
            buttons[1].SetActive(true);
            buttons[2].SetActive(true);
            buttons[3].SetActive(true);
            buttons[5].SetActive(true);
            buttons[0].SetActive(false);
            buttons[4].SetActive(false);
            itemMenuSetRect.sizeDelta = new Vector2(320, 500);
        }
        else
        {
            buttons[0].SetActive(false);
            buttons[1].SetActive(false);
            buttons[2].SetActive(false);
            buttons[3].SetActive(false);
            buttons[4].SetActive(false);
            buttons[5].SetActive(false);
        }
    }

    public void registerInReinforceUI()
    {
        if (item.type != ItemType.Equipment)
        {
            return;
        }

        ReinforceUI.instance.item = item;
        ReinforceUI.instance.slotNumber = slotNumber;

        reinforceSlot.item = item;
        reinforceSlot.slotNumber = slotNumber;
        reinforceSlot.updateSlotUI();

        GameObject.Find("Canvas").GetComponent<ReinforceUI>().uiOnOff();
        itemMenuSet.SetActive(false);
    }

    public void touchPanel()
    {

    }

    public void putInCount()
    {
        inputCount = int.Parse(inputField.text);
    }

    public void test()
    {
        Debug.Log("연결완료");
    }

    public void addZero()
    {
        if (touchPanelNumberString.Length > 3)
        {
            return;
        }

        touchPanelNumberString += "0";
    }

    public void addOne()
    {
        if (touchPanelNumberString.Length > 3)
        {
            return;
        }

        touchPanelNumberString += "1";
    }

    public void addTwo()
    {
        if (touchPanelNumberString.Length > 3)
        {
            return;
        }

        touchPanelNumberString += "2";
    }

    public void addThree()
    {
        if (touchPanelNumberString.Length > 3)
        {
            return;
        }

        touchPanelNumberString += "3";
    }

    public void addFour()
    {
        if (touchPanelNumberString.Length > 3)
        {
            return;
        }

        touchPanelNumberString += "4";
    }

    public void addFive()
    {
        if (touchPanelNumberString.Length > 3)
        {
            return;
        }

        touchPanelNumberString += "5";
    }

    public void addSix()
    {
        if (touchPanelNumberString.Length > 3)
        {
            return;
        }

        touchPanelNumberString += "6";
    }

    public void addSeven()
    {
        if (touchPanelNumberString.Length > 3)
        {
            return;
        }

        touchPanelNumberString += "7";
    }

    public void addEight()
    {
        if (touchPanelNumberString.Length > 3)
        {
            return;
        }

        touchPanelNumberString += "8";
    }

    public void addNine()
    {
        if (touchPanelNumberString.Length > 3)
        {
            return;
        }

        touchPanelNumberString += "9";
    }

    public void removeLastNumberText()
    {
        if (touchPanelNumberString != null)
        {
            touchPanelNumberString = "" + (int.Parse(touchPanelNumberString) / 10);
        }
    }
}