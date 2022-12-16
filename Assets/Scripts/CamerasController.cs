using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamerasController : MonoBehaviour
{
    public static CamerasController Instance{get; private set;}
    public Camera[] cameras;
    public Camera weaponCamera;
    public Camera playerCamera;
    [SerializeField] float defaultFov = 80f;
    public float DefaultFov{ get {return defaultFov;}}
    
    private void Awake() {
        if(Instance != this && Instance != null) Destroy(this);
        else Instance = this;
    }
    public void setDefaultFov()
    {
        for(int i=0;i<cameras.Length;i++) cameras[i].fieldOfView = defaultFov;
    }

}
