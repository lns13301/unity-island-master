using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectData : MonoBehaviour
{
    public int id;
    public bool isNpc;
    public bool isShop;
    public bool isChest;
    public bool isCollecting;
    public int shopSlotCount = 10;
    public string objectName;

    public List<int> questStart;
    public List<int> tempQuestStart;
    public List<int> questEnd;
    public List<int> tempQuestEnd;
    public bool isChangeData;

    public GameObject newQuest;
    public GameObject doneQuest;

    public GameObject canvas;
    public GameObject hud;

    public int itemCode;
    public int itemCount;

    public string getName()
    {
        return objectName;
    }

    private void Start()
    {
        questStart = new List<int>();
        questEnd = new List<int>();
        tempQuestStart = new List<int>();
        tempQuestEnd = new List<int>();
        isChangeData = false;

        if (isNpc)
        {
            newQuest = transform.GetChild(0).gameObject;
            doneQuest = transform.GetChild(1).gameObject;
            newQuest.SetActive(false);
            doneQuest.SetActive(false);
        }

        isCollecting = false;

        //putQuestToEntity();
    }

    private void Update()
    {
        if (isCollecting)
        {
            collectThis();
        }
    }

    public void setNewQuestOn()
    {
        newQuest.SetActive(true);
    }

    public void setNewQuestOff()
    {
        newQuest.SetActive(false);
    }

    public void setDoneQuestOn()
    {
        setNewQuestOff();
        doneQuest.SetActive(true);
    }

    public void setDoneQuestOff()
    {
        doneQuest.SetActive(false);
    }

    public void refresh()
    {
        if (!isChangeData)
        {
            return;
        }

        questStart = tempQuestStart;
        tempQuestStart = new List<int>();
        questEnd = tempQuestEnd;
        tempQuestEnd = new List<int>();

        isChangeData = false;
    }

    public void collectThis()
    {
        if (GameObject.Find("Player").GetComponent<Player>().collectAnimationProgress())
        {
            Item instanceItem = ItemDatabase.instance.findItemByCode(itemCode);
            instanceItem.count = itemCount;

            if (PlayerInventory.instance.addItem(instanceItem))
            {
                GameObject.Find("DialogManager").GetComponent<DialogManager>().pickUpMessage(instanceItem.itemName);
                GameObject.Find("Player").GetComponent<Player>().isPickedItem = true;
                GameObject.Find("DialogManager").GetComponent<DialogManager>().playerData.moisture -= instanceItem.weight;
                Destroy(gameObject);
            }
            else
            {
                GameObject.Find("DialogManager").GetComponent<DialogManager>().pickUpMessageFull();
            }
        }
    }

    /*    public void putQuestToEntity()
        {
            for (int i = 0; i < QuestDatabase.instance.questDB.Count; i++)
            {
                if (QuestDatabase.instance.questDB[i].npcIdStart == id)
                {
                    questStart.Add(QuestDatabase.instance.questDB[i]);
                }

                if (QuestDatabase.instance.questDB[i].npcIdEnd == id)
                {
                    questEnd.Add(QuestDatabase.instance.questDB[i]);
                }
            }
        }*/
}
