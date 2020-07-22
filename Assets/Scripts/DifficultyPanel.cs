using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DifficultyPanel : MonoBehaviour
{
    public string difficulty;
    public Text difficultyText;

    public MapUI mapUI;

    public int mapCode;
    public int mapDifficulty;

    // Start is called before the first frame update
    void Start()
    {
        difficulty = difficultyText.text;
        mapUI = GameObject.Find("Canvas").GetComponent<MapUI>();

        mapDifficulty = 0;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void buttonLeft()
    {
        if (difficulty.Equals("쉬움"))
        {
            return;
        }

        switch (difficulty)
        {
            case "어려움":
                difficulty = "보통";
                difficultyText.color = new Color(0.2f, 0.7f, 0, 1);

                mapDifficulty = 1;
                break;
            case "보통":
                difficulty = "쉬움";
                difficultyText.color = new Color(0, 0.6f, 0.7f, 1);

                mapDifficulty = 0;
                break;
            default:
                break;
        }

        difficultyText.text = difficulty;
    }

    public void buttonRight()
    {
        if (difficulty.Equals("어려움"))
        {
            return;
        }

        switch (difficulty)
        {
            case "쉬움":
                difficulty = "보통";
                difficultyText.color = new Color(0.2f, 0.7f, 0, 1);

                mapDifficulty = 1;
                break;
            case "보통":
                difficulty = "어려움";
                difficultyText.color = new Color(0.9f, 0.2f, 0.05f, 1);

                mapDifficulty = 2;
                break;
            default:
                break;
        }

        difficultyText.text = difficulty;
    }

    public void enterDungeon()
    {
        mapUI.mapCode = mapCode;
        mapUI.mapDifficulty = mapDifficulty;

        mapUI.enterDungeon();
    }
}
