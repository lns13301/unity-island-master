using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public Animator animator;
    public DoorRange doorRange;
    public GameObject portal;
    public GameObject doorCollider;

    public bool isOpen;

    public int doorCooldown;
    
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        doorRange = transform.GetChild(0).gameObject.GetComponent<DoorRange>();
        doorCollider = transform.GetChild(1).gameObject;
        closeDoorWhenStart();
        isOpen = false;
    }

    // Update is called once per frame
    void Update()
    {
        if ((doorRange.isPlayerIn) && doorRange.playerScript.pickUpAnimationProgress() && doorCooldown == 0)
        {
            if (portal.gameObject.activeSelf)
            {
                closeDoor();
                doorCooldown = 80;
            }
            else
            {
                openDoor();
                doorCooldown = 80;
            }
        }
        
        if (doorCooldown == 10 && isOpen)
        {
            portal.gameObject.SetActive(true);
            isOpen = false;
        }

        if (doorCooldown > 0)
        {
            doorCooldown--;
        }
    }

    public void openDoor()
    {
        Debug.Log("열기!");
        animator.SetTrigger("doOpen");
        //portal.gameObject.SetActive(true);
        doorCollider.gameObject.SetActive(true);
        isOpen = true;
        SoundManager.instance.door();
    }

    public void closeDoor()
    {
        Debug.Log("닫기!");
        animator.SetTrigger("doClose");
        portal.gameObject.SetActive(false);
        doorCollider.gameObject.SetActive(false);
        SoundManager.instance.door();
    }

    public void closeDoorWhenStart()
    {
        Debug.Log("닫기!");
        animator.SetTrigger("doClose");
        portal.gameObject.SetActive(false);
        doorCollider.gameObject.SetActive(false);
    }
}
