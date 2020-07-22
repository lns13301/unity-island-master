using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEquipment : MonoBehaviour
{
    public static PlayerEquipment instance;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    public delegate void OnChangeItem();
    public OnChangeItem onChangeItem;

    public int slotCnt = 10;
    public Item[] items = new Item[12];
    public Item previousItem;

    // equipment total
    public int reinforce;

    public int power;
    public int armor;
    public int accuracy;
    public int avoid;
    public float critRate;
    public float critDam;

    public float satietyPoint;
    public float moisturePoint;
    public float catharsisPoint;
    public float fatiguePoint;

    public float toolEff;
    public float skillEff;
    public float expEff;

    private DialogManager dialogManager;
    private Player playerScript;

    private void Start()
    {
        previousItem = null;
        dialogManager = GameObject.Find("DialogManager").GetComponent<DialogManager>();
        playerScript = GameObject.Find("Player").GetComponent<Player>();
    }

    private void registerMotion()
    {
        // 장비모션등록

        if (dialogManager.GetComponent<PlayerEquipment>().items[1] != null)
        {
            for (int i = 0; i < playerScript.leftHands.Length; i++)
            {
                if (playerScript.leftHands[i].name == ("" + dialogManager.GetComponent<PlayerEquipment>().items[1].code))
                {
                    playerScript.leftHands[i].SetActive(true);
                    playerScript.activeLeftHand = i;
                    break;
                }
                else
                {
                    playerScript.leftHands[i].SetActive(false);
                    playerScript.activeLeftHand = -1;
                }
            }

        }
        if (dialogManager.GetComponent<PlayerEquipment>().items[2] != null)
        {
            for (int i = 0; i < playerScript.rightHands.Length; i++)
            {
                if (playerScript.rightHands[i].name == ("" + dialogManager.GetComponent<PlayerEquipment>().items[2].code))
                {
                    playerScript.rightHands[i].SetActive(true);
                    playerScript.activeRightHand = i;
                    break;
                }
                else
                {
                    playerScript.rightHands[i].SetActive(false);
                    playerScript.activeRightHand = -1;
                }
            }

        }
        if (dialogManager.GetComponent<PlayerEquipment>().items[3] != null)
        {
            for (int i = 0; i < playerScript.heads.Length; i++)
            {
                if (playerScript.heads[i].name == ("" + dialogManager.GetComponent<PlayerEquipment>().items[3].code))
                {
                    playerScript.heads[i].SetActive(true);
                    playerScript.activeHead = i;
                    break;
                }
                else
                {
                    playerScript.heads[i].SetActive(false);
                    playerScript.activeHead = -1;
                }
            }

        }
        if (dialogManager.GetComponent<PlayerEquipment>().items[4] != null)
        {
            for (int i = 0; i < playerScript.tops.Length; i++)
            {
                if (playerScript.tops[i].name == ("" + dialogManager.GetComponent<PlayerEquipment>().items[4].code))
                {
                    playerScript.tops[i].SetActive(true);
                    playerScript.activeTop = i;
                    break;
                }
                else
                {
                    playerScript.tops[i].SetActive(false);
                    playerScript.activeTop = -1;
                }
            }

        }
        if (dialogManager.GetComponent<PlayerEquipment>().items[5] != null)
        {
            for (int i = 0; i < playerScript.pants.Length; i++)
            {
                if (playerScript.pants[i].name == ("" + dialogManager.GetComponent<PlayerEquipment>().items[5].code))
                {
                    playerScript.pants[i].SetActive(true);
                    playerScript.activePants = i;
                    break;
                }
                else
                {
                    playerScript.pants[i].SetActive(false);
                    playerScript.activePants = -1;
                }
            }

        }
        if (dialogManager.GetComponent<PlayerEquipment>().items[6] != null)
        {
            for (int i = 0; i < playerScript.gloves.Length; i++)
            {
                if (playerScript.gloves[i].name == ("" + dialogManager.GetComponent<PlayerEquipment>().items[6].code))
                {
                    playerScript.gloves[i].SetActive(true);
                    playerScript.activeGloves = i;
                    break;
                }
                else
                {
                    playerScript.gloves[i].SetActive(false);
                    playerScript.activeGloves = -1;
                }
            }

        }
        if (dialogManager.GetComponent<PlayerEquipment>().items[7] != null)
        {
            for (int i = 0; i < playerScript.shoes.Length; i++)
            {
                if (playerScript.shoes[i].name == ("" + dialogManager.GetComponent<PlayerEquipment>().items[7].code))
                {
                    playerScript.shoes[i].SetActive(true);
                    playerScript.activeShoes = i;
                    break;
                }
                else
                {
                    playerScript.shoes[i].SetActive(false);
                    playerScript.activeShoes = -1;
                }
            }

        }
        if (dialogManager.GetComponent<PlayerEquipment>().items[8] != null)
        {
            for (int i = 0; i < playerScript.necklesses.Length; i++)
            {
                if (playerScript.necklesses[i].name == ("" + dialogManager.GetComponent<PlayerEquipment>().items[8].code))
                {
                    playerScript.necklesses[i].SetActive(true);
                    playerScript.activeNeckless = i;
                    break;
                }
                else
                {
                    playerScript.tops[i].SetActive(false);
                    playerScript.activeNeckless = -1;
                }
            }

        }
        if (dialogManager.GetComponent<PlayerEquipment>().items[9] != null)
        {
            for (int i = 0; i < playerScript.earings.Length; i++)
            {
                if (playerScript.earings[i].name == ("" + dialogManager.GetComponent<PlayerEquipment>().items[9].code))
                {
                    playerScript.earings[i].SetActive(true);
                    playerScript.activeEaring = i;
                    break;
                }
                else
                {
                    playerScript.earings[i].SetActive(false);
                    playerScript.activeEaring = -1;
                }
            }

        }
        if (dialogManager.GetComponent<PlayerEquipment>().items[10] != null)
        {
            for (int i = 0; i < playerScript.rings.Length; i++)
            {
                if (playerScript.rings[i].name == ("" + dialogManager.GetComponent<PlayerEquipment>().items[10].code))
                {
                    playerScript.rings[i].SetActive(true);
                    playerScript.activeRing = i;
                    break;
                }
                else
                {
                    playerScript.rings[i].SetActive(false);
                    playerScript.activeRing = -1;
                }
            }

        }
        if (dialogManager.GetComponent<PlayerEquipment>().items[11] != null)
        {
            for (int i = 0; i < playerScript.hairs.Length; i++)
            {
                if (playerScript.hairs[i].name == ("" + dialogManager.GetComponent<PlayerEquipment>().items[11].code))
                {
                    playerScript.hairs[i].SetActive(true);
                    playerScript.activeHair = i;
                    break;
                }
                else
                {
                    playerScript.hairs[i].SetActive(false);
                    playerScript.activeHair = -1;
                }
            }

        }
    }

    private void disregisterMotion(int slotNumber)
    {
        // 장비모션등록
        if (slotNumber == 1)
        {
            playerScript.leftHands[playerScript.activeLeftHand].SetActive(false);
            playerScript.activeLeftHand = -1;
        }
        if (slotNumber == 2)
        {
            playerScript.rightHands[playerScript.activeRightHand].SetActive(false);
            playerScript.activeRightHand = -1;
        }
        if (slotNumber == 3)
        {
            playerScript.heads[playerScript.activeHead].SetActive(false);
            playerScript.activeHead = -1;
        }
        if (slotNumber == 4)
        {
            playerScript.tops[playerScript.activeTop].SetActive(false);
            playerScript.activeTop = -1;
        }
        if (slotNumber == 5)
        {
            playerScript.pants[playerScript.activePants].SetActive(false);
            playerScript.activePants = -1;
        }
        if (slotNumber == 6)
        {
            playerScript.gloves[playerScript.activeGloves].SetActive(false);
            playerScript.activeGloves = -1;
        }
        if (slotNumber == 7)
        {
            playerScript.shoes[playerScript.activeShoes].SetActive(false);
            playerScript.activeShoes = -1;
        }
        if (slotNumber == 8)
        {
            playerScript.tops[playerScript.activeNeckless].SetActive(false);
            playerScript.activeNeckless = -1;
        }
        if (slotNumber == 9)
        {
            playerScript.earings[playerScript.activeEaring].SetActive(false);
            playerScript.activeEaring = -1;
        }
        if (slotNumber == 10)
        {
            playerScript.rings[playerScript.activeRing].SetActive(false);
            playerScript.activeRing = -1;
        }
        if (slotNumber == 11)
        {
            playerScript.hairs[playerScript.activeHair].SetActive(false);
            playerScript.activeHair = -1;
        }
    }

    /*    public void removeMotion(EquipmentType slotType)
        {
            switch (slotType)
            {
                case EquipmentType.RightHand:
                    playerScript.nowRightHand = null;
                    break;
                case EquipmentType.Top:
                    playerScript.nowTop = null;
                    break;
                default:
                    break;
            }
        }*/

    public bool addItem(Item item, int slotNumber)
    {
        previousItem = null;
        int typeNumber = getEquipmentIndex(item.equipmentType);

        if (items[typeNumber] != null && items[typeNumber].itemName.Length > 0)
        {
            previousItem = items[typeNumber];
            Debug.Log("이전 아이템 이름 : " + previousItem.itemName);

            disregisterMotion(getEquipmentIndex(item.equipmentType));
        }
        items[typeNumber] = item;

        PlayerInventory.instance.removeItem(slotNumber);

        if (previousItem != null)
        {
            PlayerInventory.instance.addItem(previousItem);
        }

        instance.onChangeItem.Invoke();
        //PlayerInventory.instance.onChangeItem.Invoke();
        registerMotion();

        return true;
    }

    public bool addItem(Item item)
    {
        int typeNumber = getEquipmentIndex(item.equipmentType);
        items[typeNumber] = item;

        instance.onChangeItem.Invoke();
        //PlayerInventory.instance.onChangeItem.Invoke();
        registerMotion();

        return true;
    }

    public int getEquipmentIndex(EquipmentType type)
    {
        switch (type)
        {
            case EquipmentType.LeftHand:
                return 1;
            case EquipmentType.RightHand:
                return 2;
            case EquipmentType.TwoHands:
                return 2;
            case EquipmentType.Head:
                return 3;
            case EquipmentType.Top:
                return 4;
            case EquipmentType.Pants:
                return 5;
            case EquipmentType.Body:
                return 4;
            case EquipmentType.Gloves:
                return 6;
            case EquipmentType.Shoes:
                return 7;
            case EquipmentType.Neckless:
                return 8;
            case EquipmentType.Earing:
                return 9;
            case EquipmentType.Ring:
                return 10;
            case EquipmentType.Hair:
                return 11;
            default: return 0;
        }
    }

    public string getEquipmentTypeName(EquipmentType type)
    {
        switch (type)
        {
            case EquipmentType.LeftHand:
                return "왼손";
            case EquipmentType.RightHand:
                return "오른손";
            case EquipmentType.TwoHands:
                return "양손";
            case EquipmentType.Head:
                return "머리";
            case EquipmentType.Top:
                return "상의";
            case EquipmentType.Pants:
                return "하의";
            case EquipmentType.Body:
                return "전신";
            case EquipmentType.Gloves:
                return "장갑";
            case EquipmentType.Shoes:
                return "신발";
            case EquipmentType.Neckless:
                return "목걸이";
            case EquipmentType.Earing:
                return "귀고리";
            case EquipmentType.Ring:
                return "반지";
            case EquipmentType hair:
                return "머리카락";
            default: return "";
        }
    }

    public void removeItem(EquipmentType slotType, int quantity = 1)
    {
        int slotNumber = getEquipmentIndex(slotType);
        previousItem = null;

        // null 장비템 제거
        if (instance.items[slotNumber] == null || instance.items[slotNumber].itemName.Length < 1)
        {
            items[slotNumber] = null;
            onChangeItem.Invoke();
        }

        if (PlayerInventory.instance.items.Count >= PlayerInventory.instance.slotCount)
        {
            //sDebug.Log("인벤토리 용량초과로 장비 해제불가!");
            return;
        }

        if (items[slotNumber] != null)
        {
            previousItem = items[slotNumber];
            disregisterMotion(slotNumber);
            PlayerInventory.instance.addItem(previousItem);
        }
        items[slotNumber] = null;

        onChangeItem.Invoke();
        GameObject.Find("Canvas").GetComponent<StatUI>().isDataChanged = true;
    }

    public void updateTotalStats()
    {
        reinforce = 0;

        power = 0;
        armor = 0;
        accuracy = 0;
        avoid = 0;
        critRate = 0;
        critDam = 0;

        satietyPoint = 0;
        moisturePoint = 0;
        catharsisPoint = 0;
        fatiguePoint = 0;

        toolEff = 0;
        skillEff = 0;
        expEff = 0;

        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] != null)
            {
                putStats(items[i]);
            }
        }
        putStatsToPlayerData();
    }

    public void putStats(Item item)
    {
        reinforce += item.reinforce;

        power += item.power + item.powerReinforce;
        armor += item.armor + item.armorReinforce;
        accuracy += item.accuracy + item.accuracyReinforce;
        avoid += item.avoid + item.avoidReinforce;
        critRate += item.critRate + item.critRateReinforce;
        critDam += item.critDam + item.critDamReinforce;

        satietyPoint += item.satietyPoint + item.satietyPointReinforce;
        moisturePoint += item.moisturePoint + item.moisturePointReinforce;
        catharsisPoint += item.catharsisPoint + item.catharsisPointReinforce;
        fatiguePoint += item.fatiguePoint + item.fatiguePointReinforce;

        toolEff += item.toolEff + item.toolEffReinforce;
        skillEff += item.skillEff + item.skillEffReinforce;
        expEff += item.expEff + item.expEffReinforce;

        // 도구관련능력치도 추후 추가해야함!!!
    }

    public void putStatsToPlayerData()
    {
        dialogManager.playerData.powerEquipment = power;
        dialogManager.playerData.armorEquipment = armor;
        dialogManager.playerData.accuracyEquipment = accuracy;
        dialogManager.playerData.avoidEquipment = avoid;
        dialogManager.playerData.critRateEquipment = critRate;
        dialogManager.playerData.critDamEquipment = critDam;
        dialogManager.playerData.satietyPointEquipment = satietyPoint;
        dialogManager.playerData.moisturePointEquipment = moisturePoint;
        dialogManager.playerData.catharsisPointEquipment = catharsisPoint;
        dialogManager.playerData.fatiguePointEquipment = fatiguePoint;
        dialogManager.playerData.toolEffEquipment = toolEff;
        dialogManager.playerData.skillEffEquipment = skillEff;
        dialogManager.playerData.expEffEquipment = expEff;
    }
}
