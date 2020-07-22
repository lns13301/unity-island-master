using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExitZone : MonoBehaviour
{
    public string exitName;
    public float exitTime;
    private float time;
    public ExitType exitType;
    public Text showTime;

    public bool isPlayerIn;

    // Start is called before the first frame update
    void Start()
    {
        isPlayerIn = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlayerIn)
        {
            time += Time.deltaTime;
            showTime.text = Mathf.Ceil((exitTime - time) * 10) / 10 + " 초 남음";

            if (exitTime < time)
            {
                time = 0;
                showTime.text = getExitInformation();
                GameObject.Find("Canvas").GetComponent<MapUI>().getOut();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            isPlayerIn = true;
            GameObject.Find("Canvas").GetComponent<MapUI>().exitUIOn();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            isPlayerIn = false;
            showTime.text = getExitInformation();
            time = 0;
            isPlayerIn = false;
            GameObject.Find("Canvas").GetComponent<MapUI>().exitUIOff();
        }
    }

    public string getExitInformation()
    {
        string exitInformation = "";

        switch(exitType)
        {
            case ExitType.Able:
                exitInformation = "탈출 가능";
                showTime.color = new Color(0f, 1f, 0f);
                break;
            case ExitType.Disable:
                exitInformation = "탈출 불가능";
                showTime.color = new Color(1f, 0.33f, 0.33f);
                break;
            case ExitType.NeedItem:
                exitInformation = "준비물 필요";
                showTime.color = new Color(1f, 0.5f, 1f);
                break;
            case ExitType.NeedQuest:
                exitInformation = "퀘스트 완료";
                showTime.color = new Color(1f, 0.8f, 0f);
                break;
            case ExitType.Random:
                exitInformation = "확률 탈출 가능";
                showTime.color = new Color(0f, 0.9f, 1f);
                break;
            default:
                exitInformation = "??????";
                showTime.color = new Color(1f, 1f, 1f);
                break;
        }

        return exitInformation;
    }
}