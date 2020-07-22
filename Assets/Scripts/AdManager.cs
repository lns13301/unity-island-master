using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AdManager : MonoBehaviour
{
    // Ad
    public GameObject popUp;
    public Text popUpText;

    // Start is called before the first frame update
    void Start()
    {
        popUpText = popUp.transform.GetChild(1).GetComponent<Text>();
        popUp.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void giveReward()
    {
        popUp.gameObject.SetActive(true);

        if (PlayerInventory.instance.addItem(ItemDatabase.instance.pickRandomItem()))
        {
            popUpText.text = "보상이 성공적으로 지급되었습니다.";
        }
        else
        {
            popUpText.text = "소지품 공간이 부족하여 보상수령에 실패했습니다.";
        }
    }

    public void popUpUiOnOff()
    {
        if (popUp.gameObject.activeSelf)
        {
            popUp.gameObject.SetActive(false);
        }
    }
}
