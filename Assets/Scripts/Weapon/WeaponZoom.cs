using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponZoom : MonoBehaviour
{
    [SerializeField] Camera _camera;
    [SerializeField] PlayerMovement playerController;
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
    Vector3 weaponZoomOutPosition;
    
    [Header("Options for IN and OUT")]
    [SerializeField] float zoomTime = 2f;
    [SerializeField] float weaponZoomTime = 2f;
    private Vector3 weaponVelocity = Vector3.zero;
    
    bool zoomedIn = false;
    bool zooming = false;
    
    bool cameraZoomed=false;
    bool weaponZoomed=false;

    void OnDisable() {
        InstaZoomOut();
    }
    private void Start() {
        weaponZoomOutPosition = transform.localPosition;
    }
    void Update() {
        ZoomWeapon();
    }
    public void ZoomWeapon()
    {
        
        if(crosshair == null) return;
        //Purpose of this line is that when player change weapon he needs to press again input(1) to zoom 
        if(Input.GetMouseButtonDown(1))
        {
           zoomedIn = false;
        }
        if(Input.GetMouseButton(1) && zoomedIn == false)
        {
           ZoomIn();

        }
        else if(Input.GetMouseButtonUp(1))
        {
            ZoomOut();
            zoomedIn = true;

        }
    }

    private void ZoomOut()
    { 
        zooming = false;
        StartCoroutine(weaponToZoom(weaponZoomOutPosition,weaponZoomTime));
        StartCoroutine(cameraToZoom(zoomOutField,zoomTime));
        playerController.xSensitivity = zoomOutSensitivity;
        playerController.ySensitivity = zoomOutSensitivity;
        crosshair.SetActive(false);
    }
    private void InstaZoomOut()
    {
        if(crosshair == null) return;
        zoomedIn = true;
        transform.localPosition = weaponZoomOutPosition;
        _camera.fieldOfView = zoomOutField;
        playerController.xSensitivity = zoomOutSensitivity;
        playerController.ySensitivity = zoomOutSensitivity;
        crosshair.SetActive(false);
    }
    private void ZoomIn()
    {
        zooming=true;
        StartCoroutine(weaponToZoom(weaponZoomInPosition,weaponZoomTime));
        StartCoroutine(cameraToZoom(zoomInField,zoomTime));
        playerController.xSensitivity = zoomInSensitivity;
        playerController.ySensitivity = zoomInSensitivity;
        crosshair.SetActive(true);
    }
    IEnumerator weaponToZoom(Vector3 desiredPosition,float smoothTime)
    {
        bool _zooming = zooming;
        while(transform.localPosition != desiredPosition && zooming == _zooming)
        {
            Debug.Log("weapon zooming!");
            transform.localPosition = Vector3.SmoothDamp(transform.localPosition,desiredPosition,ref weaponVelocity,smoothTime);
            yield return null;
        }

        if(zooming != _zooming)
        {
            yield break;
        }
        transform.localPosition = desiredPosition;
        yield return null;
    }
    IEnumerator cameraToZoom(float desiredFOV,float zoomOverTime)
    {
        bool _zooming = zooming;
        float durationZoom = zoomOverTime;
        float timeElapsed = 0f;
        while(timeElapsed <= durationZoom && zooming == _zooming)
        {
            _camera.fieldOfView = Mathf.Lerp(_camera.fieldOfView,desiredFOV,timeElapsed/durationZoom);
            timeElapsed+=Time.deltaTime;
            yield return null;
        }

        if(zooming != _zooming)
        {
            yield break;
        }
        _camera.fieldOfView=desiredFOV;
        yield return null;
    }
}
