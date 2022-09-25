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
    [SerializeField] float zoomTime = 2f;
    [SerializeField] float weaponZoomTime = 2f;
    [Header("Booleans")]
    bool tryingZooming;
    bool zoomed;
    public bool isZoomed{get{return zoomed;}}
    Animator animator;
    private void OnEnable() {
        tryingZooming = false;
        zoomed = false;
    }
    private void Awake() {
        animator = GetComponent<Animator>();
        //weaponZoomOutPosition = transform.localPosition;
    }
    void Update() {
        ZoomWeapon();
    }
    private void FixedUpdate() {
        
    }
    void OnDisable() {
        InstaZoomOut();
    }
    public void ZoomWeapon()
    {
        //if theres no crosshair set in inspector then it wont work
        //if(crosshair == null) return;
        //Purpose of this line is that when player change weapon he needs to press again input(1) to zoom 
        if(Input.GetMouseButtonDown(1))
        {
           tryingZooming = true;
        }
        if(Input.GetMouseButton(1) && tryingZooming == true)
        {
           ZoomIn();
        }
        else if(Input.GetMouseButtonUp(1))
        {
            ZoomOut();
        }
    }


    
    private void ZoomOut()
    { 

        //no longer zooming
        tryingZooming = false;
        StartCoroutine(weaponToZoom(weaponZoomOutPosition,weaponZoomTime,false));
        StartCoroutine(cameraToZoom(zoomOutField,zoomTime));
        playerController.xSensitivity = zoomOutSensitivity;
        playerController.ySensitivity = zoomOutSensitivity;
      //  crosshair.SetActive(false);
    }
    private void InstaZoomOut()
    {
        if(animator.enabled == false) animator.enabled = true;
        tryingZooming = false;
       // if(crosshair == null) return;
        transform.localPosition = weaponZoomOutPosition;
        weaponCamera.fieldOfView = zoomOutField;
        viewCamera.fieldOfView = zoomOutField;

        playerController.xSensitivity = zoomOutSensitivity;
        playerController.ySensitivity = zoomOutSensitivity;
       // crosshair.SetActive(false);
    }
    private void ZoomIn()
    {
        GetComponent<Animator>().enabled = false;
        
        transform.localPosition = weaponZoomOutPosition;
        transform.localRotation = Quaternion.Euler(0,0,0);
        StartCoroutine(weaponToZoom(weaponZoomInPosition,weaponZoomTime,true));
        StartCoroutine(cameraToZoom(zoomInField,zoomTime));
        playerController.xSensitivity = zoomInSensitivity;
        playerController.ySensitivity = zoomInSensitivity;
      //  crosshair.SetActive(true);
    }
    IEnumerator weaponToZoom(Vector3 desiredPosition,float zoomOverTime,bool isZoomingIn)
    {
        if(isZoomingIn == true)
        {
            zoomed = true;

        }
        bool _zooming = tryingZooming;
        float durationZoom = zoomOverTime;
        float timeElapsed = 0f;
        while(timeElapsed <= durationZoom && _zooming == tryingZooming)
        {
            float t = timeElapsed/durationZoom;
           // t = t*t * (3f - 2f*t);
            transform.localPosition = Vector3.Lerp(transform.localPosition,desiredPosition,t);
            timeElapsed+=Time.deltaTime;
            yield return null;
        }

        if(isZoomingIn == false)
        {
            zoomed = false;
            animator.enabled = true;
            Debug.Log("Starting play animation!");
        }

        if(timeElapsed <= durationZoom)
        {
            yield break;
        }

        transform.localPosition = desiredPosition;

        yield return null;
    }
    void swapTryingZooming()
    {
        zoomed = !zoomed;
    }
    IEnumerator cameraToZoom(float desiredFOV,float zoomOverTime)
    {
        bool _zooming = tryingZooming;
        float durationZoom = zoomOverTime;
        float timeElapsed = 0f;
        while(timeElapsed <= durationZoom && _zooming == tryingZooming)
        {
            float t = timeElapsed/durationZoom;
            t = t*t * (3f - 2f*t);
            weaponCamera.fieldOfView = Mathf.Lerp(weaponCamera.fieldOfView,desiredFOV,t);
            viewCamera.fieldOfView = weaponCamera.fieldOfView;
            timeElapsed+=Time.deltaTime;
            yield return null;
        }

        if(timeElapsed <= durationZoom)
        {
            yield break;
        }

        weaponCamera.fieldOfView=desiredFOV;
        viewCamera.fieldOfView = desiredFOV;

        yield return null;
    }
}
