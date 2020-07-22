using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatUI : MonoBehaviour
{
    public string NEW_LINE = "\n";
    public GameObject statSet;

    public Image[] statPointUp;

    public Text levelText;
    public Text expText;

    public Text statPointText;
    public Text strengthText;
    public Text enduranceText;
    public Text intellectText;
    public Text concentrationText;

    public Text powerTextText;
    public Text armorTextText;
    public Text accuracyText;
    public Text avoidText;
    public Text critRateText;
    public Text critDamText;

    public Text satietyText;
    public Text moistureText;
    public Text catharsisText;
    public Text fatigueText;

    public Text toolEffText;
    public Text skillEffText;
    public Text expEffText;

    public Text fameTextText;
    public Text charmTextText;

    public Animator buttonMenuAnimator;

    // 추후 delegate로 변경해야함
    public bool isDataChanged;

    private DialogManager dialogManager;

    public bool isUIOn;

    // Start is called before the first frame update
    void Start()
    {
        dialogManager = GameObject.Find("DialogManager").GetComponent<DialogManager>();

        statSet.SetActive(false);
        isDataChanged = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            uiOnOff();
        }
    }

    private void FixedUpdate()
    {
        if (isDataChanged)
        {
            refresh();
        }
    }

    public void uiOnOff()
    {
        if (statSet.activeSelf)
        {
            statSet.SetActive(false);
        }
        else
        {
            if (GetComponent<InventoryUI>().inventorySet.activeSelf)
            {
                GetComponent<InventoryUI>().inventorySet.SetActive(false);
            }
            if (GetComponent<EquipmentUI>().equipmentSet.activeSelf)
            {
                GetComponent<EquipmentUI>().equipmentSet.SetActive(false);
            }

            statSet.SetActive(true);
            buttonMenuAnimator.SetBool("isUIOn", true);
        }
    }

    public void setPlayerEquipedTool()
    {
        // 빈 아이템일 경우 Null로 처리
        try
        {
            if (dialogManager.playerEquipment.items[2] != null && dialogManager.playerEquipment.items[2].itemName.Length > 0)
            {
                GameObject.Find("Player").GetComponent<Player>().isEquipedTool = true;
            }
            else
            {
                GameObject.Find("Player").GetComponent<Player>().isEquipedTool = false;
            }

            int dummy = dialogManager.playerEquipment.items[2].itemName.Length;
        }
        catch
        {
            PlayerEquipment.instance.removeItem(EquipmentType.RightHand);
            dialogManager.playerEquipment.items[2] = null;
        }
    }

/*    private void updateQuickSlot()
    {
        for (int i = 0; i < GetComponent<ItemMenuSet>().quickSlot.Length; i++)
        {
            if (GetComponent<ItemMenuSet>().quickSlot[i].item != null && GetComponent<ItemMenuSet>().quickSlot[i].item.count > 1)
            {
                GetComponent<ItemMenuSet>().quickSlot[i].isDataChanged = true;
            }
        }
    }*/

    public void refresh()
    {
        isDataChanged = false;
        dialogManager.playerEquipment.updateTotalStats();
        dialogManager.playerData = Calculator.calcAll(dialogManager.playerData);

        setPlayerEquipedTool();

        levelText.text = "레벨 : " + dialogManager.playerData.level;
        statPointText.text = "남은 스탯포인트 : " + dialogManager.playerData.statPoint;
        expText.text = dialogManager.playerData.exp + " / " + dialogManager.playerData.nextExp;
        strengthText.text = "" + dialogManager.playerData.strengthPoint;
        enduranceText.text = "" + dialogManager.playerData.endurancePoint;
        intellectText.text = "" + dialogManager.playerData.intellectPoint;
        concentrationText.text = "" + dialogManager.playerData.concentrationPoint;

        if (dialogManager.playerData.powerEquipment > 0)
        {
            powerTextText.text = "공격력 : " + dialogManager.playerData.power + NEW_LINE + " ( + " + dialogManager.playerData.powerEquipment + " )";
        }
        else if (dialogManager.playerData.powerEquipment < 0)
        {
            powerTextText.text = "공격력 : " + dialogManager.playerData.power + NEW_LINE + " ( " + dialogManager.playerData.powerEquipment + " )";
        }
        else
        {
            powerTextText.text = "공격력 : " + dialogManager.playerData.power;
        }

        if (dialogManager.playerData.armorEquipment > 0)
        {
            armorTextText.text = "방어력 : " + dialogManager.playerData.armor + NEW_LINE + " ( + " + dialogManager.playerData.armorEquipment + " )";
        }
        else if (dialogManager.playerData.armorEquipment < 0)
        {
            armorTextText.text = "방어력 : " + dialogManager.playerData.armor + NEW_LINE + " ( " + dialogManager.playerData.armorEquipment + " )";
        }
        else
        {
            armorTextText.text = "방어력 : " + dialogManager.playerData.armor;
        }

        if (dialogManager.playerData.accuracyEquipment > 0)
        {
            accuracyText.text = "명중률 : " + dialogManager.playerData.accuracy + NEW_LINE + " ( + " + dialogManager.playerData.accuracyEquipment + " )";
        }
        else if (dialogManager.playerData.accuracyEquipment < 0)
        {
            accuracyText.text = "명중률 : " + dialogManager.playerData.accuracy + NEW_LINE + " ( " + dialogManager.playerData.accuracyEquipment + " )";
        }
        else
        {
            accuracyText.text = "명중률 : " + dialogManager.playerData.accuracy;
        }

        if (dialogManager.playerData.avoidEquipment > 0)
        {
            avoidText.text = "회피율 : " + dialogManager.playerData.avoid + NEW_LINE + " ( + " + dialogManager.playerData.avoidEquipment + " )";
        }
        else if (dialogManager.playerData.avoidEquipment < 0)
        {
            avoidText.text = "회피율 : " + dialogManager.playerData.avoid + NEW_LINE + " ( " + dialogManager.playerData.avoidEquipment + " )";
        }
        else
        {
            avoidText.text = "회피율 : " + dialogManager.playerData.avoid;
        }

        if (dialogManager.playerData.critRateEquipment > 0)
        {
            critRateText.text = "치명 확률 : " + Mathf.Round(dialogManager.playerData.critRate * 10) / 10 + "%"
                + NEW_LINE + " ( + " + Mathf.Round(dialogManager.playerData.critRateEquipment * 10) / 10 + "% )";
        }
        else if (dialogManager.playerData.critRateEquipment < 0)
        {
            critRateText.text = "치명 확률 : " + Mathf.Round(dialogManager.playerData.critRate * 10) / 10 + "%"
                + NEW_LINE + " ( " + Mathf.Round(dialogManager.playerData.critRateEquipment * 10) / 10 + "% )";
        }
        else
        {
            critRateText.text = "치명 확률 : " + Mathf.Round(dialogManager.playerData.critRate * 10) / 10 + "%";
        }

        if (dialogManager.playerData.critDamEquipment > 0)
        {
            critDamText.text = "치명 피해 : " + Mathf.Round(dialogManager.playerData.critDam * 10) / 10 + "%"
                + NEW_LINE + " ( + " + Mathf.Round(dialogManager.playerData.critDamEquipment * 10) / 10 + "% )";
        }
        else if (dialogManager.playerData.critDamEquipment < 0)
        {
            critDamText.text = "치명 피해 : " + Mathf.Round(dialogManager.playerData.critDam * 10) / 10 + "%"
                + NEW_LINE + " ( " + Mathf.Round(dialogManager.playerData.critDamEquipment * 10) / 10 + "% )";
        }
        else
        {
            critDamText.text = "치명 피해 : " + Mathf.Round(dialogManager.playerData.critDam * 10) / 10 + "%";
        }

        satietyText.text = "포만감" + NEW_LINE + (int)dialogManager.playerData.satiety + " / " + dialogManager.playerData.satietyMax;
        moistureText.text = "수분" + NEW_LINE + (int)dialogManager.playerData.moisture + " / " + dialogManager.playerData.moistureMax;
        catharsisText.text = "배변" + NEW_LINE + (int)dialogManager.playerData.catharsis + " / " + dialogManager.playerData.catharsisMax;
        fatigueText.text = "피로도" + NEW_LINE + (int)dialogManager.playerData.fatigue + " / " + dialogManager.playerData.fatigueMax;

        toolEffText.text = "도구 효율" + NEW_LINE + Mathf.Round(dialogManager.playerData.toolEff * 10) / 10 + "%";
        skillEffText.text = "기술 효율" + NEW_LINE + Mathf.Round(dialogManager.playerData.skillEff * 10) / 10 + "%";
        expEffText.text = "경험치 보너스" + NEW_LINE + Mathf.Round(dialogManager.playerData.expEff * 10) / 10 + "%";

        fameTextText.text = "명성 : " + dialogManager.playerData.fame;
        charmTextText.text = "호감도 : " + dialogManager.playerData.charm;

        for (int i = 0; i < statPointUp.Length; i++)
        {
            if (dialogManager.playerData.statPoint > 0)
            {
                statPointUp[i].color = new Color(1, 1, 1, 1);
            }
            else
            {
                statPointUp[i].color = new Color(1, 1, 1, 0);
            }
        }
    }

    private bool pointUp()
    {
        if (dialogManager.playerData.statPoint < 1)
        {
            return false;
        }

        dialogManager.playerData.statPoint--;
        isDataChanged = true;
        GameObject.Find("Player").GetComponent<Player>().isPointUp = true;

        return true;
    }

    public void pointUpStrength()
    {
        if (pointUp())
        {
            dialogManager.playerData.strengthPoint++;
        }
    }

    public void pointUpEndurance()
    {
        if (pointUp())
        {
            dialogManager.playerData.endurancePoint++;
        }
    }

    public void pointUpIntellect()
    {
        if (pointUp())
        {
            dialogManager.playerData.intellectPoint++;
        }
    }
    public void pointUpConcentration()
    {
        if (pointUp())
        {
            dialogManager.playerData.concentrationPoint++;
        }
    }
}
