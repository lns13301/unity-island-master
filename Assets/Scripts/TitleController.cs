using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleController : MonoBehaviour
{
    private float alphaSpeed = 1f;

    public GameObject textTitleSet;

    public Text title;
    public Text subTitle;

    public float time;

    public bool isFadeIn;
    public bool isFadeOut;

    public bool isCoolOn;

    // Start is called before the first frame update
    void Start()
    {
        isCoolOn = false;
        textTitleSet.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (isCoolOn && time < 3)
        {
            fadeOutCool();
        } 
        else
        {
            isCoolOn = false;
            fadeOut();
        }

        if (isFadeIn)
        {
            title.color = new Color(title.color.r, title.color.g, title.color.b, title.color.a + Time.deltaTime * alphaSpeed);
            subTitle.color = new Color(subTitle.color.r, subTitle.color.g, subTitle.color.b, subTitle.color.a + Time.deltaTime * alphaSpeed);

            if (title.color.a + Time.deltaTime * alphaSpeed >= 1)
            {
                title.color = new Color(title.color.r, title.color.g, title.color.b, 1);
                subTitle.color = new Color(subTitle.color.r, subTitle.color.g, subTitle.color.b, 1);
                isFadeIn = false;
            }
        }

        if (isFadeOut)
        {
            title.color = new Color(title.color.r, title.color.g, title.color.b, title.color.a - Time.deltaTime * alphaSpeed);
            subTitle.color = new Color(subTitle.color.r, subTitle.color.g, subTitle.color.b, subTitle.color.a - Time.deltaTime * alphaSpeed);

            if (title.color.a - Time.deltaTime * alphaSpeed <= 0)
            {
                title.color = new Color(title.color.r, title.color.g, title.color.b, 0);
                subTitle.color = new Color(subTitle.color.r, subTitle.color.g, subTitle.color.b, 0);
                isFadeOut = false;
                textTitleSet.SetActive(false);
            }
        }
    }

    public void fadeIn()
    {
        isFadeOut = false;
        isFadeIn = true;

        time = 0;
        isCoolOn = true;
    }

    public void fadeOut()
    {
        isFadeIn = false;
        isFadeOut = true;
    }

    public void fadeOutCool()
    {
        time += Time.deltaTime;
    }
}
