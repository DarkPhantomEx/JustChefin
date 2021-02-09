using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TPSCamera : MonoBehaviour
{
    /*    private const float Y_ANGLE_MIN = 0.0f;
        private const float Y_ANGLE_MAX = 50.0f;

        public Transform lookAt;
        private Transform camTransform;

        private Camera cam;

        private float distance = 10.0f;
        private float currentX = 0.0f;
        private float currentY = 0.0f;
        private float sensitivityX = 4.0f;
        private float sensitivityY = 1.0f;
        // Start is called before the first frame update
        void Start()
        {
            camTransform = transform;
            cam = Camera.main;
        }

        void Update()
        {
            currentX += Input.GetAxis("Mouse X");
            currentY -= Input.GetAxis("Mouse Y");

            currentY = Mathf.Clamp(currentY, Y_ANGLE_MIN, Y_ANGLE_MAX);
        }

        // Update is called once per frame
        void LateUpdate()
        {
            Vector3 dir = new Vector3(0, 0, -distance);
            Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);
            camTransform.position = lookAt.position + rotation * dir;
            camTransform.LookAt(lookAt.position);
        }*/

    private const float Y_ANGLE_MIN = 0.0f;
    private const float Y_ANGLE_MAX = 89.0f;

    public float rotationSpeed;
    public Transform Target, Player;

    [SerializeField]
    private float mouseX;
    [SerializeField]
    private float mouseY;

    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {

    }

    private void LateUpdate()
    {
        mouseX += Input.GetAxis("Mouse X") * rotationSpeed;
        mouseY -= Input.GetAxis("Mouse Y");

        mouseY = Mathf.Clamp(mouseY, Y_ANGLE_MIN, Y_ANGLE_MAX);
        transform.LookAt(Target);

        Target.rotation = Quaternion.Euler(mouseY, mouseX, 0);
        Player.rotation = Quaternion.Euler(0, mouseX, 0);
    }
}
