using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemEffect
{
    public int saturationPoint = 0;
    public int moisturePoint = 0;
    public int catharsisPoint = 0;
    public int fatiguePoint = 0;

    public ItemEffect(int saturationPoint, int moisturePoint, int catharsisPoint, int fatiguePoint)
    {
        this.saturationPoint = saturationPoint;
        this.moisturePoint = moisturePoint;
        this.catharsisPoint = catharsisPoint;
        this.fatiguePoint = fatiguePoint;
    }

    public void useItem()
    {
        GameObject.Find("Player").GetComponent<Player>().controlEating();
        GameObject.Find("DialogManager").GetComponent<DialogManager>().playerData.satiety += saturationPoint;
        GameObject.Find("DialogManager").GetComponent<DialogManager>().playerData.moisture += moisturePoint;
        GameObject.Find("DialogManager").GetComponent<DialogManager>().playerData.catharsis += catharsisPoint;
        GameObject.Find("DialogManager").GetComponent<DialogManager>().playerData.fatigue += fatiguePoint;
    }
}
