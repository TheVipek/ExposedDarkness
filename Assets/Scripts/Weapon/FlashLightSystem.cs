using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public enum FlashlightState
{
    
    OFF,
    ON,
    DEFAULT
}
public class FlashLightSystem : MonoBehaviour
{
    [SerializeField] MeshRenderer flashlightMesh;
    [SerializeField] Light Mylight;
    public bool needActivateBySelf = false;
    FlashlightState flashlightActivated = FlashlightState.ON;
    [SerializeField] WeaponZoom weapon;
    [SerializeField] DefaultSoundKit flashlightSound;
    [SerializeField] AudioSource audioSource;
    [SerializeField] InputActionReference flashlightAction,zoomAction;
    private void OnEnable() {
        flashlightMesh.enabled = true;
        needActivateBySelf = false;
        zoomAction.action.performed += OnZoom;
        zoomAction.action.canceled += OnZoom;
        flashlightAction.action.started += OnFlashLight;
    }
    private void OnDisable() {
        zoomAction.action.performed -= OnZoom;
        zoomAction.action.canceled -= OnZoom;
        flashlightAction.action.started -= OnFlashLight;
    }
    public void OnFlashLight(InputAction.CallbackContext ctx)
    {
        if(ctx.started)
        {
            needActivateBySelf = false;
            SwapFlashLighter();
        }
    }
    public void OnZoom(InputAction.CallbackContext ctx)
    {
        if(ctx.performed)
        {
            DeactivateFlashLight();
        }
        else if(ctx.canceled)
        {
            ActivateFlashLight();
        }
    }
    private void ActivateFlashLight()
    {
        flashlightMesh.enabled = true;
        SwapFlashLighter(1);
    }
    private void DeactivateFlashLight()
    {
        flashlightMesh.enabled = false;
        SwapFlashLighter(0);
    }
    private void SwapFlashLighter(int value = 2)
    {

            AudioManager.playSound(audioSource,flashlightSound.Sound);
            //value 2 in enum is equal to DEFAULT which means if function is called with default parameter
            //flaslight swpas - from OFF to ON / ON to OFF
            if(value == 2)
            {
                if(flashlightActivated == 0)
                {
                    flashlightActivated = FlashlightState.ON;
                }else
                {
                    flashlightActivated = FlashlightState.OFF;
                }
                Mylight.enabled = !Mylight.enabled;
            
            }
            else
            {
                // value 1 equals true ,0 false
                flashlightActivated = (FlashlightState)value;
                Mylight.enabled = Convert.ToBoolean(value);
            }
    }
        
    
}
