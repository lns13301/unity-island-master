using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveBoard : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "EntityItem")
        {
            Debug.Log("아이템 닿음");
            Destroy(collision.gameObject);
        }
    }
}
