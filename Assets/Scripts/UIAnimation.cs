using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAnimation : MonoBehaviour
{
    private float ANIMATION_SPEED = 0.05f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool openUI(GameObject gameObject)
    {
        float value = gameObject.transform.localScale.x;

        if (value == 1)
        {
            value = 0.01f;
        }

        gameObject.transform.localScale = new Vector3(value + ANIMATION_SPEED, value + ANIMATION_SPEED, 1);

        if (gameObject.transform.localScale.x >= 0.95f)
        {
            gameObject.transform.localScale = new Vector3(1, 1, 1);

            return true;
        }

        return false;
    }

    public bool closeUI(GameObject gameObject)
    {
        float value = gameObject.transform.localScale.x;

        gameObject.transform.localScale = new Vector3(value - ANIMATION_SPEED, value - ANIMATION_SPEED, 1);

        if (gameObject.transform.localScale.x <= 0.05f)
        {
            gameObject.transform.localScale = new Vector3(0, 0, 1);

            return true;
        }

        return false;
    }
}
