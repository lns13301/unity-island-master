using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using System;

public class HomeManager : MonoBehaviour
{
    public PlayerData playerData;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void loadAndPlay()
    {
        Debug.Log("저장된 게임 시작!");
        SceneManager.LoadScene(1);
        GameObject.Find("MainCamera").transform.position = new Vector2(
            GameObject.Find("DialogManager").GetComponent<DialogManager>().playerData.playerX, 
            GameObject.Find("DialogManager").GetComponent<DialogManager>().playerData.playerY);
    }

    public void startFromNew()
    {
        Debug.Log("새로 시작!");
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

        string jsonData = JsonUtility.ToJson(playerData, false);
        // pc 저장
        //string path = Path.Combine(Application.dataPath, "playerData.json");
        // 모바일 저장
        //string path = Path.Combine(Application.persistentDataPath, "playerData.json");
        File.WriteAllText(saveOrLoad(true, true), jsonData);
        SceneManager.LoadScene(1);
    }
    
    public string saveOrLoad(bool isMobile, bool isSave)
    {
        if (isSave)
        {
            if (isMobile)
            {
                // 모바일 저장
                return Path.Combine(Application.persistentDataPath, "playerData.json");
            }
            else
            {
                // pc 저장
                return Path.Combine(Application.dataPath, "playerData.json");
            }
        }
        else
        {
            if (isMobile)
            {
                // 모바일 로드
                return Path.Combine(Application.persistentDataPath, "playerData.json");
            }
            else
            {
                // pc 로드
                return Path.Combine(Application.dataPath, "playerData.json");
            }
        }
    }

    public void gameExit()
    {
        Application.Quit();
    }
}
