using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponZoom : MonoBehaviour
{

    [Header("Cameras")]
    [SerializeField] Camera weaponCamera;
    [SerializeField] Camera viewCamera;

    [Header("Components")]
    [SerializeField] PlayerMovement playerController;

    
    [Header("Default options")]
    [Tooltip("Higher value equals weaker camera zoom")]
    private CamerasController camerasController;
    public Vector3 defaultWeaponPosition;


    [Header("Zoom-IN options")]
    [Tooltip("Lower value equals stronger camera zoom")]
    [SerializeField] float zoomInField =20f;
    [Tooltip("Lower value equals lower mouse sensitivity during zoom")]
    [SerializeField] Vector2 zoomInSensitivity = new Vector2(0.6f,0.6f);
    [SerializeField] Vector3 weaponZoomInPosition;
    
    
    [Header("Options for IN and OUT")]
    [SerializeField] float cameraZoomDuration = 1f;
    [SerializeField] float weaponZoomDuration = 1f;
    [Header("Booleans")]
    private bool isTryingToZoom;
    private bool isZoomed;
    public bool IsZoomed{get{return isZoomed;}}
    Animator animator;
    private void Awake() 
    {
        animator = GetComponent<Animator>();
        camerasController = CamerasController.Instance;
    }
    private void OnEnable() {
        
        isTryingToZoom = false;
        isZoomed = false;
    }
    void Update() 
    {
       //Purpose of this line is that when player change weapon he needs to press again input(1) to zoom 
        if(Input.GetMouseButtonDown(1))
        {
            isTryingToZoom = true;
        }
        if(Input.GetMouseButton(1) && isTryingToZoom == true && isZoomed == false)
        {
            ZoomIn();
        }
        else if(Input.GetMouseButtonUp(1))
        {
            isTryingToZoom = false;
            ZoomOut();
        }
    }
    void OnDisable() {
        resetOnChangeWeapon();
    }
 
    private void ZoomOut()
    { 
    
        StartCoroutine(weaponToZoom(defaultWeaponPosition,weaponZoomDuration,false));
        StartCoroutine(cameraToZoom(camerasController.DefaultFov,cameraZoomDuration));
        playerController.setDefaultMouseValues();
    }
    private void resetOnChangeWeapon()
    {
        
        transform.localPosition = defaultWeaponPosition;
        CamerasController.Instance.setDefaultFov();
        playerController.setDefaultMouseValues();

    }
    private void ZoomIn()
    {
        //Rebind so when next time animator is enabled animation is going from first frame, not from last ended 
        animator.Rebind();
        animator.enabled = false;


        StartCoroutine(weaponToZoom(weaponZoomInPosition,weaponZoomDuration,true));
        StartCoroutine(cameraToZoom(zoomInField,cameraZoomDuration));
        playerController.setCustomMouseValues(zoomInSensitivity.x,zoomInSensitivity.y);

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
        bool _zooming = isTryingToZoom;
        Vector3 startPos = transform.localPosition;
        float timeElapsed = 0f;
        if(isZoomingIn == true) isZoomed = true;
        //zoomed = isZoomingIn;

        while(timeElapsed<= zoomOverTime && _zooming == isTryingToZoom)
        {
            float t = timeElapsed / zoomOverTime; 
            transform.localPosition = Vector3.Lerp(startPos,desiredPosition,t);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        if(isZoomingIn == false)
        {
            isZoomed = false;
            animator.enabled = true;
            Debug.Log("Starting play animation!");
        }

        if(timeElapsed < zoomOverTime)
        {
            Debug.Log("timeElapsed <= zoomOverTime");
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
