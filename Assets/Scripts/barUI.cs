using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class barUI : MonoBehaviour
{
    public Image[] stateBarImages = new Image[4];
    private DialogManager dialogManager;

    // Start is called before the first frame update
    void Start()
    {
        dialogManager = GameObject.Find("DialogManager").GetComponent<DialogManager>();
        foreach (Image stateBarImage in stateBarImages)
        {
            stateBarImage.fillAmount = 1f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        stateBarImages[0].fillAmount = dialogManager.playerData.satiety / dialogManager.playerData.satietyMax;
        stateBarImages[1].fillAmount = dialogManager.playerData.moisture / dialogManager.playerData.moistureMax;
        stateBarImages[2].fillAmount = dialogManager.playerData.catharsis / dialogManager.playerData.catharsisMax;
        stateBarImages[3].fillAmount = dialogManager.playerData.fatigue / dialogManager.playerData.fatigueMax;
    }
}
