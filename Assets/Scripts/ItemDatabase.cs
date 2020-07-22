using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ItemDatabase : MonoBehaviour
{
    public static ItemDatabase instance;
    public ItemDataFile itemDataFile;

    public string spritePath = "Images/Items/Images";
    public string effectsPath = "Effects/";

    public Dictionary<int, Item> itemDatas = new Dictionary<int, Item>();

    private void Awake()
    {
        instance = this;
    }

    public List<Item> itemDB = new List<Item>();

    public GameObject entityItemPrefab;
    public Vector2[] pos;

    private void Start()
    {
        itemDataFile = new ItemDataFile();
        itemDataFile.itemDatas = new List<Item>();

        loadItemData();
        //saveItemData();

        //spawnItem();

        // 딕셔너리에 아이템 정보 입력
        for (int i = 0; i < itemDB.Count; i++)
        {
            itemDatas.Add(itemDB[i].code, itemDB[i]);
        }
    }

    public void spawnItem()
    {
        for (int i = 0; i < pos.Length; i++)
        {
            GameObject go = Instantiate(entityItemPrefab, pos[i], Quaternion.identity);
            go.GetComponent<EntityItem>().setItem(itemDB[Random.Range(0, 8)]);
        }
    }

    public void spawnItemByItem(Vector2 position, Item item)
    {
        GameObject go = Instantiate(entityItemPrefab, position, Quaternion.identity);
        go.GetComponent<EntityItem>().item = item;
        go.GetComponent<EntityItem>().count = item.count;
        go.GetComponent<EntityItem>().setItem(itemDB[findItemDBPositionByCode(item.code)]);
    }

    public void spawnItemByCode(Vector2 position, int itemCode, int count = 1)
    {
        GameObject go = Instantiate(entityItemPrefab, position, Quaternion.identity);
        go.GetComponent<EntityItem>().item = findItemByCode(itemCode);
        go.GetComponent<EntityItem>().count = count;
        go.GetComponent<EntityItem>().setItem(itemDB[findItemDBPositionByCode(itemCode)]);
    }

    public int findItemDBPositionByCode(int code)
    {
        for (int i = 0; i < itemDB.Count; i++)
        {
            if (itemDB[i].code == code)
            {
                return i;
            }
        }

        return -1;
    }

    public Item findItemByName(string name)
    {
        for (int i = 0; i < itemDB.Count; i++)
        {
            if (itemDB[i].itemName == name)
            {
                return itemDB[i];
            }
        }
        return null;
    }

/*    public Item findItemByCode(int code)
    {
        for (int i = 0; i < itemDB.Count; i++)
        {
            if (itemDB[i].code == code)
            {
                return itemDB[i];
            }
        }
        return null;
    }*/

    public Item findItemByCode(int code)
    {
        return itemDatas[code];
    }

    public Item pickRandomItem()
    {
        return itemDB[Random.Range(0, itemDB.Count)];
    }

    [ContextMenu("From Json Data")]
    public Sprite loadSprite(string path)
    {
        return Resources.Load<Sprite>(path);
    }

    [ContextMenu("To Json Data")]
    public void saveItemData()
    {
        Debug.Log("저장 성공");
        itemDataFile.itemDatas = new List<Item>();

        itemDataFile.itemDatas.Add(new Item
            (1, 3000001, "나뭇가지", ItemType.Etc, EquipmentType.None, spritePath + "/" + 3000001, "평범", 0.1f, 100, 0, "불피우기에 유용한 잔가지이다."));
        itemDataFile.itemDatas.Add(new Item
            (1, 3000002, "마른 나뭇잎", ItemType.Etc, EquipmentType.None, spritePath + "/" + 3000002, "평범", 0.05f, 100, 0, "불이 매우 잘붙지만 금방 재가되버리는 나뭇잎이다."));
        itemDataFile.itemDatas.Add(new Item
            (1, 2000001, "구운 닭다리", ItemType.Consumable, EquipmentType.None, spritePath + "/" + 2000001, "평범",0.05f, 100, 0, "맛있게 익은 닭다리이다.", new ItemEffect(50, 0, 0, 0)));
        itemDataFile.itemDatas.Add(new Item
            (1, 2000002, "슬라임 점액", ItemType.Consumable, EquipmentType.None, spritePath + "/" + 2000002, "평범", 0.1f, 100, 0, "물컹해서 식감은 별로지만 나름 포만감있는 점액이다.", new ItemEffect(10, 0, 0, 0)));
        itemDataFile.itemDatas.Add(new Item
            (1, 2000003, "신선한 사과", ItemType.Consumable, EquipmentType.None, spritePath + "/" + "Apple", "평범", 0.2f, 100, 0, "한 입만 베어먹어도 상큼함과 신선함이 온몸에 전해지는 사과이다.", new ItemEffect(30, 30, 0, 0)));
        itemDataFile.itemDatas.Add(new Item
            (1, 2100001, "고등어", ItemType.Fish, EquipmentType.None, spritePath + "/" + "2100001", "평범", 0.5f, 100, 0, "등푸른 생선하면 떠오르는 모스트인 고등어이다. 생으로 먹으면 원기회복, 익혀 먹으면 허기회복에 도움이된다.", new ItemEffect(0, 0, 0, 20)));
        itemDataFile.itemDatas.Add(new Item
            (1, 1000001, "나무를 깎아만든 조악한 창", ItemType.Equipment, EquipmentType.RightHand, spritePath + "/" + 1000001, "비범", 3.5f, 1, 0, "어설프지만 끝을 뾰족하게 깎아서 꽤나 날카롭게 대상을 찌를 수 있는 나무 창이다.", null
            , 0, 3, 0, 12, -3, 5, -5));
        itemDataFile.itemDatas.Add(new Item
            (1, 1000002, "마릴린 소드", ItemType.Equipment, EquipmentType.RightHand, spritePath + "/" + 1000002, "비범", 6.5f, 1, 0, "단단한 철판을 연마하여 바닷물에 수없이 담금질하여 만들어진 검이다. 검에서는 푸르스름한 빛이 감돌고 있다. 검의 날이 푸석해지면, 바닷물에 잠시 담금게되면 다시 촉촉해지게되는 신비한 검이다.", null
            , 0, 50, 0, 85, -20, -5, -10, 0, 0, 0, 0, 0, 0, 0, 5));
        itemDataFile.itemDatas.Add(new Item
            (1, 1100001, "갑판으로 만든 철제 갑옷", ItemType.Equipment, EquipmentType.RightHand, spritePath + "/" + 1100001, "비범", 9.2f, 1, 0, "배의 철갑판을 분해하여 만든 꽤나 단단한 갑옷이다.", null
            , 0, 15, 0, -10, 40, 0, -7, 0, 0, 0, -10, -20, -10, 0, 5));

        string jsonData = JsonUtility.ToJson(itemDataFile, true);

        File.WriteAllText(saveOrLoad(false, true, "ItemData"), jsonData);
    }

    [ContextMenu("From Json Data")]
    public void loadItemData()
    {
        try
        {
            Debug.Log("아이템 정보 로드 성공");
            /*string jsonData = File.ReadAllText(saveOrLoad(false, false, "ItemData"));
            itemDataFile = JsonUtility.FromJson<ItemDataFile>(jsonData);*/

            itemDataFile = JsonUtility.FromJson<ItemDataFile>(Resources.Load<TextAsset>("ItemData").ToString());

            for (int i = 0; i < itemDataFile.itemDatas.Count; i++)
            {
                itemDataFile.itemDatas[i].sprite = loadSprite(itemDataFile.itemDatas[i].spritePath);
                itemDB.Add(itemDataFile.itemDatas[i]);
            }
        }
        catch (FileNotFoundException)
        {
            Debug.Log("로드 오류");

            string jsonData = JsonUtility.ToJson(itemDataFile, true);

            File.WriteAllText(saveOrLoad(false, false, "ItemData"), jsonData);
            loadItemData();
        }
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

    public Item makeItem(Item item)
    {
        return new Item(item.count, item.code, item.itemName, item.type, item.equipmentType, item.spritePath, item.rating, item.weight, item.countLimit,
            item.price, item.itemInfo, item.itemEffect, item.size, item.levelLimit, item.reinforce, item.power, item.armor, item.accuracy, item.avoid,
            item.critDam, item.critDam, item.satietyPoint, item.moisturePoint, item.catharsisPoint, item.fatiguePoint, item.toolEff, item.skillEff, item.expEff,
            item.powerReinforce, item.armorReinforce, item.accuracyReinforce, item.avoidReinforce, item.critRateReinforce, item.critDamReinforce,
            item.satietyPointReinforce, item.moisturePointReinforce, item.catharsisPointReinforce, item.fatiguePointReinforce, item.toolEffReinforce,
            item.skillEffReinforce, item.expEffReinforce, item.effecienty, item.speed, item.luck, item.bonus, item.ability, item.effecientyReinforce,
            item.speedReinforce, item.luckReinforce, item.bonusReinforce, item.abilityReinforce, item.handType, item.spriteAnimatorPath);
    }
}

[System.Serializable]
public class ItemDataFile
{
    public List<Item> itemDatas;
}
