using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapUI : MonoBehaviour
{
    public GameObject mapSet;
    public bool isUIAnimationOn;
    public bool isUIAnimationOff;
    public int mapCode;
    public int mapDifficulty;

    public GameObject player;

    public Transform camera;
    public TitleController titleText;

    public Animator buttonMenuAnimator;

    public EntityInventoryData[] chests;

    // Start is called before the first frame update
    void Start()
    {
        mapSet.SetActive(false);
        isUIAnimationOn = false;
        isUIAnimationOff = false;
        mapCode = 0;
        player = GameObject.Find("Player").gameObject;
        camera = GameObject.Find("MainCamera").transform;

        mapSet.transform.localScale = new Vector3(0, 0, 1);
        titleText = GetComponent<TitleController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (isUIAnimationOn)
        {
            if (GameObject.Find("Canvas").GetComponent<UIAnimation>().openUI(mapSet))
            {
                isUIAnimationOn = false;
            }
        }

        if (isUIAnimationOff)
        {
            if (GameObject.Find("Canvas").GetComponent<UIAnimation>().closeUI(mapSet))
            {
                mapSet.SetActive(false);

                isUIAnimationOff = false;
            }
        }
    }

    public void uiOnOff()
    {
        // 던전에 있을 때 탈출구 표시
        if (SoundManager.instance.mapCode == 2)
        {
            Debug.Log("반복");
            transform.GetComponent<DungeonExit>().uiOnOff();
            return;
        }

        if (isUIAnimationOff || isUIAnimationOn)
        {
            return;
        }

        if (mapSet.activeSelf)
        {
            isUIAnimationOff = true;
        }
        else
        {
            if (GetComponent<StatUI>().statSet.activeSelf)
            {
                GetComponent<StatUI>().statSet.SetActive(false);
            }
            if (GetComponent<EquipmentUI>().equipmentSet.activeSelf)
            {
                GetComponent<EquipmentUI>().equipmentSet.SetActive(false);
            }

            mapSet.SetActive(true);

            isUIAnimationOn = true;
            buttonMenuAnimator.SetBool("isUIOn", true);
        }
    }

    public void exitUIOn()
    {
        if (SoundManager.instance.mapCode == 2)
        {
            transform.GetComponent<DungeonExit>().uiOn();
            return;
        }
    }

    public void exitUIOff()
    {
        transform.GetComponent<DungeonExit>().uiOff();
        return;
    }

    public void enterDungeon()
    {
        DungeonEnvironment.instance.clearField(mapCode);

        Invoke("enterDungeonSet", 0.1f);
    }

    private void enterDungeonSet()
    {
        switch (mapCode)
        {
            case 0:
                player.transform.position = new Vector2(-0.3f, -1f);
                camera.transform.position = new Vector2(-0.3f, -1f);
                titleText.title.text = "주둔지";
                mapDifficulty = -1;
                titleText.subTitle.text = showDifficulty();
                SoundManager.instance.mapCode = 0;
                break;
            case 1:
                break;
            case 2:
                player.transform.position = new Vector2(-1000.3f, 499f);
                camera.transform.position = new Vector2(-1000.3f, 499f);
                titleText.title.text = "우거진 숲";
                titleText.subTitle.text = showDifficulty();
                DungeonEnvironment.instance.spawnItem(mapCode, mapDifficulty);
                SoundManager.instance.mapCode = 2;
                break;
            case 3:
                break;
            default:
                break;
        }
        SoundManager.instance.refreshSounds();

        titleText.textTitleSet.SetActive(true);
        titleText.fadeIn();

        if (mapCode == 0)
        {
            exitUIOff();
            return;
        }

        //uiOnOff
        if (isUIAnimationOff || isUIAnimationOn)
        {
            return;
        }

        if (mapSet.activeSelf)
        {
            isUIAnimationOff = true;
        }
        else
        {
            if (GetComponent<StatUI>().statSet.activeSelf)
            {
                GetComponent<StatUI>().statSet.SetActive(false);
            }
            if (GetComponent<EquipmentUI>().equipmentSet.activeSelf)
            {
                GetComponent<EquipmentUI>().equipmentSet.SetActive(false);
            }

            mapSet.SetActive(true);

            isUIAnimationOn = true;
            buttonMenuAnimator.SetBool("isUIOn", true);
        }
    }

    public void getOut()
    {
        mapCode = 0;
        enterDungeon();
    }

    public string showDifficulty()
    {
        switch(mapDifficulty)
        {
            case 0:
                titleText.subTitle.color = new Color(0, 0.6f, 0.7f, 1);
                return "쉬움";
            case 1:
                titleText.subTitle.color = new Color(0.2f, 0.7f, 0, 1);
                return "보통";
            case 2:
                titleText.subTitle.color = new Color(0.9f, 0.2f, 0.05f, 1);
                return "어려움";
            default :
                titleText.subTitle.color = new Color(0.3f, 0.3f, 0.3f, 1);
                return "";
        }
    }
}
