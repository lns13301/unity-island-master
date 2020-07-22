using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Calculator
{
    public static PlayerData calcAll(PlayerData playerData)
    {
        PlayerData data = playerData;
        data.power = powerCalc(data);
        data.armor = armorCalc(data);
        data.accuracy = accuracyCalc(data);
        data.avoid = avoidCalc(data);
        data.critRate = critRateCalc(data);
        data.critDam = critDamCalc(data);
        data.satietyMax = satietyMaxCalc(data);
        data.moistureMax = moistureMaxCalc(data);
        data.catharsisMax = catharsisMaxCalc(data);
        data.fatigueMax = fatigueMaxCalc(data);

        return data;
    }

    public static int powerCalc(PlayerData data)
    {
        float[] datas = new float[5];

        datas[0] = data.strengthPoint;
        datas[1] = data.level;
        datas[2] = data.intellectPoint;
        datas[3] = data.concentrationPoint;
        datas[4] = data.powerEquipment;
        return (int)(datas[0] * 3 + datas[1] * 2 + datas[2] / 2 + datas[3] / 5 + datas[4]);
    }

    public static int armorCalc(PlayerData data)
    {
        float[] datas = new float[4];

        datas[0] = data.endurancePoint;
        datas[1] = data.level;
        datas[2] = data.concentrationPoint;
        datas[3] = data.armorEquipment;
        return (int)(datas[0] * 2 + datas[1] * 1 + datas[2] / 2 + datas[3]);
    }

    public static int accuracyCalc(PlayerData data)
    {
        float[] datas = new float[4];

        datas[0] = data.concentrationPoint;
        datas[1] = data.level;
        datas[2] = data.intellectPoint;
        datas[3] = data.accuracyEquipment;
        return (int)(datas[0] * 2 + datas[1] * 0.5 + datas[2] / 4 + datas[3]);
    }

    public static int avoidCalc(PlayerData data)
    {
        float[] datas = new float[4];

        datas[0] = data.endurancePoint;
        datas[1] = data.concentrationPoint;
        datas[2] = data.level;
        datas[3] = data.avoidEquipment;
        return (int)(datas[0] / 3 + datas[1] * 2 + datas[2] / 5 + datas[3]);
    }

    public static int critRateCalc(PlayerData data)
    {
        float[] datas = new float[4];

        datas[0] = data.concentrationPoint;
        datas[1] = data.level;
        datas[2] = data.accuracy;
        datas[3] = data.critRateEquipment;
        return (int)(datas[0] / 10 + datas[1] / 20 + datas[2] / 20 + datas[3]);
    }

    public static int critDamCalc(PlayerData data)
    {
        float[] datas = new float[4];

        datas[0] = data.strengthPoint;
        datas[1] = data.level;
        datas[2] = data.intellectPoint;
        datas[3] = data.critDamEquipment;
        return (int)(datas[0] / 3 + datas[1] / 5 + datas[2] / 4 + datas[3]);
    }

    public static int satietyCalc(PlayerData data)
    {
        float[] datas = new float[3];

        datas[0] = data.strengthPoint;
        datas[1] = data.weight;
        datas[2] = data.level;

        int satietyValue = (int)((datas[0] + datas[1] / 5) / 15 + datas[2] / 5 + 1);
        catharsisCalc(satietyValue);

        return satietyValue;
    }

    public static int moistureCalc(PlayerData data)
    {
        float[] datas = new float[3];

        datas[0] = data.intellectPoint;
        datas[1] = data.concentrationPoint;
        datas[2] = data.level;

        int moistureValue = (int)((datas[0] + datas[1] / 3) / 5 + datas[2] / 4);
        catharsisCalc(moistureValue);

        return moistureValue;
    }

    public static void catharsisCalc(int value)
    {
        GameObject.Find("DialogManager").GetComponent<DialogManager>().playerData.catharsis -= value / 3;
    }

    public static void fatigueCalc(int value)
    {
        GameObject.Find("DialogManager").GetComponent<DialogManager>().playerData.fatigue -= value / 10;
    }

    public static float fatigueCalc(PlayerData data, int power, bool isHit)
    {
        if (!isHit)
        {
            return 0;
        }
        float[] datas = new float[8];

        datas[0] = data.armor;
        datas[1] = data.avoid;
        datas[2] = data.satiety;
        datas[3] = data.moisture;
        datas[4] = data.catharsis;
        datas[5] = data.satietyMax;
        datas[6] = data.moistureMax;
        datas[7] = data.catharsisMax;

        float damage = power - datas[0];

        if (damage <= 0)
        {
            return 0;
        }

        float data1 = (1 - (datas[2] / datas[5])) * 5;
        float data2 = (1 - (datas[3] / datas[6])) * 10;
        float data3 = (1 - (datas[4] / datas[7])) * 2;
        float data4 = 1;

        data4 += data1 + data2 + data3;

        return damage * data4;
    }

    public static int satietyMaxCalc(PlayerData data)
    {
        float[] datas = new float[4];

        datas[0] = data.endurancePoint;
        datas[1] = data.concentrationPoint;
        datas[2] = data.level;
        datas[3] = data.satietyPointEquipment;

        return 100 + (int)(datas[0] * 3 + datas[1] + datas[2] + datas[3] - 1);
    }

    public static int moistureMaxCalc(PlayerData data)
    {
        float[] datas = new float[4];

        datas[0] = data.endurancePoint;
        datas[1] = data.concentrationPoint;
        datas[2] = data.level;
        datas[3] = data.moisturePointEquipment;

        return 100 + (int)(datas[0] * 3 + datas[1] + datas[2] + datas[3] - 1);
    }

    public static int catharsisMaxCalc(PlayerData data)
    {
        float[] datas = new float[4];

        datas[0] = data.endurancePoint;
        datas[1] = data.intellectPoint;
        datas[2] = data.level;
        datas[3] = data.catharsisPointEquipment;

        return 100 + (int)(datas[0] * 3 + datas[1] + datas[2] + datas[3] - 1);
    }
    public static int fatigueMaxCalc(PlayerData data)
    {
        float[] datas = new float[4];

        datas[0] = data.endurancePoint;
        datas[1] = data.strengthPoint;
        datas[2] = data.level;
        datas[3] = data.fatiguePointEquipment;

        return 100 + (int)(datas[0] * 5 + datas[1] * 2 + datas[2] + datas[3] - 1);
    }

    public static string numberToFormatting(int num)
    {
        string result = "";

        while (num > 0)
        {
            result = (num % 1000) + result;
            num /= 1000;

            if (num > 0)
            {
                result = "," + result;
            }
        }

        return result;
    }

    public static string numberToFormatting(double num)
    {
        string result = "";

        while (num > 0)
        {
            result = (num % 1000) + result;
            num /= 1000;

            if (num > 0)
            {
                result = "," + result;
            }
        }

        return result;
    }

    public static int fomattingToInteger(string num)
    {
        string[] numbers = num.Split(',');
        string number = "";

        for (int i = 0; i < numbers.Length; i++)
        {
            number += numbers[i];
        }

        return int.Parse(number);
    }

    public static double fomattingToDouble(string num)
    {
        string[] numbers = num.Split(',');
        string number = "";

        for (int i = 0; i < numbers.Length; i++)
        {
            number += numbers[i];
        }

        return double.Parse(number);
    }
}