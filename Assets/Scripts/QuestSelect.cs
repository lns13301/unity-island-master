using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestSelect : MonoBehaviour
{
    public int questId;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void selectStartQuest()
    {
        GameObject.Find("DialogManager").GetComponent<DialogManager>().nowQuest = QuestDatabase.instance.makeQuest(questId);
        GameObject.Find("DialogManager").GetComponent<DialogManager>().playerData.startQuest.Remove(questId);
        GameObject.Find("DialogManager").GetComponent<DialogManager>().isQuestTalk = true;
        GameObject.Find("DialogManager").GetComponent<DialogManager>().talkIndex = 0;
        GameObject.Find("DialogManager").GetComponent<DialogManager>().talkQuestPanel.SetActive(false);
        GameObject.Find("Player").GetComponent<Player>().questTalkStart();

        /*        for (int i = 0; i < GameObject.Find("DialogManager").GetComponent<DialogManager>().playerData.startQuest.Count; i++)
                {
                    if (GameObject.Find("DialogManager").GetComponent<DialogManager>().playerData.startQuest[i] == questId)
                    {
                        GameObject.Find("DialogManager").GetComponent<DialogManager>().playerData.startQuest.RemoveAt(i);
                    }
                }*/
    }

    public void selectClearQuest()
    {
        // 인벤토리에 보상받을 공간없을 때
        if (PlayerInventory.instance.slotCount - PlayerInventory.instance.items.Count < QuestDatabase.instance.questDB[questId].questReword.itemCode.Count)
        {
            //Debug.Log("슬롯 : " + PlayerInventory.instance.slotCount + "   아이템 보유수 : " + PlayerInventory.instance.items.Count + "   퀘스트 보상수 : " + QuestDatabase.instance.questDB[questId].questReword.itemCode.Count);
            QuestUI.instance.showSpaceLimitMessage(QuestDatabase.instance.questDB[questId].questReword.itemCode.Count
                - PlayerInventory.instance.slotCount - PlayerInventory.instance.items.Count);

            return;
        }

        for (int i = 0; i < GameObject.Find("DialogManager").GetComponent<DialogManager>().playerData.currentQuest.Count; i++)
        {
            if (GameObject.Find("DialogManager").GetComponent<DialogManager>().playerData.currentQuest[i].questId == questId)
            {
                GameObject.Find("DialogManager").GetComponent<DialogManager>().nowQuest = GameObject.Find("DialogManager").GetComponent<DialogManager>().playerData.currentQuest[i];
            }
        }
        
        for (int i = 0; i < GameObject.Find("DialogManager").GetComponent<DialogManager>().playerData.currentQuest.Count; i++)
        {
            if (GameObject.Find("DialogManager").GetComponent<DialogManager>().playerData.currentQuest[i].questId == questId)
            {
                GameObject.Find("DialogManager").GetComponent<DialogManager>().playerData.currentQuest.RemoveAt(i);
            }
        }

        GameObject.Find("DialogManager").GetComponent<DialogManager>().isQuestTalk = true;
        GameObject.Find("DialogManager").GetComponent<DialogManager>().talkIndex = 0;
        GameObject.Find("DialogManager").GetComponent<DialogManager>().talkQuestPanel.SetActive(false);
        GameObject.Find("Player").GetComponent<Player>().questTalkStart();

        /*        for (int i = 0; i < GameObject.Find("DialogManager").GetComponent<DialogManager>().playerData.startQuest.Count; i++)
                {
                    if (GameObject.Find("DialogManager").GetComponent<DialogManager>().playerData.startQuest[i] == questId)
                    {
                        GameObject.Find("DialogManager").GetComponent<DialogManager>().playerData.startQuest.RemoveAt(i);
                    }
                }*/
    }

    public void setQuestId(int id)
    {
        questId = id;
    }

    public void test()
    {
        Debug.Log("Hi");
    }
}
