using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item
{
    public int count;

    public int code;
    public string itemName;
    public ItemType type;
    public EquipmentType equipmentType;
    public string spritePath;
    public string rating;
    public float weight;
    public int countLimit;
    public int price;
    public string itemInfo;


    // Item effect
    public ItemEffect itemEffect;

    public Sprite sprite;

    //Fish
    public float size;

    // equipment
    public int levelLimit;
    public int reinforce;

    public int power;
    public int armor;
    public int accuracy;
    public int avoid;
    public float critRate;
    public float critDam;

    public float satietyPoint;
    public float moisturePoint;
    public float catharsisPoint;
    public float fatiguePoint;

    public float toolEff;
    public float skillEff;
    public float expEff;

    // Reinforce Amount

    public int powerReinforce;
    public int armorReinforce;
    public int accuracyReinforce;
    public int avoidReinforce;
    public float critRateReinforce;
    public float critDamReinforce;

    public float satietyPointReinforce;
    public float moisturePointReinforce;
    public float catharsisPointReinforce;
    public float fatiguePointReinforce;

    public float toolEffReinforce;
    public float skillEffReinforce;
    public float expEffReinforce;

    // tools
    public float effecienty;
    public float speed;
    public float luck;
    public float bonus;
    public float ability;

    public float effecientyReinforce;
    public float speedReinforce;
    public float luckReinforce;
    public float bonusReinforce;
    public float abilityReinforce;

    public HandType handType;

    public string spriteAnimatorPath;
    public Animator spriteAnimator;

    public Item(int count, int code, string itemName, ItemType type, EquipmentType equipmentType, string spritePath, string rating,
        float weight, int countLimit, int price, string itemInfo, ItemEffect itemEffect = null, float size = 0,
        int levelLimit = 0, int reinforce = 0, int power = 0, int armor = 0,
        int accuracy = 0, int avoid = 0, float critRate = 0, float critDam = 0,
        float satietyPoint = 0, float moisturePoint = 0, float catharsisPoint = 0,
        float fatiguePoint = 0, float toolEff = 0, float skillEff = 0, float expEff = 0,
        int powerReinforce = 0, int armorReinforce = 0, int accuracyReinforce = 0, int avoidReinforce = 0, float critRateReinforce = 0,
        float critDamReinforce = 0, float satietyPointReinforce = 0, float moisturePointReinforce = 0, float catharsisPointReinforce = 0,
        float fatiguePointReinforce = 0, float toolEffReinforce = 0, float skillEffReinforce = 0, float expEffReinforce = 0,
        float effecienty = 0, float speed = 0, float luck = 0, float bonus = 0, float ability = 0,
        float effecientyReinforce = 0, float speedReinforce = 0, float luckReinforce = 0, float bonusReinforce = 0, float abilityReinforce = 0,
        HandType handType = HandType.None, string spriteAnimatorPath = null)
    {
        this.count = count;
        this.code = code;
        this.itemName = itemName;
        this.type = type;
        this.equipmentType = equipmentType;
        this.spritePath = spritePath;
        this.rating = rating;
        this.weight = weight;
        this.countLimit = countLimit;
        this.price = price;
        this.itemInfo = itemInfo;
        this.itemEffect = itemEffect;

        this.size = size;
        this.spriteAnimatorPath = spriteAnimatorPath;

        if (type == ItemType.Equipment)
        {
            this.levelLimit = levelLimit;
            this.reinforce = reinforce;
            this.power = power;
            this.armor = armor;
            this.accuracy = accuracy;
            this.avoid = avoid;
            this.critRate = critRate;
            this.critDam = critDam;
            this.satietyPoint = satietyPoint;
            this.moisturePoint = moisturePoint;
            this.catharsisPoint = catharsisPoint;
            this.fatiguePoint = fatiguePoint;
            this.toolEff = toolEff;
            this.skillEff = skillEff;
            this.expEff = expEff;

            this.powerReinforce = powerReinforce;
            this.armorReinforce = armorReinforce;
            this.accuracyReinforce = accuracyReinforce;
            this.avoidReinforce = avoidReinforce;
            this.critRateReinforce = critRateReinforce;
            this.critDamReinforce = critDamReinforce;
            this.satietyPointReinforce = satietyPointReinforce;
            this.moisturePointReinforce = moisturePointReinforce;
            this.catharsisPointReinforce = catharsisPointReinforce;
            this.fatiguePointReinforce = fatiguePointReinforce;
            this.toolEffReinforce = toolEffReinforce;
            this.skillEffReinforce = skillEffReinforce;
            this.expEffReinforce = expEffReinforce;

            this.effecienty = effecienty;
            this.speed = speed;
            this.luck = luck;
            this.bonus = bonus;
            this.ability = ability;

            this.effecientyReinforce = effecientyReinforce;
            this.speedReinforce = speedReinforce;
            this.luckReinforce = luckReinforce;
            this.bonusReinforce = bonusReinforce;
            this.abilityReinforce = abilityReinforce;

            this.handType = handType;
        }

        sprite = loadSprite(spritePath);

        if (spriteAnimatorPath != null)
        {
            spriteAnimator = loadAnimator(spriteAnimatorPath);
        }
    }

    public bool equip(Item item)
    {
        bool isEquip = false;
        if (item.type == ItemType.Equipment)
        {
            isEquip = true;
        }

        return isEquip;
    }

    [ContextMenu("From Json Data")]
    public Sprite loadSprite(string path)
    {
        return Resources.Load<Sprite>(path);
    }

    [ContextMenu("From Json Data")]
    public Animator loadAnimator(string path)
    {
        return Resources.Load<Animator>(path);
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

    public List<int> findNotNullEquipmentOptions()
    {
        List<int> datas = new List<int>();

        if (power != 0)
        {
            datas.Add(1);
        }
        if (armor != 0)
        {
            datas.Add(2);
        }
        if (accuracy != 0)
        {
            datas.Add(3);
        }
        if (avoid != 0)
        {
            datas.Add(4);
        }
        if (critRate != 0)
        {
            datas.Add(5);
        }
        if (critDam != 0)
        {
            datas.Add(6);
        }

        if (satietyPoint != 0)
        {
            datas.Add(7);
        }
        if (moisturePoint != 0)
        {
            datas.Add(8);
        }
        if (catharsisPoint != 0)
        {
            datas.Add(9);
        }
        if (fatiguePoint != 0)
        {
            datas.Add(10);
        }

        if (toolEff != 0)
        {
            datas.Add(11);
        }
        if (skillEff != 0)
        {
            datas.Add(12);
        }
        if (expEff != 0)
        {
            datas.Add(13);
        }

        if (powerReinforce != 0)
        {
            datas.Add(14);
        }
        if (armorReinforce != 0)
        {
            datas.Add(15);
        }
        if (accuracyReinforce != 0)
        {
            datas.Add(16);
        }
        if (avoidReinforce != 0)
        {
            datas.Add(17);
        }
        if (critRateReinforce != 0)
        {
            datas.Add(18);
        }
        if (critDamReinforce != 0)
        {
            datas.Add(19);
        }

        if (satietyPointReinforce != 0)
        {
            datas.Add(20);
        }
        if (moisturePointReinforce != 0)
        {
            datas.Add(21);
        }
        if (catharsisPointReinforce != 0)
        {
            datas.Add(22);
        }
        if (fatiguePointReinforce != 0)
        {
            datas.Add(23);
        }

        if (toolEffReinforce != 0)
        {
            datas.Add(24);
        }
        if (skillEffReinforce != 0)
        {
            datas.Add(25);
        }
        if (expEffReinforce != 0)
        {
            datas.Add(26);
        }

        if (effecienty != 0)
        {
            datas.Add(27);
        }
        if (speed != 0)
        {
            datas.Add(28);
        }
        if (luck != 0)
        {
            datas.Add(29);
        }
        if (bonus != 0)
        {
            datas.Add(30);
        }
        if (ability != 0)
        {
            datas.Add(31);
        }

        return null;
    }
}

public enum ItemType
{
    Equipment,
    Consumable,
    Etc,
    Fish
}

public enum EquipmentType
{
    None,   //0
    LeftHand,   //1
    RightHand,  //2
    TwoHands,   //3
    Head,   //4
    Top,    //5
    Pants,  //6
    Body,   //7
    Gloves, //8
    Shoes,  //9
    Neckless,   //10
    Earing, //11
    Ring,   //12
    Hair    //13
}

public enum HandType
{
    None,
    Sword,
    Spear,
    Torch,
}
