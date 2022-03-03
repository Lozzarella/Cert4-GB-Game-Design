using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//things that contain code use PascalCasing e.g., methods and classes
[AddComponentMenu("RPG/CHARACTER/MOVEMENT")]
[RequireComponent(typeof(CharacterController))]

public class PlayerMovement : MonoBehaviour
{
    //Variables or the data we are manipulating use camelCasing 
    //Access modifier, Type of data, reference name, optional assigned value

    [Header("Player Speeds")]
    [Tooltip("The speed applied to the character and what we pass to the animator controller")]
    public float speed = 0.0f; //the speed applied to the character and what we pass to the animator controller
    //these values below are so we have set values to change between
    [Tooltip("The speed for crouching")]
    public float crouchSpeed = 2.5f;//the speed for crouching
    [Tooltip("The speed for walking")]
    public float walkSpeed = 5.0f;//the speed for walking
    [Tooltip("The speed for running")]
    public float runSpeed = 10.0f;//the speed for running
    [Tooltip("The speed for jumping")]
    public float jumpSpeed = 8f;

    [Header("Directions")]
    [Tooltip("Left to right direction")]
    public float leftRight = 0.0f;//left to right direction
    [Tooltip("Forward to back direction")]
    public float forwardBack = 0.0f;//forward to back direction
    public Vector3 moveDirection;//vector 3 is floats x,y,z //we will use this to move in 3D space
    //we will do this by applying the leftRight value to our x acis of the world
    //and by applying the forwardBack value to our z axis of the world
    public float isCrouching = 0.0f; //this is what will control our 2 idle state crouching vs standing
    public float gravity = 20.0f;//character controller does not have inbuilt gravity so we make our own
    public float isJumping = 0.0f;//this is what will control our 3 idle state jumping vs standing vs crouching

    [Header("Components")]
    //When we create variables that connect to components on object in our game scene
    //we must then tell the component which object to get the component from
    [Tooltip("This is a reference to the player's animator/animator controller")]
    public Animator animator;//this is a refence to the player's animator/animator controller
    [Tooltip("Reference to player's character controller and allows us to move the player")]
    public CharacterController characterController;//this is a reference to players character controller, allows us to move the player

    // Start is called before the first frame update
    void Start()
    {
        //the variable is equal to the Animator component that this script is attached to
        animator = GetComponent<Animator>();
        //attaching the Animator on our game object to our reference
        characterController = GetComponent<CharacterController>();
        //attaching the CharacterController on our game object to our reference
    }

    // Update is called once per frame
    void Update()
    {
        if (characterController.isGrounded)//isGrounded is built into the characterController component
            //it checks if we are standing on a surface that has a collider attached to it
        {
            //using unity's inbuilt input system
            leftRight = Input.GetAxis("Horizontal");//get the input value for left and right
            forwardBack = Input.GetAxis("Vertical");//get the input value for forward and back
            //apply the inputs to our move direction value (variable)
            moveDirection = transform.TransformDirection(new Vector3 (leftRight, 0, forwardBack));
            //adjust speed
            //if this condition is met
            //if our value for leftRight isn't equal to 0 we are moving side to side
            //if our value for forwardBack isn't equal to 0 we are moving forward and back
            //if our leftRight value is not equal to 0 or our forwardBack is not equal to 0 then we are moving

            if (leftRight != 0 || forwardBack != 0)
            {
                //if we are sprinting
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    //set our speed to runSpeed
                    speed = runSpeed;
                }

                //else if we are crouching
                else if (Input.GetKey(KeyCode.LeftControl))
                {
                    //set our speed to crouchSpeed
                    speed = crouchSpeed;
                }

               //we must be walking
                else
                {
                    //set our speed to walkSpeed
                    speed = walkSpeed;
                }
            }

            else//we are not moving
            {
                //set our speed to 0
                speed = 0;
                //we are not moving
                //if er are pressing the crouch button
                if (Input.GetKey(KeyCode.LeftControl))
                {
                    //we are crouching
                    isCrouching = 0;
                }
                else//we are not pressing crouch
                {
                    //so we are standing
                    isCrouching = 1;
                }
            }

            //apply the speed that we set to our direction
            moveDirection *= speed;

            if (Input.GetButton("Jump"))
            {
                moveDirection.y = jumpSpeed;
                isJumping = 1; //we are jumping
            }

            else
            {
                isJumping = 0; //we are not jumping
            }

        }
        //apply a downward force to the character that simulates gravity
        moveDirection.y -= gravity * Time.deltaTime;
        //using the CharacterController, we are utilizing the inbuilt Move function to apply our movement
        characterController.Move(moveDirection * Time.deltaTime);
        //connect our values to our animations
        //apply speed value to the animator 
        animator.SetFloat("Speed",speed);
        //apply leftRight value to the animator
        animator.SetFloat("LeftRight",leftRight);
        //apply forwardBack value to the animator
        animator.SetFloat("ForwardBack", forwardBack);
        //apply IsCrouching value to the animator
        animator.SetFloat("IsCrouching", isCrouching);
        //apply jumpSpeed value to the animator
        animator.SetFloat("IsJumping", isJumping);
    }
}
