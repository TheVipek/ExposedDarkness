using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;
public class WeaponZoom : MonoBehaviour
{

    [Header("Cameras")]
    [SerializeField] Camera weaponCamera;
    [SerializeField] Camera viewCamera;
    
    [Tooltip("Higher value equals weaker camera zoom")]
    private CamerasController camerasController;
    private Vector3 defaultWeaponPosition;


    [Header("Zoom-IN options")]
    [Tooltip("Lower value equals stronger camera zoom")]
    [SerializeField] float zoomInField =20f;
    [Tooltip("Lower value equals lower mouse sensitivity during zoom")]
    [SerializeField] Vector2 zoomInSensitivity = new Vector2(0.6f,0.6f);
    [SerializeField] Vector3 weaponZoomInPosition;
    private Vector3 startZoomPos;
    
    [Header("Options for IN and OUT")]
    [SerializeField] float cameraZoomDuration = 1f;
    [SerializeField] float weaponZoomDuration = 1f;
    [Header("Booleans")]
    private bool canZoom = true;
    public bool CanZoom{get{return canZoom;} set{canZoom = value;}}
    private bool isTryingToZoom;
    private bool isZoomed;
    public bool IsZoomed{get{return isZoomed;}}
    Animator animator;
    [SerializeField] InputActionReference zoomAction;

    private void Awake() 
    {
        animator = GetComponent<Animator>();
        
        defaultWeaponPosition = GetComponent<WeaponAnimation>().defaultWeaponPosition;
    }
    private void OnEnable() {
        zoomAction.action.started += OnZoom;
     //   zoomAction.action.performed += OnZoom;
        zoomAction.action.canceled += OnZoom;
        isTryingToZoom = false;
        isZoomed = false;
    }
    private void Start() {
        resetOnChangeWeapon();
        camerasController = CamerasController.Instance;
        viewCamera = camerasController.playerCamera;
        weaponCamera = camerasController.weaponCamera;
        
    }

    void Update() 
    {
       //Purpose of this line is that when player change weapon he needs to press again input(1) to zoom 
        if(zoomAction.action.IsPressed() && isTryingToZoom == true && isZoomed == false)
        {
            ZoomIn();
        }
    }
    void OnDisable() {
        resetOnChangeWeapon();
        zoomAction.action.started -= OnZoom;
       // zoomAction.action.performed -= OnZoom;
        zoomAction.action.canceled -= OnZoom;
    }
    public void OnZoom(InputAction.CallbackContext ctx)
    {
        if(ctx.started)
        {
//            Debug.Log("weaponZooming started");
            isTryingToZoom = true;
        }
        if(ctx.canceled)
        {
     //       Debug.Log("weaponZooming canceled");

            ZoomOut();
        }
    }
    private void ZoomOut()
    { 
        startZoomPos = transform.localPosition;
        
        
        isTryingToZoom = false;
        StartCoroutine(weaponToZoom(defaultWeaponPosition,weaponZoomDuration,false));
        StartCoroutine(cameraToZoom(camerasController.DefaultFov,cameraZoomDuration));
        PlayerCamera.mouse.DefaultSensitivity();
    }
    private void resetOnChangeWeapon()
    {
        gameObject.transform.localRotation = Quaternion.Euler(0,0,0);
        transform.localPosition = defaultWeaponPosition;
        CamerasController.Instance.setDefaultFov();
        PlayerCamera.mouse.DefaultSensitivity();

    }
    private void ZoomIn()
    {
        //Rebind so when next time animator is enabled animation is going from first frame, not from last ended 
        startZoomPos = transform.localPosition;
        //animator.Update(0f);
        animator.Rebind();
        animator.enabled = false;

        
        StartCoroutine(weaponToZoom(weaponZoomInPosition,weaponZoomDuration,true));
        StartCoroutine(cameraToZoom(zoomInField,cameraZoomDuration));
        PlayerCamera.mouse.SensitivityChange(zoomInSensitivity);

    }

    // weaponToZoom 
    //First: takes care of detection whether player is trying to zoom (tryingZooming , which is changed to true when player clicks and hold mouse button 
    //and to false when player stop holding mouse button)
    //Second: There's quick transition 
    //Third : After transition
    //  - if it was zooming out zoomed is being set to false, animator is enabled 
    //  - if timeElapsed is lower than zoomOverTime (was interrupted by something) we're breaking out of coroutine
    //Fourth: setting localPos to desiredPos
    IEnumerator weaponToZoom(Vector3 desiredPosition,float zoomOverTime,bool isZoomingIn)
    {
        transform.localRotation = Quaternion.Euler(0,0,0);
        bool _zooming = isTryingToZoom;
        //startZoomPos = transform.localPosition;
     //   Debug.Log($"StartPos:{startZoomPos}");
      //  Debug.Log($"DesiredPos:{desiredPosition}");
        float timeElapsed = 0f;
        if(isZoomingIn == true) isZoomed = true;
        while(timeElapsed<= zoomOverTime && _zooming == isTryingToZoom)
        {
            float t = timeElapsed / zoomOverTime;
            transform.localPosition = Vector3.Lerp(startZoomPos,desiredPosition,t);
//            Debug.Log($"Value:{Vector3.Lerp(startZoomPos,desiredPosition,t)}");
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        if(isZoomingIn == false)
        {
            isZoomed = false;
            animator.enabled = true;
       //     Debug.Log("Starting play animation!");
        }

        if(timeElapsed < zoomOverTime)
        {
//            Debug.Log("timeElapsed <= zoomOverTime");
            yield break;
        }
        transform.localPosition = desiredPosition;
        yield return null;
    }

    //Check weaponToZoom , basically the same stuff without few features
    IEnumerator cameraToZoom(float desiredFOV,float zoomOverTime)
    {
        bool _zooming = isTryingToZoom;
        float timeElapsed = 0f;
        while(timeElapsed <= zoomOverTime && _zooming == isTryingToZoom)
        {
            float t = timeElapsed/zoomOverTime;
            t = t*t * (3f - 2f*t);
            weaponCamera.fieldOfView = Mathf.Lerp(weaponCamera.fieldOfView,desiredFOV,t);
            viewCamera.fieldOfView = weaponCamera.fieldOfView;
            timeElapsed+=Time.deltaTime;
            yield return null;
        }

        if(timeElapsed <= zoomOverTime)
        {
            yield break;
        }

        weaponCamera.fieldOfView = desiredFOV;
        viewCamera.fieldOfView = desiredFOV;

        yield return null;
    }
}
