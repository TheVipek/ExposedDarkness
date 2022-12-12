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
    private float offSetY = 0.05f;
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
    [Tooltip("baseSpeedForce and baseMoveSpeed is multiplied by this value")]
    [SerializeField] float baseSprintMultiplier;
    [Header("Crouch")]
    [Tooltip("baseSpeedForce and baseMoveSpeed is multiplied by this value")]
    [SerializeField] float baseCrouchMultiplier;
    [Header("Jump and fall")]
    [Tooltip("baseSpeedForce and baseMoveSpeed is multiplied by this value")]
    [SerializeField] float baseJumpMultiplier;
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

    [Header("Current MovementAction")]
    public MovementActions movementActions;

    private bool crouch = false;
    public bool Crouch { get { return crouch; } }
    private bool jumping = false;
    public bool Jumping {get {return jumping;}  set{jumping = value;}}
    private bool moving = false;
    public bool Moving { get { return moving; } }
    private bool sprinting = false;
    private bool isExhausted = false;
    public bool Sprinting { get { return sprinting; } }
    private bool isGrounded = false;
    public bool IsGrounded{get{return isGrounded;} set{isGrounded = value;}}
    private Coroutine crouchTransition;
    private Vector3 preCrouchPos;
    private Vector3 move;
    private Vector3 moveDirection;

    public static Action onSprinting;
    
    [SerializeField] InputActionMap playerActionMap;
    public InputActionReference moveAction,jumpAction,sprintAction,crouchAction;
    
    public static PlayerMovement Instance { get; private set; }

    private void Awake()
    {
        if (Instance != this && Instance != null) Destroy(this);
        else Instance = this;
    }
    private void OnEnable()
    {
        InitActions();
    }
    void Start()
    {
        if (capsuleCollider == null || rb == null || playerCamera == null || breathingSource == null || feet == null)
            Debug.LogError(this.GetType().Name + " not all components attached.");

        capsuleCollider.height = defaultHeight;
        feet.localPosition = new Vector3(0,-capsuleCollider.height/2,0);
        currentStamina = staminaLength;
    }


    void Update()
    {
        if(rb.velocity.y < 0)
        {
            rb.velocity += new Vector3(0,Physics.gravity.y * (fallMultiplier-1)*Time.deltaTime,0);
        }


        //SPRINTING 
        if (sprintAction.action.IsPressed() && moving && IsGrounded && !isExhausted)
        {
            OnSprintingHolded();
        }
        else if(!moving && sprinting)
        {
            OnSprintingCanceled();
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
       RemoveActions();
    }
    void Movement()
    {

        if (move.x != 0 || move.z != 0)
        {
            moving = true;
        }
        else
        {
            moving = false;
        }

        if(movementActions != MovementActions.JUMPING)
        {
            if(!moving && isGrounded) rb.Sleep();
        }

     //   if(!moving && isGrounded) rb.Sleep();
        // Debug.Log($"jumping:{!jumping}");
        // Debug.Log($"moving:{!moving}");
        // Debug.Log($"isGrounded:{isGrounded}");

       // if(!moving && !jumping && isGrounded) rb.Sleep();
        moveDirection = playerCamera.orientation.forward * move.z + playerCamera.orientation.right * move.x;

        if(isGrounded && OnSlope())
        {
            rb.AddForce(GetSlopeMoveDirection() * currentSpeedForce,ForceMode.Acceleration);
            if(rb.velocity.y > 0 || rb.velocity.y < 0) rb.AddForce(Vector3.down*80f, ForceMode.Force);
        } 
        else if(isGrounded && !OnSlope()) rb.AddForce(moveDirection* currentSpeedForce, ForceMode.Acceleration);
        else if(!isGrounded && !OnSlope()) rb.AddForce(moveDirection * currentSpeedForce, ForceMode.Acceleration);
    }

    /// <summary> Making sure that speed is lower than currentMoveLimit </summary>
    /// <paramref name="currentMoveLimit" />
    public void SpeedController()
    {
        if(OnSlope())
        {
            if(rb.velocity.magnitude > currentMoveLimit) rb.velocity = rb.velocity.normalized * currentMoveLimit;
        }
        else
        {
            Vector3 currentMoveSpeed = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
            if (currentMoveSpeed.magnitude >= currentMoveLimit)
            {
                Vector3 limitCurrentMoveSpeed = currentMoveSpeed.normalized * currentMoveLimit;
                rb.velocity = new Vector3(limitCurrentMoveSpeed.x, rb.velocity.y, limitCurrentMoveSpeed.z);
            }

        }
    }

    /// <summary> Called when player is moving</summary>
    public void OnMove(InputAction.CallbackContext ctx)
    {
        //Making sure that player is moving with correct speed
        if (movementActions == MovementActions.DEFAULT && currentSpeedForce != baseSpeedForce) SetSpeed();
        //Debug.Log("OnMove started");

        Vector2 value = ctx.ReadValue<Vector2>();
        //Debug.Log(value);
        //                x - vertical, z - horizontal
        move = new Vector3(value.x,0,value.y).normalized;

        move.x *= movingSidesSpeed;
        move.z = move.z > 0 ? move.z *= movingFrontSpeed : move.z < 0 ? move.z *= movingBackSpeed : 0;
    }
    /// <summary> Called when player jump button is clicked</summary>
    public void OnJump(InputAction.CallbackContext ctx)
    {
        if(ctx.started)
        {
            if (isGrounded)
            {
                movementActions = MovementActions.JUMPING;
                jumping = true;
                SetSpeed(baseJumpMultiplier);
                Jump();
            }
        }
    }
    ///<summary> Jump impulse , height is equal to <see cref="jumpHeight"/> </summary>
    private void Jump() => rb.AddForce(new Vector3(0, Mathf.Sqrt(-2.0f * Physics2D.gravity.y * jumpHeight), 0), ForceMode.Impulse);
    
    /// <summary> Called when player sprint button is clicked / canceled  </summary>
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
      //  Debug.Log("OnSprintingStart called!");

        //If player isn't exhausted (exhaustion state is triggered when player is sprinting to 0 stamina) sprint is triggered
        if(movementActions == MovementActions.DEFAULT)
        {
            if (currentStamina > 0.0f && !isExhausted && moving)
            {
                movementActions = MovementActions.SPRINTING;
                SetSpeed(baseSprintMultiplier);
                sprinting = true;
                onSprinting();
            }
        }
    }
    public void OnSprintingCanceled()
    {
    //    Debug.Log("OnSprintingCanceled called!");
        if(movementActions == MovementActions.SPRINTING)
        {
            if (sprinting)
            {
                sprinting = false;
                movementActions = MovementActions.DEFAULT;
                SetSpeed();
            }
        }

    }   
    public void OnSprintingHolded()
    {
      //  Debug.Log($"Sprint is holded and all conditions are meet: moving: {moving},IsGrounded: {IsGrounded},!isExhausted:{!isExhausted}");
        if(!sprinting) OnSprintingStart();

        if(sprinting)
        {
            if (currentStamina > 0.0f) currentStamina -= Time.deltaTime;
            else
            {
                Debug.Log("Exhaustion!");
                Exhaustion(true);
                OnSprintingCanceled();
            }
        }
        

        
    }
    /// <summary> Called when player crouch button is clicked / canceled  </summary>
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
            preCrouchPos = transform.position;
            Debug.Log("Crouch started");
            movementActions = MovementActions.CROUCHING;
            crouch = true;
            SetSpeed(baseCrouchMultiplier);
            if (crouchTransition != null) StopCoroutine(crouchTransition);
            crouchTransition = StartCoroutine(CrouchTransition(crouchHeight, crouchDuration));
        }
    }
    public void OnCrouchCanceled()
    {
        Debug.Log("Crouch canceled");
        crouch = false;
        movementActions = MovementActions.DEFAULT;
        if(capsuleCollider.height != defaultHeight)
        {
            //transform.localPosition = new Vector3(transform.localPosition.x,transform.localPosition.y + capsuleCollider.height/2,transform.localPosition.z);
            capsuleCollider.height = defaultHeight;
            if(transform.position.y != preCrouchPos.y) transform.position = new Vector3(transform.position.x,preCrouchPos.y,transform.position.z);
        }
        feet.localPosition = new Vector3(0,-capsuleCollider.height/2,0);
        SetSpeed();
    }
    /// <summary>Transition between stand and crouch "state"</summary>
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
            Debug.Log($"currValue: {currValue}");
            Debug.Log($"timeElapsed: {timeElapsed}");
            capsuleCollider.height = currValue;
            feet.localPosition = new Vector3(0,-currValue/2,0);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        capsuleCollider.height = desiredValue; 
        feet.localPosition = new Vector3(0,-capsuleCollider.height/2,0);
        
        crouchTransition = null;
        Debug.Log("Crouch transition ended");
    }
 
    ///<summary> Calculate angle of current ground </summary>
    private bool OnSlope()
    {
        if(Physics.Raycast(transform.position,Vector3.down,out slopeHit, capsuleCollider.height * 0.5f + 0.3f,layerMask:groundLayer))
        {
            float angle = Vector3.Angle(Vector3.up,slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }
        return false;
    }
    ///<summary> Get current ground slope</summary>
    private Vector3 GetSlopeMoveDirection()
    {
        return Vector3.ProjectOnPlane(moveDirection,slopeHit.normal).normalized;
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
        //If crouch is pressed ,crouching start will execute
        if(crouch)
        {
            movementActions = MovementActions.CROUCHING;
            SetSpeed(baseCrouchMultiplier);
        }
        else if(jumping)
        {
            movementActions = MovementActions.JUMPING;
            SetSpeed(baseJumpMultiplier);
        }
        //If sprint is pressed ,sprinting start will execute
        else if(sprinting)
        {
            movementActions = MovementActions.SPRINTING;
            SetSpeed(baseSprintMultiplier);
            
        }
        //If any of above conditions isn't fullfiled ,move is going to default state 
        else
        {
            movementActions = MovementActions.DEFAULT;
            SetSpeed();
        }
    }
    ///<summary> Switching between in air and ground drag</summary>
    private void DragControl()
    {
        if(isGrounded) rb.drag = groundDrag;
        else rb.drag = airDrag;
    }
    public void SetSpeed(float value = 1)
    {
        if (value == 0) Debug.LogWarning($"value can't be equal to 0 !");
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
    ///<summary>Initializing functions to input actions</summary>
    public void InitActions()
    {

        moveAction.action.started += OnMove;
        moveAction.action.performed += OnMove;
        moveAction.action.canceled += OnMove;

        sprintAction.action.started += OnSprint;
        sprintAction.action.canceled += OnSprint;

        jumpAction.action.started += OnJump;

        crouchAction.action.started += OnCrouch;
        crouchAction.action.canceled += OnCrouch;
    }
    ///<summary>Removing functions to input actions</summary>
    public void RemoveActions()
    {
        moveAction.action.started -= OnMove;
        moveAction.action.performed -= OnMove;
        moveAction.action.canceled -= OnMove;

        sprintAction.action.started -= OnSprint;
        sprintAction.action.canceled -= OnSprint;

        jumpAction.action.started -= OnJump;

        crouchAction.action.started -= OnCrouch;
        crouchAction.action.canceled -= OnCrouch;
    }
    ///<summary>Activating / Deactivating player action map</summary>
    public void PlayerMapActivate(bool activate)
    {
        if(activate) playerActionMap.Enable();
        else playerActionMap.Disable();
    }
}


