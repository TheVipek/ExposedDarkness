using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [Header("Movement Values")]
    [SerializeField] float movingFrontSpeed;
    [SerializeField] float movingBackSpeed;
    [SerializeField] float movingSidesSpeed;
    [SerializeField] float sprintMultiplier;
    [SerializeField] float jumpStrength;

    [Header("Binds")]
    [SerializeField] KeyCode sprintKey = KeyCode.LeftShift;
    [SerializeField] KeyCode gaitKey = KeyCode.LeftAlt;
    [SerializeField] KeyCode jumpKey = KeyCode.Space;

    [Header("Components")]
    [SerializeField] Camera _mCamera;
    [SerializeField] CharacterController controller;
    [SerializeField] Rigidbody rb;

    [Header("Camera Settings")]
    [SerializeField] float defaultHeight;
    [SerializeField] float crouchHeight;
    [SerializeField] float crouchDuration;
    float crouchVelocity;



    [Header("Mouse Values")]
    [Tooltip("This one is truly for sensivity in game that players are allowed to change via options")]
    public float xSensitivity;
    public float ySensitivity;
    [Tooltip("Default sensivity multiplier,better not change!")]
    public float defaultMouseSensivityMultiplier = 100f;
    float xMouse, yMouse;
    float xRotation;
    float maxXAngle = 50f;




    [Header("Other meaningfull movement stuff")]
    [SerializeField] bool crouch = false;
    public bool Crouch{get{return crouch;}}
    [SerializeField] bool jumping = false;
    [SerializeField] bool moving = false;
    public bool Moving{get{return moving;}}
    [SerializeField] bool sprinting = false;
    public bool Sprinting{get{return sprinting;}}
    [SerializeField] bool gaiting = false;
    public bool Gaiting{get{return gaiting;}}
    Coroutine crouchTransition;
    float gravity = -9.81f;
    float horizontalMove;
    float verticalMove;
    Vector3 velocity;


    public static PlayerMovement instance;
    private void Awake() {
        if(instance!= this && instance != null)
        {
            Destroy(this);
        }else
        {
            instance = this;
        }
    }
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        controller.height = defaultHeight;
    }

    void Update()
    {
        CameraMouseLook();
        ProcessCrouch();

    }
    private void FixedUpdate() {
        Movement();
        
    }
    void CameraMouseLook()
    {
        xMouse = (Input.GetAxis("Mouse X") * xSensitivity * Time.deltaTime) * defaultMouseSensivityMultiplier;
        yMouse = (Input.GetAxis("Mouse Y") * ySensitivity * Time.deltaTime) * defaultMouseSensivityMultiplier;
        
        xRotation -= yMouse;
        xRotation = Mathf.Clamp(xRotation, -maxXAngle, maxXAngle);
        _mCamera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * xMouse);
    }
    void Movement()
    {
        //Vertical - Z-axis
        verticalMove = Input.GetAxis("Vertical");
        verticalMove = verticalMove > 0 ? verticalMove *= movingFrontSpeed : verticalMove < 0 ? verticalMove *= movingBackSpeed : 0;

        //Horizontal - X-axis
        horizontalMove = Input.GetAxis("Horizontal") * movingSidesSpeed;
        if(verticalMove != 0 || horizontalMove != 0)
        {
            moving = true;
        }else
        {
            moving = false;
        }

        //Applying position and gravity
        Vector3 position = _mCamera.transform.forward * verticalMove + _mCamera.transform.right * horizontalMove;

        //Checking for sprint
        if (Input.GetKey(sprintKey))
        {
            position *= sprintMultiplier;
            sprinting = true;
        }
        else if(Input.GetKey(gaitKey) && crouch == false)
        {
            position /= 1.5f;
            gaiting = true;
            
        }else
        {
            sprinting = false;
            gaiting = false;
        }
        if(crouch == true)
        {
            position /= 2f;
        }
        if(Input.GetKeyDown(jumpKey) && controller.isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpStrength * -2f* gravity);
        }
        velocity.y += gravity * Time.deltaTime;
        //Moving
        controller.Move(position * Time.deltaTime);
        controller.Move(velocity * Time.deltaTime);
    }

    void ProcessCrouch()
    {
        //Debug.Log(crouchTransition);
        if (Input.GetKeyDown(KeyCode.LeftControl) && crouchTransition == null)
        {
            crouch = !crouch;

            if (crouch == true)
            {
                crouchTransition = StartCoroutine(CrouchTransition(crouchHeight, crouchDuration));
            }
            else
            {
                crouchTransition = StartCoroutine(CrouchTransition(defaultHeight, crouchDuration));

            }
        }
    }
    IEnumerator CrouchTransition(float desiredValue, float crouchDur)
    {
        float durationCrouch = crouchDur;
        float timeElapsed = 0f;
        while (timeElapsed <= durationCrouch)
        {
            controller.height = Mathf.Lerp(controller.height, desiredValue, timeElapsed / durationCrouch);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        controller.height = desiredValue;
        crouchTransition = null;
        /*while(controller.height != desiredValue)
        {
            controller.height = Mathf.SmoothDamp(controller.height,desiredValue,ref crouchVelocity,smoothTime);
            Debug.Log("Changing controller height: "+controller.height);
            yield return null;
        }
        Debug.Log("Setting desired after all: "+ controller.height);
        controller.height = desiredValue;
        crouchTransition = null;*/
    }
}


