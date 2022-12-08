using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;
[Serializable]
public class MouseLook
{
    
    private Vector2 move;
    private Vector2 defaultSensitivity = new Vector2(1,1);
    private Vector2 sensitivity;
    public Vector2 Sensitivity{get{return sensitivity;}}

    /// <summary> MouseLook constructor </summary>
    public MouseLook(Vector2 _sensitivity) => sensitivity = _sensitivity / 10;

    /// <summary> Return mouse move </summary>
    public Vector2 getMove() => move;
    public Vector2 setMove(Vector2 _move) => move = _move;

    /// <summary> Change mouse sensitivity </summary>
    public void SensitivityChange(Vector2 _sensitivity)
    {
        sensitivity.x = _sensitivity.x / 10;
        sensitivity.y = _sensitivity.y / 10;
    }
    ///<summary> Change mouse sensitivity to default value </summary>
    public void DefaultSensitivity() => sensitivity = defaultSensitivity / 10;
}

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] Transform cameraView;
    public Transform orientation;
    [SerializeField] Vector2 mouseSensitivity;
    [SerializeField] float maxCameraXRotation;
    public static MouseLook mouse;
    private Vector2 cameraRotation;
    [SerializeField] InputActionReference lookAction;

    private void OnEnable() {
        lookAction.action.started += OnLook;
        lookAction.action.performed += OnLook;


    }
    private void OnDisable() {
        lookAction.action.started -= OnLook;
        lookAction.action.performed -= OnLook;

        
    }
    private void Awake() {
        Cursor.lockState = CursorLockMode.Locked;
        mouse = new MouseLook(mouseSensitivity);
    }
    private void LateUpdate() {
        CameraLooking();
    }
    public void CameraLooking()
    {
        //Increase by move made in this frame
        Vector2 currentMove = mouse.getMove();
        cameraRotation.x += currentMove.x;
        cameraRotation.y -= currentMove.y;

        cameraRotation.y = Mathf.Clamp(cameraRotation.y,-maxCameraXRotation,maxCameraXRotation);
        cameraView.transform.rotation = Quaternion.Euler(cameraRotation.y,cameraRotation.x,0);
        orientation.transform.rotation = Quaternion.Euler(0,cameraRotation.x,0);
    }

    public void OnLook(InputAction.CallbackContext ctx)
    {
        Vector2 move = ctx.ReadValue<Vector2>() * mouse.Sensitivity;
        mouse.setMove(move);
    }









    // Camera mCamera;
    // public static PlayerCamera instance;
    // [SerializeField] float shakeDuration = .5f;
    // [SerializeField] float shakePower = 1f;
    // [SerializeField] Animator animator;
    // public void TriggerShake()
    // {
    //     animator.SetTrigger("Shake");
    // }

    // private void Awake() {
    //     if(instance!=this && instance != null)
    //     {
    //         Destroy(this);
    //     }else{
    //         instance = this;
    //     }
    //     mCamera = GetComponent<Camera>();
    // }

           
    // public IEnumerator shakeCamera()
    // {
    //     Vector3 originalPos = new Vector3(transform.localPosition.x,transform.localPosition.y,transform.localPosition.z);
    //     Debug.Log(originalPos);
    //     float timeElapsed = shakeDuration;
    //     while(timeElapsed > 0)
    //     {
    //         float x = Random.Range(-1f,1f) * shakePower;
    //         float y = Random.Range(-1f,1f) * shakePower;
    //         transform.localPosition = Vector3.Lerp(originalPos,new Vector3(originalPos.x+x,originalPos.y+y,originalPos.z),.1f);
    //         timeElapsed-=Time.deltaTime;
    //         yield return null;
    //     }
    //     transform.localPosition = originalPos;
    //     Debug.Log(transform.localPosition);

        
    // }
}
