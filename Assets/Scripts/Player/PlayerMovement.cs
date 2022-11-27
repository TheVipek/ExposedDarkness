using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class PlayerMovement : MonoBehaviour
{

    [Header("Movement Values")]
    [SerializeField] float movingFrontSpeed;
    [SerializeField] float movingBackSpeed;
    [SerializeField] float movingSidesSpeed;
    [SerializeField] float sprintMultiplier;
    [SerializeField] float jumpStrength;
    [SerializeField] float staminaLength;
    [SerializeField] float staminaRegenerartionSpeed;
    public float currentStamina;
    public float CurrentStamina {get{return currentStamina;}}
    public float StaminaLength {get{return staminaLength;}}



    [Header("Binds")]
    [SerializeField] KeyCode sprintKey = KeyCode.LeftShift;
    [SerializeField] KeyCode jumpKey = KeyCode.Space;

    [SerializeField] KeyCode crouchKey = KeyCode.LeftControl;

    [Header("Components")]
    [SerializeField] Camera _mCamera;
    [SerializeField] Rigidbody rb;
    [SerializeField] CapsuleCollider capsuleCollider;
    [SerializeField] AudioSource breathingSource;

    [Header("Camera Settings")]
    [SerializeField] float defaultHeight;
    [SerializeField] float crouchHeight;
    [SerializeField] float crouchDuration;
    float crouchVelocity;



    [Header("Mouse Values")]
    [Tooltip("This one is truly for sensivity in game that players are allowed to change via options")]
    [SerializeField] Vector2 defaultSensitivity;
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
    private bool canSprinting = true;
    public bool Sprinting{get{return sprinting;}}
    [SerializeField] bool isGrounded = false;
    public bool IsGrounded{get{return isGrounded;}}
    [SerializeField] bool canMove = true;
    Coroutine crouchTransition;
    float horizontalMove;
    float verticalMove;
    public static PlayerMovement Instance {get; private set;}
    public static Action onSprinting;
    private void Awake() {
        if(Instance!= this && Instance != null)
        {
            Destroy(this);
        }else
        {
            Instance = this;
        }
    }
    void Start()
    {
        if(capsuleCollider == null) gameObject.GetComponent<CapsuleCollider>();
        if(rb == null) GetComponent<Rigidbody>();
        if(_mCamera == null) PlayerCamera.instance.gameObject.GetComponent<Camera>();
        Cursor.lockState = CursorLockMode.Locked;
        capsuleCollider.height = defaultHeight;
        currentStamina = staminaLength;
    }

    void Update()
    {
        ProcessCrouch();
        Jumping();
        if(rb.velocity.y > 0)
        {
            isGrounded = false;
        }

        
    }
    private void FixedUpdate() {
        Movement();
    }
    private void LateUpdate() {
        CameraMouseLook();
    }
    void CameraMouseLook()
    {
        xMouse = (Input.GetAxis("Mouse X") * xSensitivity * Time.deltaTime) * defaultMouseSensivityMultiplier;
        yMouse = (Input.GetAxis("Mouse Y") * ySensitivity * Time.deltaTime) * defaultMouseSensivityMultiplier;
        
        xRotation -= yMouse;
        xRotation = Mathf.Clamp(xRotation, -maxXAngle, maxXAngle);
        _mCamera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * xMouse,Space.Self);
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

            //Get local camera direction ,by using forward we get space ahead of us , by right we get on side and inverse those vectors to player direction
            Vector3 cameraForward = transform.InverseTransformVector(_mCamera.transform.forward);
            Vector3 cameraSides = transform.InverseTransformVector(_mCamera.transform.right);
        
            // this is changed to 0 to get rid of going unnecesary down or up 
            cameraForward.y = 0;
            cameraSides.y = 0;
            cameraForward = cameraForward.normalized;
            cameraSides = cameraSides.normalized;

            Vector3 position = cameraForward * verticalMove + cameraSides * horizontalMove;

            //Checking for sprint
            if(moving == true && isGrounded == true)
            {
                
                if (Input.GetKey(sprintKey) && currentStamina > 0.0f  && (sprinting == false && canSprinting == true))
                {
                    sprinting = true;
                    Debug.Log("Clicked sprintKey");
                    onSprinting();
                }

                if(sprinting == true && Input.GetKey(sprintKey))
                {
                    if(currentStamina > 0.0f)
                    {
                        position *= sprintMultiplier;
                     //   Debug.Log(currentStamina);
                        currentStamina -= Time.deltaTime;
                    }
                    else
                    {
                        sprinting = false;
                        canSprinting = false;
                        breathingSource.enabled = true;
                    }
                }
                else
                {
                    sprinting = false;
                }
            }
            else
            {
                sprinting = false;
            }
            
            if(sprinting == false && currentStamina < staminaLength)
            {
                currentStamina += Time.deltaTime * staminaRegenerartionSpeed;
                if(currentStamina >= staminaLength)
                {
                    currentStamina = staminaLength;
                    canSprinting = true;
                    breathingSource.enabled = false;

                }
            }

            if(crouch == true)
            {
                position /= 2f;
            }
            
            // multiplying it by Time.deltaTime to make it frame-independent
            //velocity.y += gravity * Time.deltaTime;
            
            //Moving it

            //frame independent
            transform.Translate(position * Time.fixedDeltaTime);
            // physics of free fall
            /*if(isGrounded == false)
            {
                //transform.Translate(velocity * Time.deltaTime);

            }*/

        
    }



    void Jumping()
    {
        if(Input.GetKeyDown(jumpKey) && isGrounded == true)
            {
                // formula 
            rb.AddForce(new Vector3(0,Mathf.Sqrt(jumpStrength * -2 * Physics.gravity.y),0),ForceMode.Impulse);
        
            //isGrounded = false;
            // velocity.y = Mathf.Sqrt(jumpStrength * -2 * gravity);
            
            }
    }
    void ProcessCrouch()
    {
        //Debug.Log(crouchTransition);
        if (Input.GetKeyDown(crouchKey) && crouchTransition == null && isGrounded == true)
        {
            crouch = !crouch;

            if (crouch == true)
            {
                Debug.Log("Crouching!");
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
            capsuleCollider.height = Mathf.Lerp(capsuleCollider.height, desiredValue, timeElapsed / durationCrouch);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        capsuleCollider.height = desiredValue;
        crouchTransition = null;
    }

    private void OnCollisionStay(Collision other) {
        if(other.gameObject.tag == "Ground")
        {
            isGrounded=true;
        }
    }

    public void RestoreStamina()
    {
        currentStamina = staminaLength;
        onSprinting();
    }
    public void setCustomMouseValues(float x,float y)
    {
        xSensitivity = x;
        ySensitivity = y;
    }
    public void setDefaultMouseValues()
    {
        xSensitivity = defaultSensitivity.x;
        ySensitivity = defaultSensitivity.y;
    }
}


