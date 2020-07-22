using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FishingStart : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public FishingInventoryUI fishingInventoryUI;

    public Image startPanel;
    public GameObject fishGame;
    public bool isBtnDown;
    public bool isIncrease;
    public float power;

    void Start()
    {
        fishGame.SetActive(false);

        isBtnDown = false;
        isIncrease = true;
        power = 0;
    }

    void Update()
    {
        if (isBtnDown)
        {
            controlPower();
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isBtnDown = true;
        isIncrease = false;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isBtnDown = false;
        isIncrease = true;
        power = 0;

        fishGameUIOnOff();
    }

    public void fishGameUIOnOff()
    {
        if (fishGame.activeSelf)
        {
            fishGame.SetActive(false);
        }
        else
        {
            fishGame.SetActive(true);
            fishingInventoryUI.fishingInventorySet.SetActive(false);

            FishRod.instance.transform.position = FishRod.instance.dragRangeStartPosition;
            FishRod.instance.bobber.position = FishRod.instance.bobberStartPosition;
            FishRod.instance.fishRodString.position = FishRod.instance.stringStartPosition;
            FishRod.instance.fishRodStringRect.sizeDelta = new Vector2(4, 0);
        }
    }

    private void controlPower()
    {
        if (isIncrease)
        {
            power += 0.05f;
            startPanel.fillAmount = power;

            if (power >= 1)
            {
                isIncrease = false;
            }
        }
        else
        {
            power -= 0.05f;
            startPanel.fillAmount = power;

            if (power <= 0)
            {
                isIncrease = true;
            }
        }
    }
}
