using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.InputSystem;

public class WeaponReloader : MonoBehaviour
{
    [SerializeField] Canvas reloadCanvas;
    [SerializeField] Image reloadImage;
    [SerializeField] Image bgImage;
    public bool currentlyReloading= false;
    private Weapon weapon;
    private WeaponZoom weaponZoom;
    private Coroutine reloadInitializationCR;
    private bool couldShootBefore;
    WeaponSwitcher weaponSwitcher;
    public InputActionReference reloadAction;

    private void Start() {
        weaponSwitcher = WeaponsManager.Instance.weaponSwitcher; 
        reloadAction.action.started += OnReload;
    }
    
    private void OnReload(InputAction.CallbackContext ctx)
    {
        if(ctx.started)
        {
            if(!currentlyReloading)
            {
                weapon = weaponSwitcher.CurrentWeapon;
                RangeWeapon currentWeapon = weapon.GetComponent<RangeWeapon>();
                weaponZoom = weapon.GetComponent<WeaponZoom>();
                if(Ammo.Instance.GetAmmoInSlot(currentWeapon.AmmoType) != Ammo.Instance.GetAmmoPerSlot(currentWeapon.AmmoType) && Ammo.Instance.GetAmmoAmount(currentWeapon.AmmoType) > 0)
                {
                    reloadInitializationCR = StartCoroutine(reloadInitialization(currentWeapon,currentWeapon.TimeToReload));
                }
            }
        }
    }
    private void OnEnable() {
        reloadInitializationCR = null;
        currentlyReloading = false;

    }
    private void OnDisable() {
        Debug.Log("WeaponReloader disabled");
        if(reloadInitializationCR != null)
        {
            StopCoroutine(reloadInitializationCR);
            weapon.CanAttack = couldShootBefore;
            weaponZoom.CanZoom = true;
            ResetUI();
            if(!PlayerMovement.Instance.sprintAction.action.enabled)
            {
                PlayerMovement.Instance.sprintAction.action.Enable();
                Debug.Log($"sprintAction is: {PlayerMovement.Instance.sprintAction.action.enabled}");
                PlayerMovement.Instance.SetSpeed();
            }
        }
    }
    
    IEnumerator reloadInitialization(RangeWeapon weapon,float timeToReload)
    {
        couldShootBefore = weapon.CanAttack;
        AudioManager.playSound(weapon.AudioSource,weapon.weaponSounds.ReloadSound);

        weapon.CanAttack = false;
        
        weaponZoom.CanZoom = false;
        reloadCanvas.enabled = true;
        

        currentlyReloading = true;
        float leftTime = timeToReload;
        PlayerMovement.Instance.sprintAction.action.Disable();
       PlayerMovement.Instance.SetSpeed(0.7f);
//Reloading timer
        while(leftTime >= 0)
        {

            leftTime-= Time.deltaTime;
            reloadImage.fillAmount = leftTime/timeToReload;
            bgImage.fillAmount = reloadImage.fillAmount;
//If player tries to swap weapon during reloading ,reloading process will be disturbed
            if(weapon!= weaponSwitcher.CurrentWeapon)
            {
//                Debug.Log(weapon.name + "is not " + weaponSwitcher.CurrentWeapon.name);
                Debug.Log($"Swapped to different weapon - canceling reloading : {weapon} != {weaponSwitcher.CurrentWeapon}");
                weapon.AudioSource.Stop();
                PlayerMovement.Instance.sprintAction.action.Enable();
                Debug.Log($"sprintAction is: {PlayerMovement.Instance.sprintAction.action.enabled}");
                PlayerMovement.Instance.SetSpeed();


//                Debug.Log("Stopped playing");
                break;
            }
            yield return null;
        }
//        Debug.Log("After loop:"+leftTime);

//If timer went to 0 ,which means that reload process ended
        if(leftTime <= 0)
        {
            Debug.Log("Weapon changed1");
            Ammo ammos = GetComponent<Ammo>();
            ammos.ReloadAmmo(weapon.AmmoType);
            weapon.CanAttack = true;
        }
        else
        {
            Debug.Log("Weapon changed2");

            weapon.CanAttack = couldShootBefore;
        }
        PlayerMovement.Instance.sprintAction.action.Enable();
        PlayerMovement.Instance.SetSpeed(1f);
        ResetUI();
        currentlyReloading = false;
        weaponZoom.CanZoom = true;
        reloadInitializationCR = null;
    }
    public void ResetUI()
    {
        reloadCanvas.enabled = false;
        //Setting it to 1 after disabling so next time when player try to reload bar will be full
        reloadImage.fillAmount = 1;
        bgImage.fillAmount = 1;
    }
}
