using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public JoystickValue value;
    public void OnPointerDown(PointerEventData eventData)
    {
        value.bTouch = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        value.bTouch = false;
    }
}
