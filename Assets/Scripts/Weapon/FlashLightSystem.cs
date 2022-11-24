using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    [SerializeField] KeyCode flashlightKey;
    FlashlightState flashlightActivated = FlashlightState.ON;
    [SerializeField] WeaponZoom weapon;
    [SerializeField] DefaultSoundKit flashlightSound;
    [SerializeField] AudioSource audioSource;
    private void OnEnable() {
        flashlightMesh.enabled = true;
        needActivateBySelf = false;
    }
    private void Start() {
       // weapon = GetComponentInParent<WeaponZoom>();
        //audioSource = GetComponent<AudioSource>();
    }
    void Update()
    {
        if(weapon.enabled == false) return;
        if(Input.GetKeyDown(flashlightKey) && needActivateBySelf == false)
        {
            SwapFlashLighter();

        }
        if(weapon.IsZoomed == true && needActivateBySelf == false)
        {
            //Debug.Log("Zoomed in");
            flashlightMesh.enabled = false;
            SwapFlashLighter(0);
            needActivateBySelf = true;
        }else if(weapon.IsZoomed == false && needActivateBySelf == true)
        {
            //Debug.Log("Out!");
            flashlightMesh.enabled = true;
            SwapFlashLighter(1);
            needActivateBySelf = false;
        }
        
    }
    public void SwapFlashLighter(int value = 2)
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
