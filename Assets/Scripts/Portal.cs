using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public Vector2 sendLocation;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("플레이어 닿음");
            GameObject.Find("Player").GetComponent<Player>().transform.position = new Vector2(sendLocation.x, sendLocation.y);
            GameObject.Find("MainCamera").transform.position = new Vector2(sendLocation.x, sendLocation.y);
        }
    }
}
