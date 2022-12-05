using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerCamera))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(AudioSource))]
public class PlayerMovement : MonoBehaviour 
{
    
    [Header("Components")]
    [SerializeField] PlayerCamera playerCamera;
    [SerializeField] Rigidbody rb;
    [SerializeField] CapsuleCollider capsuleCollider;
    [Tooltip("Ground collision detector")]
    [SerializeField] Transform feet;
    [SerializeField] AudioSource breathingSource;
    [Header("Movement Settings")]

    [Header("Direction speed")]
    [Tooltip("W speed")]
    [SerializeField] float movingFrontSpeed;
    [Tooltip("S speed")]
    [SerializeField] float movingBackSpeed;
    [Tooltip("A/D speed")]
    [SerializeField] float movingSidesSpeed;

    [Header("Base")]
    [Tooltip("How fast player speed will increase")]
    [SerializeField] float baseSpeedForce;
    [Tooltip("Maximum speed that player can reach")]
    [SerializeField] float baseMoveSpeed;

    [Header("Sprint")]
    [Tooltip("baseSpeedForce is multiplied by this value")]
    [SerializeField] float sprintSpeedForce;
    [Tooltip("baseMoveSpeed is multiplied by this value")]
    [SerializeField] float sprintMoveSpeed;

    [Header("Crouch")]
    [Tooltip("baseSpeedForce is multiplied by this value")]
    [SerializeField] float crouchSpeedForce;
    [Tooltip("baseMoveSpeed is multiplied by this value")]
    [SerializeField] float crouchMoveSpeed;

    [Header("Jump and fall")]
    [Tooltip("baseSpeedForce is multiplied by this value")]
    [SerializeField] float jumpSpeedForce;
    [Tooltip("baseMoveSpeed is multiplied by this value")]
    [SerializeField] float jumpMoveSpeed;
    [Tooltip("How high player can jump")]
    [SerializeField] float jumpHeight;
    [SerializeField] float fallMultiplier;
    [Header("Stamina")]
    [SerializeField] float staminaLength;
    public float StaminaLength { get { return staminaLength; } }
    [SerializeField] float staminaRegenerartionSpeed;


    private float currentSpeedForce;
    private float currentMoveLimit;
    [HideInInspector] public float currentStamina;
    public float CurrentStamina { get { return currentStamina; } }
    [Header("Drag")]
    [SerializeField] float groundDrag;
    [SerializeField] float airDrag;

    [Header("Slope")]
    [SerializeField] float maxSlopeAngle;
    private RaycastHit slopeHit;
    [Header("LayerMask")]
    public LayerMask groundLayer;

    [Header("Collider Height")]
    [SerializeField] float defaultHeight;
    [SerializeField] float crouchHeight;
    [Tooltip("How long is crouch transition")]
    [SerializeField] float crouchDuration;
    float crouchVelocity;
    private float currentHeight;
    public MovementActions movementActions;
    private bool crouch = false;
    public bool Crouch { get { return crouch; } }
    [HideInInspector] public bool jumping = false;
    private bool moving = false;
    public bool Moving { get { return moving; } }
    private bool sprinting = false;
    private bool canSprinting = true;
    private bool isExhausted = false;
    public bool Sprinting { get { return sprinting; } }
    private bool isGrounded = false;
    public bool IsGrounded{get{return isGrounded;} set{isGrounded = value;}}
    private Coroutine crouchTransition;
    private float horizontalMove;
    private float verticalMove;
    private Vector3 movementDirection;
    public static PlayerMovement Instance { get; private set; }
    public static Action onSprinting;
    public PlayerControls playerControls;
    private void Awake()
    {
        if (Instance != this && Instance != null)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
        playerControls = new PlayerControls();
        InitActions();
    }
    private void OnEnable()
    {
        playerControls.Enable();
    }
    void Start()
    {
        if (capsuleCollider == null || rb == null || playerCamera == null || breathingSource == null || feet == null)
            Debug.LogError(this.GetType().Name + " not all components attached.");

        capsuleCollider.height = defaultHeight;
        feet.localPosition = new Vector3(0,-capsuleCollider.height/2,0);
        currentStamina = staminaLength;
        
        SetSpeed();
    }


    void Update()
    {
        if(rb.velocity.y < 0)
        {
            rb.velocity += new Vector3(0,Physics.gravity.y * (fallMultiplier-1)*Time.deltaTime,0);
        }
        
        if (playerControls.Player.Sprint.IsPressed() && moving && isGrounded && !isExhausted)
        {
            
            //if (currentSpeedForce != baseSpeedForce * sprintSpeedForce) SetSpeed(sprintSpeedForce);
            
            if (currentStamina > 0.0f)
            {
                currentStamina -= Time.deltaTime;
               // onSprinting();
            }
            else
            {
                Debug.Log("Exhaustion!");
                Exhaustion(true);
                OnSprintingCanceled();
            }
        }
        //If player was exhausted ,but stamina is regenerated fully variable isExhausted is set to false
        else if (currentStamina < staminaLength)
        {
            currentStamina += Time.deltaTime * staminaRegenerartionSpeed;
            if (currentStamina >= staminaLength)
            {
                currentStamina = staminaLength;
                if (isExhausted == true)
                {
                    Exhaustion(false);
                }
            }
        }
        
        DragControl();
        SpeedController();

    }
    private void FixedUpdate()
    {
        Movement();
    }
    private void OnDisable()
    {
        playerControls.Disable();
    }
    void Movement()
    {

        if (verticalMove != 0 || horizontalMove != 0)
        {
            moving = true;
        }
        else
        {
            moving = false;
        }
        // Debug.Log($"jumping:{!jumping}");
        // Debug.Log($"moving:{!moving}");
        // Debug.Log($"isGrounded:{isGrounded}");

       // if(!moving && !jumping && isGrounded) rb.Sleep();
        movementDirection = playerCamera.orientation.forward * horizontalMove + playerCamera.orientation.right * verticalMove;

        if(isGrounded && OnSlope())
        {
            rb.AddForce(GetSlopeMoveDirection() * currentSpeedForce,ForceMode.Acceleration);
            if(rb.velocity.y > 0 || rb.velocity.y < 0) rb.AddForce(Vector3.down*80f, ForceMode.Force);
        } 
        else if(isGrounded && !OnSlope()) rb.AddForce(movementDirection.normalized * currentSpeedForce,ForceMode.Acceleration);
        else if(!isGrounded && !OnSlope()) rb.AddForce(movementDirection.normalized * currentSpeedForce, ForceMode.Acceleration);
    }

    public void SpeedController()
    {
        Vector3 currentMoveSpeed = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        if (currentMoveSpeed.magnitude >= currentMoveLimit)
        {
            Vector3 limitCurrentMoveSpeed = currentMoveSpeed.normalized * currentMoveLimit;
            rb.velocity = new Vector3(limitCurrentMoveSpeed.x, rb.velocity.y, limitCurrentMoveSpeed.z);
        }
    }

    public void OnMove(InputAction.CallbackContext ctx)
    {
        if (!jumping && !crouch && !sprinting && currentSpeedForce != baseSpeedForce) SetSpeed();
        //Debug.Log("OnMove started");

        Vector2 value = ctx.ReadValue<Vector2>();
        //Debug.Log(value);

        verticalMove = value.x;
        verticalMove = verticalMove > 0 ? verticalMove *= movingFrontSpeed : verticalMove < 0 ? verticalMove *= movingBackSpeed : 0;
        //Debug.Log("VerticalMove:"+verticalMove);

        horizontalMove = value.y;
        horizontalMove *= movingSidesSpeed;
        // Debug.Log("HorizontalMove:"+horizontalMove);
    }
    public void OnJump(InputAction.CallbackContext ctx)
    {
        if (isGrounded)
        {
            movementActions = MovementActions.JUMPING;
            jumping = true;
            SetSpeed(jumpSpeedForce);
            Jump();
        }
    }
    private void Jump() => rb.AddForce(new Vector3(0, Mathf.Sqrt(-2.0f * Physics2D.gravity.y * jumpHeight), 0), ForceMode.Impulse);
    public void OnSprint(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            OnSprintingStart();
        }
        else if (ctx.canceled)
        {

            OnSprintingCanceled();
        }
    }
    public void OnSprintingStart()
    {
        Debug.Log("Sprint Started");

        //If player isn't exhausted (exhaustion state is triggered when player is sprinting to 0 stamina) sprint is triggered
        if (currentStamina > 0.0f && !isExhausted)
        {
            if (!jumping && !crouch) SetSpeed(sprintSpeedForce);
            sprinting = true;
            onSprinting();
        }
    }
    public void OnSprintingCanceled()
    {
        Debug.Log("Sprint canceled");
        if (sprinting)
        {
            sprinting = false;
            if (!crouch && !jumping) SetSpeed();
        }

    }
    public void OnCrouch(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            OnCrouchStarted();
           //Debug.Log("started");
        }

        if (ctx.canceled)
        {
            OnCrouchCanceled();
            //Debug.Log("canceled");
            
        }
    }
    public void OnCrouchStarted()
    {
        if (movementActions != MovementActions.JUMPING)
        {
            movementActions = MovementActions.CROUCHING;
            crouch = true;
            SetSpeed(crouchSpeedForce);
            if (crouchTransition != null) StopCoroutine(crouchTransition);
            crouchTransition = StartCoroutine(CrouchTransition(crouchHeight, crouchDuration));
        }
    }
    public void OnCrouchCanceled()
    {
        crouch = false;
        if(capsuleCollider.height != defaultHeight) capsuleCollider.height = defaultHeight;
        feet.localPosition = new Vector3(0,-capsuleCollider.height/2,0);
        SetSpeed();
    }
    IEnumerator CrouchTransition(float desiredValue, float crouchDur)
    {
        float timeElapsed = 0f;
        float currentHeight = capsuleCollider.height;
        while (timeElapsed <= crouchDur)
        {
            if(!crouch)
            {
                yield break;
            } 
            float currValue = Mathf.Lerp(currentHeight, desiredValue, timeElapsed / crouchDur);
            capsuleCollider.height = currValue;
            feet.localPosition = new Vector3(0,-currValue/2,0);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        capsuleCollider.height = desiredValue; 
        feet.localPosition = new Vector3(0,-capsuleCollider.height/2,0);
        crouchTransition = null;
    }

   
    
    private bool OnSlope()
    {
        if(Physics.Raycast(transform.position,Vector3.down,out slopeHit, capsuleCollider.height * 0.5f + 0.3f,layerMask:groundLayer))
        {
            float angle = Vector3.Angle(Vector3.up,slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }
        return false;
    }
    private Vector3 GetSlopeMoveDirection()
    {
        return Vector3.ProjectOnPlane(movementDirection,slopeHit.normal).normalized;
    }
    private void CheckSlope()
    {
        RaycastHit slopeHit;
        if (Physics.Raycast(capsuleCollider.bounds.center, Vector3.down, out slopeHit, capsuleCollider.bounds.extents.y + 0.1f))
        {
            //    Debug.Log(Vector3.Angle(Vector3.up,slopeHit.normal));
        }
    }

    private void OnDrawGizmos()
    {
        Vector3 pos = new Vector3(capsuleCollider.bounds.center.x, capsuleCollider.bounds.min.y, capsuleCollider.bounds.center.z);
        Debug.DrawRay(pos, Vector3.down, Color.blue);
    }
    public void RestoreStamina()
    {
        currentStamina = staminaLength;
        onSprinting();
    }

    ///<summary>Set value of speed - default is 1 which is equal to normal walk.Value can't be equal 0.</summary>
    public void SetMoveBasedOnState()
    {
        //If sprint is pressed ,sprinting start will execute
        if(playerControls.Player.Sprint.IsPressed())
        {
            OnSprintingStart();
        }
        //If crouch is pressed ,crouching start will execute
        else if(playerControls.Player.Crouch.IsPressed())
        {
            OnCrouchStarted();
        }
        else if(jumping)
        {
            SetSpeed(jumpMoveSpeed);
        }
        //If any of above conditions isn't fullfiled ,move is going to default state 
        else
        {
            SetSpeed();
        }
    }
    private void DragControl()
    {
        if(isGrounded) rb.drag = groundDrag;
        else rb.drag = airDrag;
    }
    private void SetSpeed(float value = 1)
    {
        if (value == 0) Debug.LogWarning($"{value} can't be equal to 0 !");
        else
        {
            currentSpeedForce = baseSpeedForce * value;
            currentMoveLimit = baseMoveSpeed * value;
        }
    }
    public void Exhaustion(bool value) => SetExhaustion(value);
    public void SetExhaustion(bool value)
    {
        isExhausted = value;
        breathingSource.enabled = value;
    }

    public void InitActions()
    {

        playerControls.Player.Move.started += OnMove;
        playerControls.Player.Move.performed += OnMove;
        playerControls.Player.Move.canceled += OnMove;

        playerControls.Player.Sprint.started += OnSprint;
        playerControls.Player.Sprint.canceled += OnSprint;

        playerControls.Player.Jump.started += OnJump;

        playerControls.Player.Crouch.started += OnCrouch;
        playerControls.Player.Crouch.canceled += OnCrouch;
    }
}


