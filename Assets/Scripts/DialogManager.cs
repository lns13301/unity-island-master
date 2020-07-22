﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Linq;
using System;
using UnityEngine.SceneManagement;

public class DialogManager : MonoBehaviour
{
    public TalkManager talkManager;
    public QuestManager questManager;
    public Image portraitImg;

    private Animator talkPanel;
    private Animator portraitAnim;
    public Sprite prevPortrait;
    private TypeEffect talkEffect;
    private Text NPCName;
    public Text questText;

    public Text pickUpText;
    public GameObject scanObject;
    public int talkIndex;
    public bool isAction;

    //gameManager
    public GameObject player;
    public PlayerData playerData;
    public SkillDataFile playerSkillData;

    public GameObject menuSet;

    //joystick
    public JoystickValue value;
    public int buttonTimer = 0;
    public GameObject ButtonsTop;

    //pcOrMobile, SaveOrLoad
    public bool isMobile = false;
    public bool doLoadInventory;
    public PlayerInventory playerInventory;
    public PlayerData playerInventoryItemsTemp;
    public PlayerEquipment playerEquipment;
    public PlayerData playerEquipmentItemsTemp;
    public FishingPlayerInventory fishingPlayerInventory;
    public PlayerData fishingPlayerInventoryItemsTemp;
    public ExpTable expTable;
    public TextAsset expTextAsset;
    public StatUI statUI;

    // UI
    public Text moneyText;

    // Quest
    public Quest nowQuest;
    public GameObject questStartButton;
    public GameObject questClearButton;
    public GameObject talkQuestPanel;
    public GameObject content;
    public bool isQuestTalk;
    private bool isDataChanged;
    public GameObject talkBackground;
    public GameObject tempScanObjectData;
    private bool isQuestExist;

    void Start()
    {
        loadPlayerDataFromJson();
        loadSkillDataFromJson();
        loadExpTable();

        talkManager = GameObject.Find("TalkManager").GetComponent<TalkManager>();
        questManager = GameObject.Find("QuestManager").GetComponent<QuestManager>();
        talkPanel = GameObject.Find("TalkSet").GetComponent<Animator>();
        portraitAnim = GameObject.Find("Portrait").GetComponent<Animator>();
        talkEffect = GameObject.Find("Interaction").GetComponent<TypeEffect>();
        NPCName = GameObject.Find("NPCName").GetComponent<Text>();
        playerInventory = GetComponent<PlayerInventory>();
        playerInventoryItemsTemp.items = playerInventory.items;
        playerEquipment = GetComponent<PlayerEquipment>();
        fishingPlayerInventory = GameObject.Find("ContentsManager").GetComponent<FishingPlayerInventory>();
        statUI = GameObject.Find("Canvas").GetComponent<StatUI>();

        questText.text = questManager.checkQuest();  // 여기를 수정해야 퀘스트 오류 고침
        questManager.questId = playerData.questId;
        questManager.questActionIndex = playerData.questActionIndex;

        menuSet.SetActive(false);
        player.transform.position = new Vector3(playerData.playerX, playerData.playerY, 0);

        talkPanel.SetBool("isShow", isAction);
        pickUpMessageClear();

        doLoadInventory = false;

        talkIndex = 0;
        isQuestTalk = false;
        isDataChanged = false;

        questSettings();
/*        for (int i = 0; i < playerData.inventorySize; i++)
        {
            PlayerInventory.instance.slotCount++;
        }*/
    }

    void Update()
    {
        levelUp();

        if (isDataChanged)
        {
            questSettings();
            isDataChanged = false;
        }

        if (Input.GetKeyDown(KeyCode.Escape) || (value.cTouch && buttonTimer == 0))
        {
            buttonTimer = 10;
            playerData.money += 20;
            if (menuSet.activeSelf)
            {
                menuSet.SetActive(false);
            }
            else
            {
                if (GameObject.Find("Canvas").GetComponent<InventoryUI>().inventorySet.activeSelf)
                {
                    GameObject.Find("Canvas").GetComponent<InventoryUI>().inventorySet.SetActive(false);
                }
                if (GameObject.Find("Canvas").GetComponent<StatUI>().statSet.activeSelf)
                {
                    GameObject.Find("Canvas").GetComponent<StatUI>().statSet.SetActive(false);
                }
                if (GameObject.Find("Canvas").GetComponent<EquipmentUI>().equipmentSet.activeSelf)
                {
                    GameObject.Find("Canvas").GetComponent<EquipmentUI>().equipmentSet.SetActive(false);
                }
                if (GameObject.Find("Canvas").GetComponent<ReinforceUI>().reinforceSet.activeSelf)
                {
                    GameObject.Find("Canvas").GetComponent<ReinforceUI>().reinforceSet.SetActive(false);
                }
                if (GameObject.Find("Canvas").GetComponent<FishingUI>().fishingSet.activeSelf)
                {
                    GameObject.Find("Canvas").GetComponent<FishingUI>().fishingSet.SetActive(false);
                }
                menuSet.SetActive(true);
            }
        }
        if (buttonTimer > 0)
        {
            buttonTimer--;
        }

        moneyText.text = "" + playerData.money;

        if (!doLoadInventory && playerData.inventorySize > 0)
        {
            doLoadInventory = true;
            putItemsFromInventoryData();
            putItemsFromEquipmentData();
            putItemsFromFishingInventoryData();
        }

        // 게이지 초과할 경우 최대값 보정
        if (playerData.satiety > playerData.satietyMax)
        {
            playerData.satiety = playerData.satietyMax;
        }
        if (playerData.moisture > playerData.moistureMax)
        {
            playerData.moisture = playerData.moistureMax;
        }
        if (playerData.catharsis > playerData.catharsisMax)
        {
            playerData.catharsis = playerData.catharsisMax;
        }
        if (playerData.fatigue > playerData.fatigueMax)
        {
            playerData.fatigue = playerData.fatigueMax;
        }

        if (playerData.satiety < 0)
        {
            playerData.satiety = 0;
        }
        if (playerData.moisture < 0)
        {
            playerData.moisture = 0;
        }
        if (playerData.catharsis < 0)
        {
            playerData.catharsis = 0;
        }
        if (playerData.fatigue < 0)
        {
            playerData.fatigue = 0;
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            questSettings();
        }
    }

    public void action(GameObject scanObj)
    {
        if (GameObject.Find("Canvas").GetComponent<ShopUI>().shopSet.activeSelf)
        {
            return;
        }

        tempScanObjectData = scanObj;

        ObjectData objData = scanObj.GetComponent<ObjectData>();

        talk(objData.id, objData.isNpc, objData);
        //talkBackground.SetActive(isAction);
        ButtonsTop.SetActive(!isAction);
        talkPanel.SetBool("isShow", isAction);
    }

    public void action()
    {
        ObjectData objData = tempScanObjectData.GetComponent<ObjectData>();
        talk(objData.id, objData.isNpc, objData);
        ButtonsTop.SetActive(!isAction);
        talkPanel.SetBool("isShow", isAction);
    }

    public void pickUpMessage(string itemName)
    {
        pickUpText.text = itemName + "을(를) 주웠습니다.";
    }

    public void pickUpMessageFull()
    {
        pickUpText.text = "더 이상 소지품을 획득할 공간이 없습니다.";
    }

    public void pickUpMessageClear()
    {
        pickUpText.text = null;
    }

    void talk(int id, bool isNpc, ObjectData objData)
    {
        int questTalkIndex = 0;
        string talkData = "";

        if (talkIndex == 0)
        {
            isQuestExist = false;
            talkQuestPanel.SetActive(true);

            for (int i = content.transform.childCount - 1; i >= 0; i--)
            {
                DestroyImmediate(content.transform.GetChild(i).gameObject);
            }

            // 시작가능 퀘스트를 대화 시 노출되도록 설정
            for (int i = 0; i < playerData.startQuest.Count; i++)
            {
                if (QuestDatabase.instance.questDB[playerData.startQuest[i]].npcIdStart == id)
                {
                    GameObject go = Instantiate(questStartButton);
                    go.transform.SetParent(content.transform);
                    go.GetComponent<QuestSelect>().setQuestId(QuestDatabase.instance.questDB[playerData.startQuest[i]].questId);

                    // 타이틀 제목
                    GameObject title = go.transform.GetChild(0).gameObject;
                    title.GetComponent<Text>().text = QuestDatabase.instance.questDB[playerData.startQuest[i]].questTitle;

                    isQuestExist = true;
                }
            }

            // 완료가능 퀘스트를 대화 시 노출되도록 설정
            for (int i = 0; i < playerData.currentQuest.Count; i++)
            {
                if (playerData.currentQuest[i].npcIdEnd == id && QuestDatabase.instance.isSatisfactionClearLimit(playerData.currentQuest[i]))
                {
                    GameObject go = Instantiate(questClearButton);
                    go.transform.SetParent(content.transform);
                    go.GetComponent<QuestSelect>().setQuestId(playerData.currentQuest[i].questId);

                    // 타이틀 제목
                    GameObject title = go.transform.GetChild(0).gameObject;
                    title.GetComponent<Text>().text = playerData.currentQuest[i].questTitle;

                    isQuestExist = true;
                }
            }

            if (isQuestExist)
            {
                talkQuestPanel.SetActive(true);
            }
            else
            {
                talkQuestPanel.SetActive(false);
            }
        }

        if (talkEffect.isAnimation)
        {
            talkEffect.setMessage("");
            return;
        }
        else
        {
            // 완료가능 퀘스트 대화 처리
            if (isQuestTalk && nowQuest.questState == QuestState.Proceeding)
            {
                talkData = nowQuest.getTalkDataEnd(talkIndex);

                if (talkData == null)
                {
                    // 퀘스트 요구 아이템 제거
                    for (int i = 0; i < nowQuest.questClearLimit.itemCode.Count; i++)
                    {
                        PlayerInventory.instance.removeItem(PlayerInventory.instance.findItemByCode(nowQuest.questClearLimit.itemCode[i]), QuestDatabase.instance.questDB[nowQuest.questId].questClearLimit.itemCount[i]);
                    }

                    // 퀘스트 보상 지급
                    for (int i = 0; i < nowQuest.questReword.itemCode.Count; i++)
                    {
                        PlayerInventory.instance.addItem(ItemDatabase.instance.findItemByCode(nowQuest.questReword.itemCode[i]), nowQuest.questReword.itemCount[i]);
                    }

                    playerData.exp += nowQuest.questReword.rewordExp;
                    playerData.money += nowQuest.questReword.rewordMoney;

                    statUI.GetComponent<StatUI>().isDataChanged = true;

                    isAction = false;
                    talkIndex = 0;
                    nowQuest.questState = QuestState.Complete;
                    playerData.clearQuest.Add(nowQuest.questId);
                    isQuestTalk = false;

                    QuestUI.instance.showRewordPanel(nowQuest);
                    nowQuest = null;

                    GameObject.Find(QuestDatabase.instance.findNpcNameByCode(id)).GetComponent<ObjectData>().setDoneQuestOff();
                    questSettings();
                    talkQuestPanel.SetActive(true);

                    return;
                }

                if (isNpc)
                {
                    talkEffect.setMessage(talkData.Split('$')[0]);

                    try
                    {
                        portraitImg.sprite = talkManager.getPortrait(id, int.Parse(talkData.Split('$')[1]));
                    }
                    catch
                    {
                        portraitImg.sprite = talkManager.getPortrait(id, 0);
                    }

                    portraitImg.color = new Color(1, 1, 1, 1);
                    if (prevPortrait != portraitImg.sprite)
                    {
                        portraitAnim.SetTrigger("doEffect");
                        prevPortrait = portraitImg.sprite;
                    }
                }
                else
                {
                    talkEffect.setMessage(talkData);

                    portraitImg.color = new Color(1, 1, 1, 0);
                }

                isAction = true;
                talkIndex++;

                return;
            }

            // 시작가능 퀘스트 대화 처리
            else if (isQuestTalk && nowQuest.questState == QuestState.Startable)
            {
                talkData = nowQuest.getTalkDataStart(talkIndex);

                if (talkData == null)
                {
                    isAction = false;
                    talkIndex = 0;
                    nowQuest.questState = QuestState.Proceeding;

                    playerData.currentQuest.Add(nowQuest);
                    isQuestTalk = false;
                    nowQuest = null;

                    GameObject.Find(QuestDatabase.instance.findNpcNameByCode(id)).GetComponent<ObjectData>().setNewQuestOff();
                    questSettings();
                    talkQuestPanel.SetActive(true);

                    return;
                }

                if (isNpc)
                {
                    talkEffect.setMessage(talkData.Split('$')[0]);

                    portraitImg.sprite = talkManager.getPortrait(id, int.Parse(talkData.Split('$')[1]));
                    portraitImg.color = new Color(1, 1, 1, 1);
                    if (prevPortrait != portraitImg.sprite)
                    {
                        portraitAnim.SetTrigger("doEffect");
                        prevPortrait = portraitImg.sprite;
                    }
                }
                else
                {
                    talkEffect.setMessage(talkData);

                    portraitImg.color = new Color(1, 1, 1, 0);
                }

                isAction = true;
                talkIndex++;

                return;
            }


            //일반 대화 처리
            talkData = talkManager.getTalk(id + questTalkIndex, talkIndex);
        }

        // 일반 대화 처리
        if (talkData == null)
        {
            isAction = false;
            talkIndex = 0;
            talkQuestPanel.SetActive(true);

            // 상점 처리
            if (objData.isShop)
            {
                objData.GetComponent<EntityInventoryData>().setEntityInventoryDataToEntityInventory();
                EntityInventory.instance.testPutItems();
                GameObject.Find("Canvas").GetComponent<ShopUI>().uiOnOff();
            }

            if (objData.isChest)
            {
                objData.GetComponent<EntityInventoryData>().setEntityInventoryDataToEntityInventory();
                GameObject.Find("Canvas").GetComponent<ChestUI>().uiOnOff();
            }

            return;
        }

        if (isNpc)
        {
            talkEffect.setMessage(talkData.Split('$')[0]);

            portraitImg.sprite = talkManager.getPortrait(id, int.Parse(talkData.Split('$')[1]));
            portraitImg.color = new Color(1, 1, 1, 1);
            if (prevPortrait != portraitImg.sprite)
            {
                portraitAnim.SetTrigger("doEffect");
                prevPortrait = portraitImg.sprite;
            }
        }
        else
        {
            talkEffect.setMessage(talkData);

            portraitImg.color = new Color(1, 1, 1, 0);
        }

        isAction = true;
        talkIndex++;
    }

    public void questTalk()
    {
        for (int i = content.transform.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(content.transform.GetChild(i).gameObject);
        }

        int newQuestCount = GameObject.Find(scanObject.name).GetComponent<ObjectData>().questStart.Count;
        //int doneQuestCount = GameObject.Find(scanObject.name).GetComponent<ObjectData>().questEnd.Count;

        for (int i = newQuestCount - 1; i >= 0; i--)
        {
            GameObject go = Instantiate(questStartButton);
            go.transform.SetParent(content.transform);

            // 타이틀 제목
            GameObject title = go.transform.GetChild(0).gameObject;
            title.GetComponent<Text>().text = QuestDatabase.instance.questDB[GameObject.Find(scanObject.name).GetComponent<ObjectData>().questStart[i]].questTitle;
        }
    }

    /* public bool checkQuestSatisfaction(int questTalkIndex)
     {
         for (int i = 0; i < talkManager.questInformations.Count; i++)
         {
             if (talkManager.questInformations[i].questNumber == questTalkIndex)
             {
                 // Find match quest index
                 for (int h = 0; h < playerData.questInformation.Count; h++) 
                 {
                     Debug.Log("실행 1");
                     if (playerData.questInformation[h].questNumber == questTalkIndex)
                     {
                         matchQuestIndex = h;
                     }
                     Debug.Log("맞는 퀘스트 번호 : " + matchQuestIndex);
                 }

                 // Item count check
                 for (int j = 0; j < talkManager.questInformations[i].itemCode.Count; j++)
                 {
                     Item item = PlayerInventory.instance.findItemByCode(talkManager.questInformations[i].itemCode[j]);

                     Debug.Log("필요수량 : " + talkManager.questInformations[i].itemCount[j] + "보유량 : " + PlayerInventory.instance.totalItemAmount(item));

                     if (talkManager.questInformations[i].itemCount[j] > PlayerInventory.instance.totalItemAmount(item))
                     {
                         return false;
                     }
                 }

                 // Mob kill count check
                 for (int j = 0; j < talkManager.questInformations[i].mobName.Count; j++)
                 {
                     if  (talkManager.questInformations[i].killCount[j] > playerData.questInformation[matchQuestIndex].killCount[j])
                     {
                         return false;
                     }
                 }

                 // visit check

             }
         }

         return true;
     }*/

    public void setNPCName(string name)
    {
        NPCName.text = name;
    }

    [ContextMenu("To Json Data")]
    public void savePlayerDataToJson()
    {
        Debug.Log("저장 성공");
        playerData.playerX = player.transform.position.x;
        playerData.playerY = player.transform.position.y;
        playerData.questId = questManager.questId;
        playerData.questActionIndex = questManager.questActionIndex;
        playerData.inventorySize = GetComponent<PlayerInventory>().slotCount;
        playerData.fishingInventorySize = GameObject.Find("ContentsManager").GetComponent<FishingPlayerInventory>().fishingSlotCount;
        playerData.items = PlayerInventory.instance.items;
        playerData.equipments = GetComponent<PlayerEquipment>().items;
        playerData.fishingItems = FishingPlayerInventory.instance.items;

        string jsonData = JsonUtility.ToJson(playerData, true);
        // pc 저장
        //string path = Path.Combine(Application.dataPath, "playerData.json");
        // 모바일 저장
        //string path = Path.Combine(Application.persistentDataPath, "playerData.json");
        File.WriteAllText(saveOrLoad(isMobile, true, "playerData"), jsonData);

        menuSet.SetActive(false);
    }

    public void saveAndLoadPlayerInventoryTemp()
    {
        playerInventoryItemsTemp.items = PlayerInventory.instance.items;

        string jsonData = JsonUtility.ToJson(playerInventoryItemsTemp, true);
        File.WriteAllText(saveOrLoad(isMobile, true, "playerInventoryItemsTemp"), jsonData);

        jsonData = File.ReadAllText(saveOrLoad(isMobile, false, "playerInventoryItemsTemp"));
        playerInventoryItemsTemp = JsonUtility.FromJson<PlayerData>(jsonData);
        PlayerInventory.instance.items = playerInventoryItemsTemp.items;
    }

    public void saveAndLoadPlayerEquipmentTemp()
    {
        playerEquipmentItemsTemp.equipments = GetComponent<PlayerEquipment>().items;

        string jsonData = JsonUtility.ToJson(playerEquipmentItemsTemp, true);
        File.WriteAllText(saveOrLoad(isMobile, true, "playerEquipmentItemsTemp"), jsonData);

        jsonData = File.ReadAllText(saveOrLoad(isMobile, false, "playerEquipmentItemsTemp"));
        playerEquipmentItemsTemp = JsonUtility.FromJson<PlayerData>(jsonData);
        GetComponent<PlayerEquipment>().items = playerEquipmentItemsTemp.equipments;
    }

    public void saveAndLoadPlayerFishingInventoryTemp()
    {
        fishingPlayerInventoryItemsTemp.items = FishingPlayerInventory.instance.items;

        string jsonData = JsonUtility.ToJson(fishingPlayerInventoryItemsTemp, true);
        File.WriteAllText(saveOrLoad(isMobile, true, "fishingPlayerInventoryItemsTemp"), jsonData);

        jsonData = File.ReadAllText(saveOrLoad(isMobile, false, "fishingPlayerInventoryItemsTemp"));
        fishingPlayerInventoryItemsTemp = JsonUtility.FromJson<PlayerData>(jsonData);
        FishingPlayerInventory.instance.items = fishingPlayerInventoryItemsTemp.items;
    }

    [ContextMenu("From Json Data")]
    public void loadPlayerDataFromJson()
    {
        try
        {
            Debug.Log("플레이어 정보 로드 성공");
            // pc 로드
            //string path = Path.Combine(Application.dataPath, "playerData.json");
            // 모바일 로드
            //string path = Path.Combine(Application.persistentDataPath, "playerData.json");
            string jsonData = File.ReadAllText(saveOrLoad(isMobile, false, "playerData"));
            playerData = JsonUtility.FromJson<PlayerData>(jsonData);

            playerData = Calculator.calcAll(playerData);

            // 버전 변경 시 스프라이트 이미지 코드가 변경되는 현상 막기
            for (int i = 0; i < playerData.items.Count; i++)
            {
                playerData.items[i].spritePath = ItemDatabase.instance.findItemByCode(playerData.items[i].code).spritePath;
                playerData.items[i].sprite = playerData.items[i].loadSprite(playerData.items[i].spritePath);

                if (playerData.items[i].spriteAnimatorPath != null)
                {
                    playerData.items[i].spriteAnimator = playerData.items[i].loadAnimator(playerData.items[i].spriteAnimatorPath);
                }
            }
        }
        catch (FileNotFoundException)
        {
            Debug.Log("로드 오류");
            playerData.playerX = -7f;
            playerData.playerY = -3f;
            playerData.questId = 100;
            playerData.questActionIndex = 0;
            playerData.inventorySize = 2;
            playerData.fishingInventorySize = 5;

            playerData.level = 1;
            playerData.statPoint = 3;
            playerData.strengthPoint = 0;
            playerData.endurancePoint = 0;
            playerData.intellectPoint = 0;
            playerData.concentrationPoint = 0;
            playerData.power = 0;
            playerData.armor = 0;
            playerData.accuracy = 0;
            playerData.avoid = 0;
            playerData.critRate = 0;
            playerData.critDam = 0;
            playerData.satietyMax = 100;
            playerData.moistureMax = 100;
            playerData.catharsisMax = 100;
            playerData.fatigueMax = 100;
            playerData.satiety = 100;
            playerData.moisture = 100;
            playerData.catharsis = 100;
            playerData.fatigue = 100;
            playerData.toolEff = 0;
            playerData.skillEff = 0;
            playerData.expEff = 0;
            playerData.fame = 0;
            playerData.charm = 0;
            playerData.weight = 0;
            playerData.weightMax = 10;

            playerData.items = new List<Item>();
            playerData.equipments = new Item[11];
            playerData.fishingItems = new List<Item>();

            playerData.startQuest = new List<int>();
            playerData.currentQuest = new List<Quest>();
            playerData.clearQuest = new List<int>();

            string jsonData = JsonUtility.ToJson(playerData, true);
            // pc 로드
            //string path = Path.Combine(Application.dataPath, "playerData.json");
            // 모바일 로드
            //string path = Path.Combine(Application.persistentDataPath, "playerData.json");
            File.WriteAllText(saveOrLoad(isMobile, false, "playerData"), jsonData);
            loadPlayerDataFromJson();
        }
    }

    [ContextMenu("To Json Data")]
    public void saveSkillDataToJson()
    {
        Debug.Log("스킬 저장 성공");

        string jsonData = JsonUtility.ToJson(playerSkillData, true);
        File.WriteAllText(saveOrLoad(isMobile, true, "playerSkillData"), jsonData);
    }

    [ContextMenu("From Json Data")]
    public void loadSkillDataFromJson()
    {
        try
        {
            Debug.Log("스킬 정보 로드 성공");
            string jsonData = File.ReadAllText(saveOrLoad(isMobile, false, "playerSkillData"));
            playerSkillData = JsonUtility.FromJson<SkillDataFile>(jsonData);
        }
        catch (FileNotFoundException)
        {
            Debug.Log("스킬 로드 오류");
            playerSkillData = new SkillDataFile();
            playerSkillData = SkillDatabase.instance.skillDataFile;
            playerSkillData.resetSkills();

            string jsonData = JsonUtility.ToJson(playerSkillData, true);
            File.WriteAllText(saveOrLoad(isMobile, true, "playerSkillData"), jsonData);
            loadSkillDataFromJson();
        }
    }

    public void putItemsFromInventoryData()
    {
        for (int i = 0; i < playerData.items.Count; i++)
        {
            playerInventory.addItem(playerData.items[i], playerData.items[i].count);
        }

        PlayerInventory.instance = playerInventory;
    }

    public void putItemsFromEquipmentData()
    {
        try
        {
            for (int i = 0; i < playerData.equipments.Length; i++)
            {
                playerEquipment.addItem(playerData.equipments[i]);
                if (playerEquipment.items[i].itemName.Length < 2 || playerEquipment.items[i].weight < 0.01f)
                {
                    playerEquipment.items[i] = null;
                }
            }
        }
        catch (NullReferenceException)
        {
            Debug.Log("저장된 장비데이터가 없습니다!");
        }
    }

    public void putItemsFromFishingInventoryData()
    {
        for (int i = 0; i < playerData.fishingItems.Count; i++)
        {
            fishingPlayerInventory.addItem(playerData.fishingItems[i], playerData.fishingItems[i].count);
        }

        FishingPlayerInventory.instance = fishingPlayerInventory;
    }

    public void environmentSettings()
    {
        Debug.Log("환경설정!");
    }

    public void gameExit()
    {
        Application.Quit();
    }

    public string saveOrLoad(bool isMobile, bool isSave, string fileName)
    {
        if (isSave)
        {
            if (isMobile)
            {
                // 모바일 저장
                return Path.Combine(Application.persistentDataPath, fileName + ".json");
            }
            else
            {
                // pc 저장
                return Path.Combine(Application.dataPath, fileName + ".json");
            }
        }
        else
        {
            if (isMobile)
            {
                // 모바일 로드
                return Path.Combine(Application.persistentDataPath, fileName + ".json");
            }
            else
            {
                // pc 로드
                return Path.Combine(Application.dataPath, fileName + ".json");
            }
        }
    }
    public void goToHome()
    {
        Debug.Log("저장된 게임 시작!");
        SceneManager.LoadScene(0);
    }

    [ContextMenu("From Json expTable Data")]
    public void loadExpTable()
    {
        Debug.Log("경험치 테이블 로드 성공");

        expTextAsset = Resources.Load<TextAsset>("GameData/expTable");
        expTable = JsonUtility.FromJson<ExpTable>(expTextAsset.ToString());
        playerData.nextExp = expTable.expTable[playerData.level - 1];
        GameObject.Find("Canvas").GetComponent<StatUI>().isDataChanged = true;
    }

    public void levelUp()
    {
        if (playerData.exp > expTable.expTable[playerData.level - 1] - 1)
        {
            playerData.exp -= expTable.expTable[playerData.level - 1];
            playerData.nextExp = expTable.expTable[playerData.level++];
            player.GetComponent<Player>().isLevelUp = true;
            playerData.statPoint += 3;
            statUI.isDataChanged = true;
        }
    }

    public void addExp(int exp)
    {
        playerData.exp += exp;
        statUI.isDataChanged = true;
    }

    // 시작조건에 부합하는 퀘스트를 정렬
    public void questSettings()
    {
        for (int i = 0; i < QuestDatabase.instance.questDB.Count; i++)
        {
            if (QuestDatabase.instance.isSatisfactQuestStartLimit(i, playerData.startQuest, playerData.currentQuest, playerData.clearQuest, playerData))
            {
                playerData.startQuest.Add(QuestDatabase.instance.questDB[i].questId);
            }
        }

        refreshQuest();
    }

    // 시작 가능한 퀘스트에 따라서 npc 머리위에 시작가능 퀘스트 여부를 띄움
    public void refreshQuest()
    {
        // 시작가능
        for (int i = 0; i < playerData.startQuest.Count; i++)
        {
            string npcName = QuestDatabase.instance.findNpcNameByCode(QuestDatabase.instance.questDB[playerData.startQuest[i]].npcIdStart);

            GameObject.Find(npcName).GetComponent<ObjectData>().tempQuestStart.Add(playerData.startQuest[i]);
            GameObject.Find(npcName).GetComponent<ObjectData>().setNewQuestOn();
            GameObject.Find(npcName).GetComponent<ObjectData>().isChangeData = true;
        }

        // 완료가능
        for (int i = 0; i < playerData.currentQuest.Count; i++)
        {
            if (QuestDatabase.instance.isSatisfactionClearLimit(playerData.currentQuest[i]))
            {
                string npcName = QuestDatabase.instance.findNpcNameByCode(QuestDatabase.instance.questDB[playerData.currentQuest[i].questId].npcIdEnd);

                GameObject.Find(npcName).GetComponent<ObjectData>().tempQuestEnd.Add(playerData.currentQuest[i].questId);
                GameObject.Find(npcName).GetComponent<ObjectData>().setDoneQuestOn();
                GameObject.Find(npcName).GetComponent<ObjectData>().isChangeData = true;
            }
        }
    }

    public void isDataChange()
    {
        isDataChanged = true;
    }
}

[System.Serializable]
public class PlayerData
{
    public float playerX;
    public float playerY;
    public int questId;
    public int questActionIndex;
    public string name;
    public int level;
    public int exp;
    public int nextExp;
    public int money;
    public int gold;
    public int inventorySize;
    public int fishingInventorySize;
    //public List<QuestInformation> questInformation;
    public List<Item> items;
    public Item[] equipments;
    public List<Item> fishingItems;

    //Quest
    public List<int> startQuest;
    public List<Quest> currentQuest;
    public List<int> clearQuest;

    //Stats
    public int statPoint;
    public int strengthPoint;
    public int endurancePoint;
    public int intellectPoint;
    public int concentrationPoint;

    public int power;
    public int armor;
    public int accuracy;
    public int avoid;
    public float critRate;
    public float critDam;

    public float satiety;
    public float satietyMax;
    public float moisture;
    public float moistureMax;
    public float catharsis;
    public float catharsisMax;
    public float fatigue;
    public float fatigueMax;

    public float toolEff;
    public float skillEff;
    public float expEff;

    public int fame;
    public int charm;
    public float weight;
    public float weightMax;

    //Equipments
    public int reinforce;

    public int powerEquipment;
    public int armorEquipment;
    public int accuracyEquipment;
    public int avoidEquipment;
    public float critRateEquipment;
    public float critDamEquipment;

    public float satietyPointEquipment;
    public float moisturePointEquipment;
    public float catharsisPointEquipment;
    public float fatiguePointEquipment;

    public float toolEffEquipment;
    public float skillEffEquipment;
    public float expEffEquipment;
}


[System.Serializable]
public class ExpTable
{
    public List<int> expTable;
}
