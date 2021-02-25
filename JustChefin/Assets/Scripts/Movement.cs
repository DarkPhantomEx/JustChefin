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
    //Vector3 moveDirection;
    float VInput, HInput;
    //float jumpHeight = 3;
    float sprint = 2.5f;


    public CharacterController controller;

    bool isCrouching; 
    float crouchFactor; //if Crouching, slows character speed.

    
    void Start()
    {
        //moveDirection = Vector3.zero;
        isCrouching = false;
        crouchFactor = 1.0f;
    }

    // Update is called once per frame
    void Update()
    {

        VInput = Input.GetAxis("Vertical");
        HInput = Input.GetAxis("Horizontal");
        //move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        Vector3 move = new Vector3(HInput, 0, VInput);
        if(move != Vector3.zero)
        transform.rotation = Quaternion.LookRotation(move);
        move = Vector3.right * HInput *crouchFactor + Vector3.forward * VInput *crouchFactor;

        //move.y += gravity * Time.deltaTime;
        //controller.Move(velocity * Time.deltaTime); // deltaY = 1/2g t^2     


        if (move != Vector3.zero)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                controller.Move(move * speed * sprint * Time.deltaTime);

            }
            controller.Move(move * speed * Time.deltaTime);

        }

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
  
        
    }

}
