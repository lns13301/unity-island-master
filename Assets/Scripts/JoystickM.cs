using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class JoystickM : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    RectTransform rect;
    Vector2 touch = Vector2.zero;
    public RectTransform handle;
    public bool isFirst = false;

    float widthHalf;

    public JoystickValue value;

    private void Start()
    {
        rect = GetComponent<RectTransform>();
        widthHalf = rect.sizeDelta.x * 0.5f;
        if (!isFirst)
        {
            value.joyTouch = Vector2.zero;
            isFirst = true;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (GameObject.Find("Player").GetComponent<Player>().isKnockDown)
        {
            value.joyTouch = Vector2.zero;
            handle.anchoredPosition = Vector2.zero;
            return;
        }

        touch = (eventData.position - rect.anchoredPosition) / widthHalf;
        if (touch.magnitude > 1)
        {
            touch = touch.normalized;
        }
        value.joyTouch = touch;
        handle.anchoredPosition = touch * widthHalf;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // 상점 이용 시 버튼누르면 UI 종료
        closeUIs();

        OnDrag(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        // 상점 이용 시 버튼누르면 UI 종료
        closeUIs();

        value.joyTouch = Vector2.zero;
        handle.anchoredPosition = Vector2.zero;
    }

    private void closeUIs()
    {
        GameObject.Find("Canvas").GetComponent<ShopUI>().shopSet.SetActive(false);
        GameObject.Find("Canvas").GetComponent<ShopUI>().shopInformation.SetActive(false);
        GameObject.Find("Canvas").GetComponent<ShopInformation>().notice.SetActive(false);
        GameObject.Find("Canvas").GetComponent<ChestUI>().chestSet.SetActive(false);
        GameObject.Find("Canvas").GetComponent<ChestUI>().chestInformation.SetActive(false);
        GameObject.Find("Canvas").GetComponent<ChestInformation>().notice.SetActive(false);
        GameObject.Find("Canvas").GetComponent<ShopUI>().shopNotice.SetActive(false);
        GameObject.Find("Canvas").GetComponent<ChestUI>().chestNotice.SetActive(false);
    }
}
