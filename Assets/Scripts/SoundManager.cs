using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SoundManager : MonoBehaviour
{
    public BGM[] bgm;
    public int mapCode;
    public bool isMapChanged;
    public static SoundManager instance;

    private void Start()
    {
        instance = this;
        mapCode = GameObject.Find("Canvas").GetComponent<MapUI>().mapCode;

        try
        {
            beachWave();
        }
        catch (NullReferenceException)
        {
            Debug.Log("오디오가 비어있음");
        }

        isMapChanged = true;
    }
    private void Update()
    {
        if (!isMapChanged)
        {
            return;
        }

        switch (mapCode)
        {
            case 0:
                beachSounds();
                break;
            case 2:
                forestSounds();
                break;
            default:
                break;
        }

        isMapChanged = false;
    }

    public void beachWave()
    {
        bgm[0].playBGM();
        Debug.Log("파도소리 다시재생");
    }

    public void seagull()
    {
        bgm[1].playBGM();
    }

    public void dolphin()
    {
        bgm[2].playBGM();
    }

    public void beachSounds()
    {
        InvokeRepeating("beach", 0.1f, 154);
        InvokeRepeating("beachWave", 15, 15);
        InvokeRepeating("seagull", 25, 25);
        InvokeRepeating("dolphin", 45, 45);
    }

    public void bubblePop()
    {
        bgm[3].playBGM();
    }

    public void birds()
    {
        bgm[4].playBGM();
    }

    public void forestSounds()
    {
        InvokeRepeating("forest", 0.1f, 92);
        InvokeRepeating("birds", 4f, 20);
    }

    public void forest()
    {
        bgm[5].playBGM();
    }

    public void beach()
    {
        bgm[6].playBGM();
    }

    public void door()
    {
        bgm[7].playBGM();
    }

    public void refreshSounds()
    {
        CancelInvoke();
        stopAllSounds();
        isMapChanged = true;
    }

    public void stopAllSounds()
    {
        for (int i = 0; i < bgm.Length; i++)
        {
            bgm[i].stopBGM();
        }
    }
}
