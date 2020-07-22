using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorRange : MonoBehaviour
{
    public Animator playerAnimator;
    public Player playerScript;
    public bool isPlayerIn;

    // Start is called before the first frame update
    void Start()
    {
        playerAnimator = GameObject.Find("Player").GetComponent<Animator>();
        playerScript = GameObject.Find("Player").GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            isPlayerIn = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            isPlayerIn = false;
        }
    }
}
