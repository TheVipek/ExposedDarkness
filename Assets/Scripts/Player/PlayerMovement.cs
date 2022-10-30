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

    [SerializeField] KeyCode crouchKey = KeyCode.LeftControl;

    [Header("Components")]
    [SerializeField] Camera _mCamera;
    [SerializeField] Rigidbody rb;
    [SerializeField] CapsuleCollider capsuleCollider;

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
    [SerializeField] bool isGrounded = false;
    public bool IsGrounded{get{return isGrounded;}}
    Coroutine crouchTransition;
    float gravity = -9.81f;
    float horizontalMove;
    float verticalMove;
    Vector3 velocity;
    float rigidbodySpeed;
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
        if(capsuleCollider == null) gameObject.GetComponent<CapsuleCollider>();
        if(rb == null) GetComponent<Rigidbody>();
        if(_mCamera == null) PlayerCamera.instance.gameObject.GetComponent<Camera>();
        Cursor.lockState = CursorLockMode.Locked;
        capsuleCollider.height = defaultHeight;
    }

    void Update()
    {
        CameraMouseLook();
        ProcessCrouch();
        Movement();
        

        if(rb.velocity.y > 0)
        {
            isGrounded = false;
        }
        
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
        if(moving == true)
        {
            if (Input.GetKey(sprintKey))
            {
                position *= sprintMultiplier;
                sprinting = true;
            }
            else if(Input.GetKey(gaitKey) && crouch == false)
            {
                position /= 1.5f;
                gaiting = true;
                
            }
            else
            {
                sprinting = false;
                gaiting = false;
            }
        }
        else
        {
            sprinting = false;
            gaiting = false;
        }
        

        if(crouch == true)
        {
            position /= 2f;
        }
        if(Input.GetKeyDown(jumpKey) && isGrounded == true)
        {
            // formula 
           rb.AddForce(new Vector3(0,Mathf.Sqrt(jumpStrength * -2 * Physics.gravity.y),0),ForceMode.Impulse);
      
           //isGrounded = false;
           // velocity.y = Mathf.Sqrt(jumpStrength * -2 * gravity);
        
        }
        // multiplying it by Time.deltaTime to make it frame-independent
        //velocity.y += gravity * Time.deltaTime;
        
        //Moving it

        //frame independent
        transform.Translate(position * Time.deltaTime);
        // physics of free fall
        /*if(isGrounded == false)
        {
            //transform.Translate(velocity * Time.deltaTime);

        }*/
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
        isGrounded=true;
    }
}


