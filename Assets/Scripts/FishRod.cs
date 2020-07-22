using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FishRod : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    public static FishRod instance;

    public float originPosX;
    public float originPosY;

    public float beginPosX;
    public float beginPosY;

    public Vector2 dragRangeStartPosition;
    public Vector2 bobberStartPosition;
    public Vector2 stringStartPosition;

    public Transform bobber;
    public Transform fishRodString;
    public RectTransform fishRodStringRect;

    public bool isCapturing;


    public void OnBeginDrag(PointerEventData eventData)
    {
        beginPosX = eventData.position.x;
        beginPosY = eventData.position.y;

        //transform.position = eventData.position;
        originPosX = bobber.position.x - 1080;
        originPosY = bobber.position.y - 635;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (eventData.position.x != beginPosX || eventData.position.y != beginPosY) 
        {
            bobber.position = new Vector2(bobber.position.x + eventData.position.x - beginPosX, bobber.position.y + eventData.position.y - beginPosY);
            beginPosX = eventData.position.x;
            beginPosY = eventData.position.y;
        }

        //transform.position = eventData.position;
        originPosX = bobber.position.x - 1080;
        originPosY = bobber.position.y - 635;

        fishRodString.position = new Vector2(bobber.position.x, fishRodString.position.y);

        float movementValue = -bobber.position.y + 860;
        fishRodStringRect.sizeDelta = new Vector2(4, movementValue);
    }

    public void OnDrop(PointerEventData eventData)
    {
        //transform.position = originPosition;
        isCapturing = false;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //transform.position = originPosition;
        isCapturing = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        originPosX = transform.position.x - 1180;
        originPosY = transform.position.y - 850;
        isCapturing = false;

        dragRangeStartPosition = transform.position;
        bobberStartPosition = bobber.position;
        stringStartPosition = fishRodString.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
