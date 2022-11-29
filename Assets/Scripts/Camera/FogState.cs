using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
public class FogState : MonoBehaviour
{
    [SerializeField] bool fog;
    public Camera cameraWithoutFog;
    private void OnEnable() {
        RenderPipelineManager.beginCameraRendering += OnBeginCameraRendering;
        
    }
    private void OnDisable() {
        RenderPipelineManager.beginCameraRendering -= OnBeginCameraRendering;
        
    }
    private void Start() {
    }
    private void OnDestroy() {
        
    }
    void OnBeginCameraRendering(ScriptableRenderContext context,Camera camera)
    {
        if(camera == cameraWithoutFog)
        {
            Debug.Log(camera.name +" disabling fog");
            RenderSettings.fog = false;
        }
        else
        {
            RenderSettings.fog = true;
        }
    }
}
