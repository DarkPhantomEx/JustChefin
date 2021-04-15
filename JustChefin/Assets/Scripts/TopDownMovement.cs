using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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
    // Audio variable
    private bool Playing = false;
    private float StartTime;
    private float duration;

    // Boolean to check if player can move
    [SerializeField]
    bool canMove;

    Transform PlayerTransform;

    //Crouch
    Vector3 crouchScale = new Vector3(0, 0.3f, 0);
    Vector3 crouchPos = new Vector3(0, 0.3f, 0);
   //Vector3 defaultPos;
    bool isCrouching;

    private Vector3 RaycastPointOffset;
    private Vector3 pointToRaycastFrom;

    void Start()
    {
        //Initializes PlayerTransform to Transform of the PlayerCharacter, also saves teh default position
        PlayerTransform = GameObject.FindGameObjectWithTag("MainPlayer").GetComponent<Transform>();
        //defaultPos = PlayerTransform.position;
        isCrouching = false;

        // Initially player can move
        SetCanMove(true);

        // Height of the point to start raycasting from
        RaycastPointOffset = new Vector3(0f, 10f, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.DrawRay(this.transform.position, this.transform.up * 10f, Color.green);
        //Debug.Log(this.gameObject.transform.position);
        // If player is allowed to move
        if (GetCanMove())
        {
            // Get arrow inputs
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");
            verticalSpeed += gravity * Time.deltaTime;
            Vector3 movement = new Vector3(horizontal, 0f, vertical).normalized;
            Vector3 movementY = Vector3.up * verticalSpeed;

            //Crouching
            if (Input.GetKeyDown(KeyCode.C))
            {
                //Ternary operators - based on isCrouching, changes scale/ht accordingly. Likewise for movementSpeed.
                PlayerTransform.localScale += crouchScale * (isCrouching ? 1 : -1);
                PlayerTransform.position += crouchPos * (isCrouching ? 1 : -1);
                moveSpeed += (isCrouching ? 3f : -3f);
                isCrouching = !isCrouching;
            }

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
                if (!Playing)
                {
                    StartTime = Time.time;
                    if (isCrouching)
                    {
                        AudioManager.instance.Crouching.start();                        
                        Playing = true;                        
                        duration = 1.15f;
                    }
                    else
                    {
                        AudioManager.instance.Walking.start();
                        Playing = true;
                        duration = 0.75f;
                    }
                }
                // Disable Nav Mesh Obstacle when moving
                this.GetComponent<NavMeshObstacle>().enabled = false;
            }
            else
            {
                if (Time.time - StartTime > duration)
                {
                    AudioManager.instance.Crouching.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                    AudioManager.instance.Walking.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                    Playing = false;
                }
                // Enable Nav Mesh Obstacle when stationary
                this.GetComponent<NavMeshObstacle>().enabled = true;
            }
        }
    }

    private void FixedUpdate()
    {
        // Updated point to ray cast from as per player's position
        pointToRaycastFrom = this.transform.position + RaycastPointOffset;
    }

    // Returns the roof gameobject if raycast from player to up vector hits a roof
    public GameObject GetRaycastedRoof()
    {
        RaycastHit hit;
        if (Physics.Raycast(pointToRaycastFrom, this.transform.position - pointToRaycastFrom, out hit))
        {
            if (hit.collider.tag.Contains("Roof"))
                return hit.collider.gameObject;
        }
        return null;
    }

    // Getter and Setter for player's ability to move
    public bool GetCanMove() { return canMove; }
    public void SetCanMove(bool canMove) { this.canMove = canMove; }
}
