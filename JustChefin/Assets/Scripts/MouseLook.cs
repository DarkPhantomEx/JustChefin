using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    [SerializeField] float mouseSensitivity = 100f;

    float xRotation = 0f;

    public Transform playerBody;

    

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        float MHInput = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float MVInput = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= MVInput;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0, 0);

        playerBody.Rotate(Vector3.up * MHInput);


    }
}
