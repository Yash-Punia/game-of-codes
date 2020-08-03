using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] float minX;
    [SerializeField] float maxX;
    [SerializeField] float minY;
    [SerializeField] float maxY;
    [SerializeField] float minZ;
    [SerializeField] float maxZ;
    [SerializeField] float zoomMin;
    [SerializeField] float zoomMax;
    [SerializeField] float movementSpeed;

    // Update is called once per frame
    void Update()
    {
        if(Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
        {
            Vector2 touchDeltaPosition = Input.GetTouch(0).deltaPosition;
            transform.Translate(-touchDeltaPosition.x * movementSpeed, 0, -touchDeltaPosition.y * movementSpeed);

            transform.position = new Vector3(
                Mathf.Clamp(transform.position.x, minX, maxX),
                5,
                Mathf.Clamp(transform.position.z, minZ, maxZ));
        }
    }
}
