using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonMenu : MonoBehaviour
{
    public Animator animator;
    public ReinforceSlot reinforceSlot;
    public GameObject buttonMenu;
    public GameObject buttonMenuArrow;

    void Start()
    {
        buttonMenuArrow.GetComponent<Image>().enabled = false;
        buttonMenu.GetComponent<Image>().enabled = true;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            uiOnOff();
        }

        if (!animator.GetBool("isMenuOn") && !animator.GetBool("isUIOn"))
        {
            buttonMenuArrow.GetComponent<Image>().enabled = false;
            buttonMenu.GetComponent<Image>().enabled = true;
        }
        else
        {
            buttonMenu.GetComponent<Image>().enabled = false;
            buttonMenuArrow.GetComponent<Image>().enabled = true;
        }
    }

    public void uiOnOff()
    {
        if (ItemMenuSet.instance.isSetSlotOn)
        {
            ItemMenuSet.instance.closeQuickSlots();
        }

        // 상점 이용 시 버튼누르면 UI 종료
        GameObject.Find("Canvas").GetComponent<ShopUI>().shopSet.SetActive(false);
        GameObject.Find("Canvas").GetComponent<ShopUI>().shopInformation.SetActive(false);

        if (GameObject.Find("Canvas").GetComponent<ItemMenuSet>().isReinforceProgressing)
        {
            return;
        }

        if (animator.GetBool("isUIOn"))
        {
            if (GetComponent<StatUI>().statSet.activeSelf)
            {
                GetComponent<StatUI>().statSet.SetActive(false);
            }
            if (GetComponent<EquipmentUI>().equipmentSet.activeSelf)
            {
                GetComponent<EquipmentUI>().equipmentSet.SetActive(false);
            }
            if (GetComponent<InventoryUI>().inventorySet.activeSelf)
            {
                GetComponent<InventoryUI>().inventorySet.SetActive(false);
            }
            if (GetComponent<ReinforceUI>().reinforceSet.activeSelf)
            {
                GetComponent<ReinforceUI>().reinforceSet.SetActive(false);
            }
            if (GetComponent<FishingUI>().fishingSet.activeSelf)
            {
                GetComponent<FishingUI>().fishingSet.SetActive(false);
            }
            if (FishingUI.instance.fishingStartSet.activeSelf)
            {
                FishingUI.instance.fishingStartSet.SetActive(false);
            }
            GetComponent<FishingInventoryUI>().fishingInventorySet.SetActive(false);

            if (GetComponent<QuestUI>().questSet.activeSelf)
            {
                GetComponent<QuestUI>().questSet.SetActive(false);
            }
            if (GetComponent<QuestUI>().questInformationPanel.activeSelf)
            {
                GetComponent<QuestUI>().questInformationPanel.SetActive(false);
            }
            if (GetComponent<ShopUI>().shopSet.activeSelf)
            {
                GetComponent<ShopUI>().shopSet.SetActive(false);
            }
            if (GetComponent<MapUI>().mapSet.activeSelf)
            {
                GetComponent<MapUI>().mapSet.SetActive(false);
            }
            if (GetComponent<SkillUI>().skillSet.activeSelf)
            {
                GetComponent<SkillUI>().skillSet.SetActive(false);
            }

            animator.SetBool("isUIOn", false);
        }
        else if (!animator.GetBool("isMenuOn") && !animator.GetBool("isUIOn"))
        {
            animator.SetBool("isMenuOn", true);
        }
        else if (animator.GetBool("isMenuOn") && !animator.GetBool("isUIOn"))
        {
            animator.SetBool("isMenuOn", false);
        }

        if (reinforceSlot.item != null)
        {
            reinforceSlot.item = null;
        }

        reinforceSlot.removeSlotUI();

        GameObject.Find("Canvas").GetComponent<ItemMenuSet>().item = null;
        GameObject.Find("Canvas").GetComponent<ItemMenuSet>().itemInfomationUI.gameObject.SetActive(false);
        GetComponent<ItemMenuSet>().itemMenuSet.SetActive(false);
    }
}
