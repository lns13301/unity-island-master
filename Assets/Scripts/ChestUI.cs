using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChestUI : MonoBehaviour
{
    public EntityInventory entityInventory;
    public PlayerInventory playerInventory;

    public GameObject chestSet;
    public GameObject chestNotice;
    public GameObject chestInformation;

    public ChestSlot[] slotsBuy;
    public ChestSlot[] slotsSell;
    public Transform slotHolderBuy;
    public Transform slotHolderSell;

    public GameObject buyUI;
    public GameObject sellUI;

    public Text money;

    void Start()
    {
        entityInventory = EntityInventory.instance;
        playerInventory = PlayerInventory.instance;

        slotHolderBuy = chestSet.transform.Find("Buy").GetChild(0).GetChild(0);
        slotHolderSell = chestSet.transform.Find("Sell").GetChild(0).GetChild(0);

        buyUI = chestSet.transform.Find("Buy").gameObject;
        sellUI = chestSet.transform.Find("Sell").gameObject;

        slotsBuy = slotHolderBuy.GetComponentsInChildren<ChestSlot>();
        slotsSell = slotHolderSell.GetComponentsInChildren<ChestSlot>();

        entityInventory.onSlotCountChange += slotChange;
        entityInventory.onChangeItem += redrawSlotUI;

        playerInventory.onSlotCountChange += slotChange;
        playerInventory.onChangeItem += redrawSlotUI;

        chestSet.SetActive(false);
        chestInformation.SetActive(false);

        chestNotice.gameObject.SetActive(false);
    }

    private void slotChange(int val)
    {
        //buy
        for (int i = 1; i < slotsBuy.Length; i++)
        {
            slotsBuy[i].slotNumber = i;

            if (i < entityInventory.slotCount)
            {
                slotsBuy[i].GetComponent<Button>().interactable = true;
            }
            else
            {
                slotsBuy[i].GetComponent<Button>().interactable = false;
            }
        }
        //sell
        for (int i = 1; i < slotsSell.Length; i++)
        {
            slotsSell[i].slotNumber = i;

            if (i < playerInventory.slotCount)
            {
                slotsSell[i].GetComponent<Button>().interactable = true;
            }
            else
            {
                slotsSell[i].GetComponent<Button>().interactable = false;
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
        money.text = "" + GameObject.Find("DialogManager").GetComponent<DialogManager>().playerData.money;

        if (chestSet.activeSelf)
        {
            chestSet.SetActive(false);
            chestInformation.SetActive(false);
            chestNotice.SetActive(false);

            return;
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

            chestSet.SetActive(true);
            redrawSlotUI();

            buyUI.SetActive(false);
            sellUI.SetActive(false);

            chestNotice.SetActive(true);
        }
    }

    public void addSlot()
    {
        entityInventory.slotCount++;
    }

    void redrawSlotUI()
    {
        //buy
        for (int i = 0; i < slotsBuy.Length; i++)
        {
            slotsBuy[i].removeSlotUI();
        }
        for (int i = 0; i < entityInventory.items.Count; i++)
        {
            slotsBuy[i].item = entityInventory.items[i];
            slotsBuy[i].updateSlotUI();
        }

        //sell
        for (int i = 0; i < playerInventory.slotCount; i++)
        {
            slotsSell[i].removeSlotUI();
        }
        for (int i = 0; i < playerInventory.items.Count; i++)
        {
            slotsSell[i].item = playerInventory.items[i];
            slotsSell[i].updateSlotUI();
        }
    }

    public void testSetItemPowerUp()
    {
        entityInventory.items[0].power = 999;
    }

    public void openBuyUI()
    {

        buyUI.SetActive(true);
        sellUI.SetActive(false);
        chestNotice.gameObject.SetActive(false);

        GetComponent<ChestInformation>().buttonPurchase.SetActive(true);
        GetComponent<ChestInformation>().buttonSell.SetActive(false);
    }

    public void openSellUI()
    {
        buyUI.SetActive(false);
        sellUI.SetActive(true);
        chestNotice.gameObject.SetActive(false);

        GetComponent<ChestInformation>().buttonPurchase.SetActive(false);
        GetComponent<ChestInformation>().buttonSell.SetActive(true);
    }

    public void back()
    {
        buyUI.SetActive(false);
        sellUI.SetActive(false);
        chestNotice.gameObject.SetActive(true);
        ChestInformation.instance.offInformation();

        GetComponent<ChestInformation>().buttonPurchase.SetActive(false);
        GetComponent<ChestInformation>().buttonSell.SetActive(false);
    }

    public void chestNoticeOff()
    {
        chestNotice.gameObject.SetActive(false);
        chestSet.SetActive(false);
    }
}
