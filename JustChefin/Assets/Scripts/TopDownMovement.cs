﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownMovement : MonoBehaviour
{
    // Reference to the character controller
    public CharacterController controller;

    // Player movement properties
    public float moveSpeed = 6f;
    public float gravity = -9.81f;
    private float verticalSpeed = 0;
    // Player rotation properties
    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;

    // Boolean to check if player can move
    [SerializeField]
    bool canMove;

    void Start()
    {
        // Initially player can move
        SetCanMove(true);    
    }

    // Update is called once per frame
    void Update()
    {
        // If player is allowed to move
        if (GetCanMove())
        {
            // Get arrow inputs
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");
            verticalSpeed += gravity * Time.deltaTime;
            Vector3 movement = new Vector3(horizontal, 0f, vertical).normalized;
            Vector3 movementY = Vector3.up * verticalSpeed;

            // If the input is significant
            if (movement.magnitude >= 0.1f)
            {
                // Find the angle between the X and Z vector of the input provided
                float targetAngle = Mathf.Atan2(movement.x, movement.z) * Mathf.Rad2Deg;
                // Smooth the rotation towards where player is moving
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
                // Set the rotation as set above
                transform.rotation = Quaternion.Euler(0f, angle, 0f);
                // Move the player based on movement input
                controller.Move((movementY + (movement * moveSpeed)) * Time.deltaTime);
            }
        }
    }

    // Getter and Setter for player's ability to move
    public bool GetCanMove() { return canMove; }
    public void SetCanMove(bool canMove) { this.canMove = canMove; }
}
