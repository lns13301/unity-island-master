using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishAI : MonoBehaviour
{
    public RectTransform fishPanel;
    public GameObject fish;
    public float originPosX;
    public float originPosY;

    public float movedHorizontal;
    public float movedVertical;

    public float nextMoveX;
    public float nextMoveY;
    public float idleMoveY;
    public float fishSpeed;

    public float borderX;
    public float borderY;

    private float thinkTimer;
    private float idleTimer;

    public GameObject hudScoreText;
    public Transform hudPos;
    public string scoreType;
    public int comboStack;
    public float comboTimer;

    public bool isBobberIn;

    // Start is called before the first frame update
    void Start()
    {
        fishSpeed = 1.5f;
        idleMoveY = 1f;
        borderX = 400;
        borderY = 250;
        comboStack = 0;
        movedHorizontal = originPosX;
        movedVertical = originPosY;
    }

    // Update is called once per frame
    void Update()
    {
        fishMovement();
        basicMovement();
        borderCheck();

        detectRange();

        if (isBobberIn)
        {
            comboCheck();
        }
    }

    public void fishMovement()
    {
        //Move
        fish.transform.position = new Vector2(fish.transform.position.x + nextMoveX, fish.transform.position.y + nextMoveY + idleMoveY);
        movedHorizontal += nextMoveX;
        movedVertical += nextMoveY;

        thinkTimer += Time.deltaTime;

        if (thinkTimer > 2.5f)
        {
            thinkTimer = 0;
            think();
        }
    }

    private void basicMovement()
    {
        idleTimer += Time.deltaTime;

        if (idleTimer > 0.5)
        {
            idleTimer = 0;
            idleMoveY *= -1;
        }
    }

    private void think()
    {
        nextMoveX = Random.Range(-2, 3) * fishSpeed;
        nextMoveY = Random.Range(-2, 3) * 0.3f * fishSpeed;
        doFlip();
    }

    private void doFlip()
    {
        if (nextMoveX < 0)
        {
            fishPanel.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            fishPanel.localScale = new Vector3(1, 1, 1);
        }
    }

    private void borderCheck()
    {
        if (movedHorizontal > borderX)
        {
            nextMoveX = Random.Range(-2, 1) * 0.3f * fishSpeed;
            thinkTimer = 0;
        }
        if (movedHorizontal < -borderX)
        {
            nextMoveX = Random.Range(0, 3) * 0.3f * fishSpeed;
            thinkTimer = 0;
        }
        if (movedVertical > borderY)
        {
            nextMoveY = Random.Range(-2, 0) * 0.3f * fishSpeed;
            thinkTimer = 0;
        }
        if (movedVertical < -borderY)
        {
            nextMoveY = Random.Range(0, 3) * 0.3f * fishSpeed;
            thinkTimer = 0;
        }
    }

    public void comboCheck()
    {
        comboTimer += Time.deltaTime;

        if (comboStack == 0 && comboTimer > 1)
        {
            scoreType = "GOOD";
            setScoreState();
            comboStack++;
        }
        if (comboStack == 1 && comboTimer > 2)
        {
            scoreType = "GREAT";
            setScoreState();
            comboStack++;
        }
        if (comboStack == 2 && comboTimer > 3)
        {
            scoreType = "EXCELLENT";
            setScoreState();
            comboStack++;
        }
        if (comboStack == 3 && comboTimer > 4)
        {
            scoreType = "AMAZING";
            setScoreState();
            comboStack++;
        }
        if (comboStack == 4 && comboTimer > 5)
        {
            scoreType = "PERFECT";
            setScoreState();
            comboStack++;
        }

        if (comboStack == 5 && comboTimer > 6)
        {
            scoreType = "CAPTURE!!";
            setScoreState();
            FishingUI.instance.isCapture = true;

            FishRod.instance.isCapturing = false;
            isBobberIn = false;

            comboStack = -1;
            comboTimer = -1;
        }
    }

    public void setScoreState()
    {
        GameObject hudText = Instantiate(hudScoreText);
        hudText.transform.position = hudPos.position;

        hudText.GetComponent<FishingScore>().scoreType = scoreType;
        hudText.transform.SetParent(transform.parent);
    }

    void detectRange()
    {
        if (FishRod.instance.isCapturing && isBobberIn 
            && !((Mathf.Abs(FishRod.instance.originPosX - movedHorizontal) < 133) && (Mathf.Abs(FishRod.instance.originPosY - movedVertical)) < 100))
        {
            FishRod.instance.isCapturing = false;
            isBobberIn = false;

            scoreType = "MISS";
            setScoreState();

            comboStack = 0;
            comboTimer = 0;
        }
        if ((Mathf.Abs(FishRod.instance.originPosX - movedHorizontal) < 100) && (Mathf.Abs(FishRod.instance.originPosY - movedVertical) < 75) && !FishRod.instance.isCapturing)
        {
            isBobberIn = true;
            FishRod.instance.isCapturing = true;
        }
    }
}
