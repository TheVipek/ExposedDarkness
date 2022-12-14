using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PlayerMoveSettings : ScriptableObject
{
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
    [Tooltip("How long is crouch transition")]
    [SerializeField] float crouchDuration;
    [Header("Jump and fall")]
    [Tooltip("baseSpeedForce and baseMoveSpeed is multiplied by this value")]
    [SerializeField] float baseJumpMultiplier;
    [Tooltip("How high player can jump")]
    [SerializeField] float jumpHeight;
    [SerializeField] float fallMultiplier;
    [Header("Stamina")]
    [SerializeField] float staminaRegenerartionSpeed;
    [SerializeField] float staminaLength;
    [SerializeField] float currentStamina;
     [Header("Drag")]
    [SerializeField] float groundDrag;
    [SerializeField] float airDrag;

    [Header("Slope")]
    [SerializeField] float maxSlopeAngle;

    [Header("Collider Height")]
    [SerializeField] float defaultHeight;
    [SerializeField] float crouchHeight;

    [SerializeField] MovementActions movementActions;
    public GameEvent onMovementActionChange;
    
    public MovementActions MovementActions{get{return movementActions;}set{movementActions = value;}}
    // Dynamically Changing
    private float currentSpeedForce;
    public float CurrentSpeedForce{get{return currentSpeedForce;} set{currentSpeedForce = value;}}
    private float currentMoveLimit;
    public float CurrentMoveLimit{get{return currentMoveLimit;} set{currentMoveLimit = value;}}
    public float MovingFrontSpeed{get{return movingFrontSpeed;}}
    public float MovingBackSpeed{get{return movingBackSpeed;}}
    public float MovingSidesSpeed{get{return movingSidesSpeed;}}
    public float BaseSpeedForce{get{return baseSpeedForce;}}
    public float BaseMoveSpeed{get{return baseMoveSpeed;}}
    public float BaseSprintMultiplier{get{return baseSprintMultiplier;}}
    public float BaseCrouchMultiplier{get{return baseCrouchMultiplier;}}
    public float CrouchHeight{get{return crouchHeight;}}
    public float CrouchDuration{get{return crouchDuration;}}
    public float BaseJumpMultiplier{get{return baseJumpMultiplier;}}
    public float JumpHeight{get{return jumpHeight;}}
    public float FallMultiplier{get{return fallMultiplier;}}
    public float StaminaRegenerartionSpeed{get{return staminaRegenerartionSpeed;}}
    public float StaminaLength{get{return staminaLength;}}
    public float CurrentStamina{get{return currentStamina;} set{currentStamina = value;}}
    public float GroundDrag{get{return groundDrag;}}
    public float AirDrag{get{return airDrag;}}
    public float MaxSlopeAngle{get{return maxSlopeAngle;}}
    public float DefaultHeight{get{return defaultHeight;}}
    private void OnEnable() {
        InitStats();
    }
    public void InitStats()
    {
        currentStamina = staminaLength;
        currentMoveLimit = baseMoveSpeed;
        currentSpeedForce = baseSpeedForce;
    }
    public void SetMovementAction(MovementActions movementAction)
    {
//        Debug.Log($"From {movementActions} to {movementAction}");
        movementActions = movementAction;
//        Debug.Log("Raising event");
        onMovementActionChange.Raise();

    }
}
