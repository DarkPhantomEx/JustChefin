using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // FIRST MOVEMENT IMPLEMENTATION
    private Rigidbody body;

    private float vertical;
    private float horizontal;
    public float speed;
    public float rotationSpeed;
    public float jumpForce;
    private Vector3 velocity;
    private int jumpCount;
    private Animator playerAnim;
    private Quaternion targetRotation;

    private bool isGrounded;
    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody>();
        playerAnim = GetComponent<Animator>();
        targetRotation = transform.rotation;
    }

    void Update()
    {
        vertical = Input.GetAxis("Vertical");
        horizontal = Input.GetAxis("Horizontal");
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isGrounded)
            {
                jumpCount = 0;
                playerAnim.SetBool("isSecondJumping", false);
                playerAnim.SetBool("isIdle", true);
            }
                
            if (jumpCount < 2)
            {
                body.AddForce(transform.up * jumpForce);
                jumpCount++;
                if (jumpCount == 2)
                {
                    playerAnim.SetBool("isSecondJumping", true);
                    /*targetRotation *= Quaternion.AngleAxis(90, Vector3.right);
                    transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, 10.0f * Time.deltaTime);*/
                }                
            }
        }
    }

    void FixedUpdate()
    {
        velocity = (transform.forward * vertical * speed * Time.fixedDeltaTime) + (transform.right * horizontal * speed * Time.fixedDeltaTime);
        velocity.y = body.velocity.y;
        body.velocity = velocity;
        //transform.Rotate(transform.up * horizontal * rotationSpeed * Time.fixedDeltaTime);
        /*movement = new Vector3(horizontal, 0, vertical) * speed * Time.fixedDeltaTime;
        movement.y = body.velocity.y;
        body.velocity = movement;*/
        /* if (movement != Vector3.zero)
             transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movement), 0.15f);*/
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isGrounded = true;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isGrounded = false;
        }
    }

/*  SECOND MOVEMENT IMPLEMENTATION    
    [SerializeField]
    private Rigidbody playerBody;
    private Vector3 inputVector;

    private void Start()
    {
        playerBody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        inputVector = new Vector3(Input.GetAxis("Horizontal") * 10f, playerBody.velocity.y, Input.GetAxis("Vertical") * 10f);
        transform.LookAt(transform.position + new Vector3(inputVector.x, 0, inputVector.z));
        playerBody.velocity = inputVector;
    }*/
}
