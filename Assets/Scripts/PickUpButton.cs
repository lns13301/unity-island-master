﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;

public class PickUpButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public JoystickValue value;
    public Image currentImage;
    public Sprite[] sprites;

    private void Start()
    {
        currentImage = GetComponent<Image>();
    }

    private void Update()
    {
        try
        {
            if (GameObject.Find("Player").GetComponent<Player>().scanObject.tag == "Entity")
            {
                currentImage.sprite = sprites[1];
            }
            else if (GameObject.Find("Player").GetComponent<Player>().scanObject.tag == "Door")
            {
                currentImage.sprite = sprites[2];
            }
            else  if (GameObject.Find("Player").GetComponent<Player>().scanObject.tag == "Box")
            {
                currentImage.sprite = sprites[2];
            }
            else
            {
                currentImage.sprite = sprites[0];
            }
        }
        catch (NullReferenceException)
        {
            currentImage.sprite = sprites[0];
        }
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        value.dTouch = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        value.dTouch = false;
    }
}
