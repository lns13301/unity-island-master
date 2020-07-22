using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SettingsButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public JoystickValue value;
    public void OnPointerDown(PointerEventData eventData)
    {
        value.cTouch = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        value.cTouch = false;
    }
}
