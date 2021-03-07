using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownMovement : MonoBehaviour
{
    public CharacterController controller;

    public float moveSpeed = 6f;
    public float turnSmoothTime = 0.1f;
    public float gravity = -9.81f;
    private float verticalSpeed = 0;
    float turnSmoothVelocity;

    // Update is called once per frame
    void Update()
    {
        if (this.gameObject.GetComponent<PlayerStatus>().isAlive())
        {
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");
            verticalSpeed += gravity * Time.deltaTime;
            Vector3 movement = new Vector3(horizontal, 0f, vertical).normalized;
            Vector3 movementY = Vector3.up * verticalSpeed;

            if (movement.magnitude >= 0.1f)
            {
                float targetAngle = Mathf.Atan2(movement.x, movement.z) * Mathf.Rad2Deg;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
                transform.rotation = Quaternion.Euler(0f, angle, 0f);
                controller.Move((movementY + (movement * moveSpeed)) * Time.deltaTime);
            }
        }
    }
}
