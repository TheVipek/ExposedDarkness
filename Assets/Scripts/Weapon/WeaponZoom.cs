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
    [Header("Needed objects")]
    [SerializeField] GameObject crosshair;
    
    [Header("Zoom-IN options")]
    [Tooltip("Lower value equals stronger camera zoom")]
    [SerializeField] float zoomInField =20f;
    [SerializeField] float zoomInSensitivity =0.6f;
    [SerializeField] Vector3 weaponZoomInPosition;
    
    [Header("Zoom-OUT options")]
    [Tooltip("Higher value equals weaker camera zoom")]
    [SerializeField] float zoomOutField = 60f;
    [SerializeField] float zoomOutSensitivity = 2f;
    public Vector3 weaponZoomOutPosition;
    
    [Header("Options for IN and OUT")]
    [SerializeField] float zoomTime = 1f;
    [SerializeField] float weaponZoomTime = 1f;
    [Header("Booleans")]
    bool tryingZooming;
    bool zoomed;
    public bool isZoomed{get{return zoomed;}}
    Animator animator;
    private void OnEnable() {
        if(animator.enabled == false) animator.enabled = true;
        tryingZooming = false;
        zoomed = false;
    }
    private void Awake() {
        animator = GetComponent<Animator>();
    }
    void Update() {
        ZoomWeapon();
    }
    void OnDisable() {
        resetOnChangeWeapon();
    }
    public void ZoomWeapon()
    {
        //Purpose of this line is that when player change weapon he needs to press again input(1) to zoom 
        if(Input.GetMouseButtonDown(1))
        {
           tryingZooming = true;
        }
        if(Input.GetMouseButton(1) && tryingZooming == true && zoomed == false)
        {
           ZoomIn();
        }
        else if(Input.GetMouseButtonUp(1))
        {
            tryingZooming = false;
            ZoomOut();
        }
    }


    
    private void ZoomOut()
    { 
    
        StartCoroutine(weaponToZoom(weaponZoomOutPosition,weaponZoomTime,false));
        StartCoroutine(cameraToZoom(zoomOutField,zoomTime));
        playerController.setMouseValues(zoomOutSensitivity,zoomOutSensitivity);
    }
    private void resetOnChangeWeapon()
    {
        
        transform.localPosition = weaponZoomOutPosition;
        weaponCamera.fieldOfView = zoomOutField;
        viewCamera.fieldOfView = zoomOutField;
        playerController.setMouseValues(zoomOutSensitivity,zoomOutSensitivity);

    }
    private void ZoomIn()
    {
        //Rebind so when next time animator is enabled animation is going from first frame, not from last ended frame
        animator.Rebind();
        animator.enabled = false;


        StartCoroutine(weaponToZoom(weaponZoomInPosition,weaponZoomTime,true));
        StartCoroutine(cameraToZoom(zoomInField,zoomTime));
        playerController.setMouseValues(zoomInSensitivity,zoomInSensitivity);

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
        bool _zooming = tryingZooming;
        Vector3 startPos = transform.localPosition;
        float timeElapsed = 0f;
        if(isZoomingIn == true) zoomed = true;
        //zoomed = isZoomingIn;

        while(timeElapsed<= zoomOverTime && _zooming == tryingZooming)
        {
            float t = timeElapsed / zoomOverTime; 
            transform.localPosition = Vector3.Lerp(startPos,desiredPosition,t);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        if(isZoomingIn == false)
        {
            zoomed = false;
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
        bool _zooming = tryingZooming;
        float timeElapsed = 0f;
        while(timeElapsed <= zoomOverTime && _zooming == tryingZooming)
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

        weaponCamera.fieldOfView=desiredFOV;
        viewCamera.fieldOfView = desiredFOV;

        yield return null;
    }
}
