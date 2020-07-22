using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    private Transform playerTransform;
    private float followSpeed = 1f;
    private float offsetX = 0f;
    private float offsetY = 0f;
    private float offsetZ = -10f;

    private Vector3 cameraPosition;
    // Start is called before the first frame update
    void Start()
    {
        playerTransform = GameObject.Find("Player").GetComponent<Transform>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        cameraPosition.x = playerTransform.position.x + offsetX;
        cameraPosition.y = playerTransform.position.y + offsetY;
        cameraPosition.z = playerTransform.position.z + offsetZ;

        transform.position = 
            Vector3.Lerp(transform.position, cameraPosition, followSpeed * Time.deltaTime);
    }
}
