using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FishingUI : MonoBehaviour
{
    public static FishingUI instance;

    public GameObject fishingSet;
    public Animator buttonMenuAnimator;
    public GameObject fishingStartSet;

    public bool isCapture;
    public float captureTimer;
    public Text captureMessage;
    public float messageTimer;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        isCapture = false;
        captureTimer = 0;
        fishingSet.SetActive(false);
        captureMessage.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (isCapture)
        {
            captureTimer += Time.deltaTime;
            messageTimer += Time.deltaTime;

            if (captureTimer > 2)
            {
                Item capturedItem = ItemDatabase.instance.makeItem(ItemDatabase.instance.findItemByCode(2100001));

                FishingPlayerInventory.instance.addItem(capturedItem);
                captureTimer = 0;
                fishingStartSet.SetActive(false);

                captureMessage.gameObject.SetActive(true);
                captureMessage.text = capturedItem.itemName + "(을)를 잡았습니다!";

                isCapture = false;
            }
        }

        if (messageTimer > 2)
        {
            messageTimer += Time.deltaTime;
            
            if (messageTimer > 5)
            {
                captureMessage.gameObject.SetActive(false);
                messageTimer = 0;
            }
        }
    }

    public void uiOnOff()
    {
        if (fishingSet.activeSelf)
        {
            fishingSet.SetActive(false);
        }
        else
        {
            if (GetComponent<InventoryUI>().inventorySet.activeSelf)
            {
                GetComponent<InventoryUI>().inventorySet.SetActive(false);
            }
            if (GetComponent<StatUI>().statSet.activeSelf)
            {
                GetComponent<StatUI>().statSet.SetActive(false);
            }
            if (GetComponent<EquipmentUI>().equipmentSet.activeSelf)
            {
                GetComponent<EquipmentUI>().equipmentSet.SetActive(false);
            }
            if (GetComponent<ReinforceUI>().reinforceSet.activeSelf)
            {
                GetComponent<ReinforceUI>().reinforceSet.SetActive(false);
            }

            fishingSet.SetActive(true);
            buttonMenuAnimator.SetBool("isUIOn", true);
        }
    }
}
