using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject player;
    public float smoothFactor = 10f;
    private Vector3 cameraOffset;

    // Start is called before the first frame update
    void Start()
    {
        // Saves the current camera position in the editor
        cameraOffset = transform.position - player.transform.position;
    }

    // LateUpdate is called once per frame
    void LateUpdate()
    {
        // Calculate new position of the camera as per player's position
        Vector3 newPosition = player.transform.position + cameraOffset;
        // Set camera's position
        transform.position = Vector3.Slerp(transform.position, newPosition, smoothFactor);
    }
}
