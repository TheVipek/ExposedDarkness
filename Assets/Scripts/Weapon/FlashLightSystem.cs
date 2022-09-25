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
    // [SerializeField] float lightDecay = .1f;
    // [SerializeField] float angleDecay = 1f;
    // [SerializeField] float minimumAngle = 40f;
    // [SerializeField] float decreasingSpeed = 10f;
    // [SerializeField] bool isDecreasing = false;
    [SerializeField] MeshRenderer flashlightMesh;
    [SerializeField] Light Mylight;
    public bool needActivateBySelf = false;
    [SerializeField] KeyCode flashlightKey;
    FlashlightState flashlightActivated = FlashlightState.ON;
    WeaponZoom weapon;
    [SerializeField] string soundToPlay;
    AudioSource audioSource;
    private void OnEnable() {
        flashlightMesh.enabled = true;
        needActivateBySelf = false;
        Debug.Log("Enabled!");
    }
    private void Start() {
        weapon = GetComponentInParent<WeaponZoom>();
        audioSource = GetComponent<AudioSource>();
    }
    void Update()
    {
        if(Input.GetKeyDown(flashlightKey) && needActivateBySelf == false)
        {
            SwapFlashLighter();

        }
        if(weapon.isZoomed == true && needActivateBySelf == false)
        {
            //Debug.Log("Zoomed in");
            flashlightMesh.enabled = false;
            SwapFlashLighter(0);
            needActivateBySelf = true;
        }else if(weapon.isZoomed == false && needActivateBySelf == true)
        {
            Debug.Log("Out!");
            flashlightMesh.enabled = true;
            SwapFlashLighter(1);
            needActivateBySelf = false;
        }
        
    }
    public void SwapFlashLighter(int value = 2)
    {
        if(canDisplay() == true)
        {
            AudioManager.Instance.playSound(audioSource,soundToPlay);
            if(value == 2)
            {
                Debug.Log("Value is equal two");
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
            FlashlightDisplayer.Instance.SetFlashlightTextState(flashlightActivated);
            //AudioManager.Instance.playSound()
        }
        
    }
    public static bool canDisplay()
    {
        if(WeaponSwitcher.Instance.CurrentWeapon.AmmoType == AmmoType.Pistol_Bullets)
        {
            return true;
        }
        return false;

    }


    // public void RestoreLightAngle(float restoreAngle)
    // {
    //     Mylight.spotAngle = restoreAngle;
    // }
    // public void RestoreLightIntensity(float intensityAmount)
    // {
    //     Mylight.intensity += intensityAmount;
    // }

    // void DecreaseLightIntensity()
    // {
    //     if(Mylight.intensity>0)
    //     {
    //         Mylight.intensity -= lightDecay * Time.deltaTime *decreasingSpeed;
    //     }
    // }

    // void DecreaseLightAngle()
    // {
    //     if(Mylight.spotAngle > minimumAngle)
    //     {
    //         Mylight.spotAngle-=angleDecay * Time.deltaTime*decreasingSpeed;
    //     }
    // }
}
