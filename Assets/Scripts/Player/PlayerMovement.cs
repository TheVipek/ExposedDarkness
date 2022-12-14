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
    [Header("Player Settings")]
    public PlayerMoveSettings settings;
    private RaycastHit slopeHit;

    [Header("LayerMask")]
    public LayerMask groundLayer;

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

    
    public InputActionReference moveAction,jumpAction,sprintAction,crouchAction;
    [Header("GameEvents")]
    public GameEvent onSprinting;
    private void OnEnable()
    {
        InitActions();
    }

    void Start()
    {
        if (capsuleCollider == null || rb == null || playerCamera == null || breathingSource == null || feet == null)
            Debug.LogError(this.GetType().Name + " not all components attached.");

        capsuleCollider.height = settings.DefaultHeight;
        feet.localPosition = new Vector3(0,-capsuleCollider.height/2,0);
    }


    void Update()
    {
        if(rb.velocity.y < 0)
        {
            rb.velocity += new Vector3(0,Physics.gravity.y * (settings.FallMultiplier-1)*Time.deltaTime,0);
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
        else if (settings.CurrentStamina < settings.StaminaLength)
        {
            settings.CurrentStamina += Time.deltaTime * settings.StaminaRegenerartionSpeed;
            if (settings.CurrentStamina >= settings.StaminaLength)
            {
                settings.CurrentStamina = settings.StaminaLength;
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
            if(settings.MovementActions == MovementActions.NONE) SetMoveBasedOnState();
        }
        else
        {
            moving = false;
            if(settings.MovementActions == MovementActions.DEFAULT) settings.SetMovementAction(MovementActions.NONE);
        }

        if(settings.MovementActions != MovementActions.JUMPING)
        {
            if(!moving && isGrounded) rb.Sleep();
        }
        moveDirection = playerCamera.orientation.forward * move.z + playerCamera.orientation.right * move.x;

        if(isGrounded && OnSlope())
        {
            rb.AddForce(GetSlopeMoveDirection() * settings.CurrentSpeedForce,ForceMode.Acceleration);
            if(rb.velocity.y > 0 || rb.velocity.y < 0) rb.AddForce(Vector3.down*80f, ForceMode.Force);
        } 
        else if(isGrounded && !OnSlope()) rb.AddForce(moveDirection* settings.CurrentSpeedForce, ForceMode.Acceleration);
        else if(!isGrounded && !OnSlope()) rb.AddForce(moveDirection * settings.CurrentSpeedForce, ForceMode.Acceleration);
    }

    /// <summary> Making sure that speed is lower than currentMoveLimit </summary>
    /// <paramref name="currentMoveLimit" />
    public void SpeedController()
    {
        if(OnSlope())
        {
            if(rb.velocity.magnitude > settings.CurrentMoveLimit) rb.velocity = rb.velocity.normalized * settings.CurrentMoveLimit;
        }
        else
        {
            Vector3 currentMoveSpeed = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
            if (currentMoveSpeed.magnitude >= settings.CurrentMoveLimit)
            {
                Vector3 limitCurrentMoveSpeed = currentMoveSpeed.normalized * settings.CurrentMoveLimit;
                rb.velocity = new Vector3(limitCurrentMoveSpeed.x, rb.velocity.y, limitCurrentMoveSpeed.z);
            }

        }
    }

    /// <summary> Called when player is moving</summary>
    public void OnMove(InputAction.CallbackContext ctx)
    {
        //Making sure that player is moving with correct speed
        if (settings.MovementActions == MovementActions.DEFAULT && settings.CurrentSpeedForce != settings.BaseSpeedForce) SetSpeed();
        //Debug.Log("OnMove started");

        Vector2 value = ctx.ReadValue<Vector2>();
        //Debug.Log(value);
        //                x - vertical, z - horizontal
        move = new Vector3(value.x,0,value.y).normalized;

        move.x *= settings.MovingSidesSpeed;
        move.z = move.z > 0 ? move.z *= settings.MovingFrontSpeed : move.z < 0 ? move.z *= settings.MovingBackSpeed : 0;
    }
    /// <summary> Called when player jump button is clicked</summary>
    public void OnJump(InputAction.CallbackContext ctx)
    {
        if(ctx.started)
        {
            if (isGrounded)
            {
                settings.SetMovementAction(MovementActions.JUMPING);

                jumping = true;
                SetSpeed(settings.BaseJumpMultiplier);
                Jump();
            }
        }
    }
    ///<summary> Jump impulse , height is equal to <see cref="jumpHeight"/> </summary>
    private void Jump() => rb.AddForce(new Vector3(0, Mathf.Sqrt(-2.0f * Physics2D.gravity.y * settings.JumpHeight), 0), ForceMode.Impulse);
    
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
        if(settings.MovementActions == MovementActions.DEFAULT)
        {
            if (settings.CurrentStamina > 0.0f && !isExhausted && moving)
            {
                settings.SetMovementAction(MovementActions.SPRINTING);
                SetSpeed(settings.BaseSprintMultiplier);
                sprinting = true;
                
                onSprinting.Raise();             
            }
        }
    }
    public void OnSprintingCanceled()
    {
    //    Debug.Log("OnSprintingCanceled called!");
        if(settings.MovementActions == MovementActions.SPRINTING)
        {
            if (sprinting)
            {
                sprinting = false;
                settings.SetMovementAction(MovementActions.DEFAULT);
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
            if (settings.CurrentStamina > 0.0f) settings.CurrentStamina -= Time.deltaTime;
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
        if (settings.MovementActions != MovementActions.JUMPING)
        {
            preCrouchPos = transform.position;
            Debug.Log("Crouch started");
            settings.SetMovementAction(MovementActions.CROUCHING);
            crouch = true;
            SetSpeed(settings.BaseCrouchMultiplier);
            if (crouchTransition != null) StopCoroutine(crouchTransition);
            crouchTransition = StartCoroutine(CrouchTransition(settings.CrouchHeight, settings.CrouchDuration));

        }
    }
    public void OnCrouchCanceled()
    {
        Debug.Log("Crouch canceled");
        crouch = false;
        settings.SetMovementAction(MovementActions.DEFAULT);
        if(capsuleCollider.height != settings.DefaultHeight)
        {
            //transform.localPosition = new Vector3(transform.localPosition.x,transform.localPosition.y + capsuleCollider.height/2,transform.localPosition.z);
            capsuleCollider.height = settings.DefaultHeight;
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
            return angle < settings.MaxSlopeAngle && angle != 0;
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
        settings.CurrentStamina = settings.StaminaLength;
        onSprinting.Raise();
    }


    ///<summary>Set value of speed - default is 1 which is equal to normal walk.Value can't be equal 0.</summary>
    public void SetMoveBasedOnState()
    {
        //If crouch is pressed ,crouching start will execute
        if(crouch)
        {
            settings.SetMovementAction(MovementActions.CROUCHING);
            SetSpeed(settings.BaseCrouchMultiplier);
        }
        else if(jumping)
        {
            settings.SetMovementAction(MovementActions.JUMPING);
            SetSpeed(settings.BaseJumpMultiplier);
        }
        //If sprint is pressed ,sprinting start will execute
        else if(sprinting)
        {
            settings.SetMovementAction(MovementActions.SPRINTING);
            SetSpeed(settings.BaseSprintMultiplier);
            
        }
        //If any of above conditions isn't fullfiled ,move is going to default state 
        else
        {
            settings.SetMovementAction(MovementActions.DEFAULT);
            SetSpeed();
        }
    }
    ///<summary> Switching between in air and ground drag</summary>
    private void DragControl()
    {
        if(isGrounded) rb.drag = settings.GroundDrag;
        else rb.drag = settings.AirDrag;
    }
    public void SetSpeed(float value = 1)
    {
        if (value == 0) Debug.LogWarning($"value can't be equal to 0 !");
        else
        {
            settings.CurrentSpeedForce = settings.BaseSpeedForce * value;
            settings.CurrentMoveLimit = settings.BaseMoveSpeed * value;
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

}


