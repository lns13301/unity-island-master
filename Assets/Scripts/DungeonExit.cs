using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonExit : MonoBehaviour
{
    public GameObject dungeonSet;
    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = dungeonSet.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void uiOnOff()
    {
        if (animator.GetBool("isUIOn"))
        {
            animator.SetBool("isUIOn", false);
        }
        else
        {
            animator.SetBool("isUIOn", true);
        }
    }

    public void uiOn()
    {
        animator.SetBool("isUIOn", true);
    }

    public void uiOff()
    {
        animator.SetBool("isUIOn", false);
    }
}

public enum ExitType
{
    Able,
    Disable,
    Random,
    NeedItem,
    NeedQuest
}