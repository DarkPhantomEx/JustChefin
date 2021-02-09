using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.UI;


public class Movement : MonoBehaviour
{
    [SerializeField] float speed = 12.5f;
    public float gravity = -9.81f;

    Vector3 velocity; 
    float VInput, HInput;
    //float jumpHeight = 3;
    float sprint = 2.5f;


    public CharacterController controller;

    //public Transform groundCheck, wallCheckF;
    //public float groundDist = 0.4f;
    //public float wallDist = 0.2f;
    //public LayerMask groundMask;
    //public LayerMask wallMask;

    //bool isGrounded;    
    //bool isFacingWall;
    
    bool isCrouching; 
    float crouchFactor; //if Crouching, slows character speed.

    // Variables for double jump mechanics and animation
    //private Animator playerAnim;
    //int jumpCount;
    //float dashSpeed = 1000.0f;
    //[SerializeField]
    //bool somerSault = false;
    //[SerializeField]
    //bool dashing = true;

    // Variables for Score counting
    //private int score = 0;
    //public Text scoreText;

    void Start()
    {
        isCrouching = false;
        crouchFactor = 1.0f;
        //playerAnim = GetComponentInChildren<Animator>();
        //jumpCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //isGrounded = Physics.CheckSphere(groundCheck.position, groundDist, groundMask);
        //isFacingWall = Physics.CheckSphere(wallCheckF.position, wallDist, wallMask);

        //if(isGrounded && velocity.y < 0)
        //{
        //    velocity.y = -2f;
        //    jumpCount = 0;
        //    playerAnim.SetBool("isIdle", true);
        //    playerAnim.SetBool("isSecondJumping", false);
        //}

        VInput = Input.GetAxis("Vertical");
        HInput = Input.GetAxis("Horizontal");

        //transform.position += new Vector3(HInput , 0, VInput);

        Vector3 move = transform.right * HInput *crouchFactor + transform.forward * VInput *crouchFactor;

        if(Input.GetKey(KeyCode.LeftShift)) // removed "&& isGrounded)" as sprinting followed by jumping was a bit odd PS: IT'S STILL ODD
        {
            controller.Move(move * speed * sprint * Time.deltaTime);
        }

        controller.Move(move * speed * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.C)) //"Crouching" LOL
        {
            //Makes the mesh appear as if it's "crouching" up

            if (!isCrouching) 
            {
                isCrouching = true;
                crouchFactor = 0.5f;
                this.transform.localScale-= new Vector3(0.0f,0.5f,0.0f);  //Squashes mesh if crouching          
            }
            else
            {
                isCrouching = false;
                crouchFactor = 1.0f;
                this.transform.localScale += new Vector3(0.0f, 0.5f, 0.0f);
            }
            
        }

        //if(Input.GetButtonDown("Jump")) // removed && isGrounded)
        //{
        //    if (jumpCount < 2)
        //    {
        //        if (jumpCount == 0)
        //        {
        //            velocity.y += Mathf.Sqrt(jumpHeight * -2f * gravity);
        //            jumpCount++;
        //        }
        //        else if (jumpCount == 1)
        //        {
        //            jumpCount++;
        //        }
        //        if (jumpCount == 2)
        //        {
        //            if(dashing)
        //                controller.Move(transform.forward * dashSpeed * Time.deltaTime);
        //            if(somerSault)
        //            {
        //                velocity.y += Mathf.Sqrt(jumpHeight * -2f * gravity);
        //                playerAnim.SetBool("isIdle", false);
        //                playerAnim.SetBool("isSecondJumping", true);
        //            }
        //            //transform.position = Vector3.Lerp(transform.position, new Vector3(0, 0, 10), Time.deltaTime);
        //        }
        //    }
        //}

        //if(!isFacingWall)


        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime); // deltaY = 1/2g t^2
        //if(Input.GetKeyDown(KeyCode.E))
        //{
        //    if(dashing && !somerSault)
        //    {
        //        dashing = false;
        //        somerSault = true;
        //    }
        //    else if(!dashing && somerSault)
        //    {
        //        dashing = true;
        //        somerSault = false;
        //    }
        //}
    }

    //private void OnControllerColliderHit(ControllerColliderHit hit)
    //{
    //    if (hit.collider.tag == "Collectible")
    //    {
    //        Destroy(hit.collider.gameObject);
    //        score += 10;
    //        scoreText.text = "Score: " + score.ToString();
    //    }
    //}
}
