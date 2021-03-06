﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChestInformation : MonoBehaviour
{
    public static ChestInformation instance;

    public static string NEW_LINE = "\n";
    public Item item;
    public int slotNumber;

    public GameObject chestInformationUI;
    public Text itemName;
    public Text itemRating;
    public Text itemStats;
    public Text itemContents;
    public Text itemLeftCount;
    public Text itemPrice;
    public Text itemSellPrice;

    public GameObject infoTag;
    public int optionSize;
    public RectTransform chestInformationUIRect;

    public GameObject notice;

    public GameObject buttonPurchase;
    public GameObject buttonSell;

    private void Start()
    {
        instance = this;
        noticeOff();
    }

    public void setItem(Item item, int slotNumber)
    {
        this.item = item;
        this.slotNumber = slotNumber;
    }

    public void showInformation()
    {
        if (item == null || item.itemName.Length < 1)
        {
            return;
        }

        chestInformationUI.SetActive(true);

        itemName.text = item.itemName;

        itemRating.text = "등급 : " + item.rating;
        itemContents.text = item.itemInfo;

        if (item.type != ItemType.Equipment)
        {
            itemLeftCount.text = "수량 : " + item.count + "개";
        }
        else
        {
            itemLeftCount.text = "";
        }

        if (item.price == 0)
        {

            if (GetComponent<ChestUI>().buyUI.activeSelf)
            {
                itemPrice.text = item.price + " Isle";
            }
            else
            {
                itemSellPrice.text = item.price + " Isle";
            }
        }
        else
        {
            if (GetComponent<ChestUI>().buyUI.activeSelf)
            {
                itemPrice.text = Calculator.numberToFormatting(item.price) + " Isle";
            }
            else
            {
                itemSellPrice.text = Calculator.numberToFormatting(item.price) + " Isle";
            }
        }

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

            chestInformationUIRect.sizeDelta = new Vector2(800, 580 + optionSize * 30);
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

            chestInformationUIRect.sizeDelta = new Vector2(800, 580 + optionSize * 30);
        }
        else
        {
            infoTag.SetActive(false);
            itemStats.text = null;
            chestInformationUIRect.sizeDelta = new Vector2(800, 400);
        }
    }

    public void offInformation()
    {
        chestInformationUI.SetActive(false);
    }

    public void purchaseItem()
    {
        int purchaseCount = TouchPad.instance.getNumber();

        if (purchaseCount == 0)
        {
            return;
        }

        // 플레이어 인벤토리에 아이템 추가할 수 있는지 검사할 때 아이템 수량 주소 참조 값이 변해버리므로 임시저장용 변수
        int tempCount = item.count;

        if (PlayerInventory.instance.addItem(item, purchaseCount))
        {
            item.count = tempCount;
            Debug.Log((EntityInventory.instance.items[slotNumber].count));
            EntityInventory.instance.removeItem(EntityInventory.instance.items[slotNumber], purchaseCount);

            offInformation();

            return;
        }

        item.count = tempCount;

        notice.SetActive(true);
    }

    public void sellItem()
    {
        int sellCount = TouchPad.instance.getNumber();

        if (sellCount == 0 || sellCount < item.count)
        {
            return;
        }

        int tempCount = sellCount;

        if (EntityInventory.instance.addItem(item, sellCount))
        {
            if (PlayerInventory.instance.removeItem(item, sellCount))
            {
                item.count = tempCount;

                offInformation();
                return;
            }

            EntityInventory.instance.removeItem(item, sellCount);
        }

        item.count = tempCount;
    }

    public void noticeOff()
    {
        notice.SetActive(false);
    }
}
