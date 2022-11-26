using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class WeaponReloader : MonoBehaviour
{
    [SerializeField] Canvas reloadCanvas;
    [SerializeField] Image reloadImage;
    [SerializeField] Image bgImage;
    public bool currentlyReloading= false;
    WeaponSwitcher weaponSwitcher;
    private Weapon weapon;
    private WeaponZoom weaponZoom;
    private void Start() {
        weaponSwitcher = WeaponSwitcher.Instance;
    }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.R) && currentlyReloading == false) 
        {
            weapon = weaponSwitcher.CurrentWeapon;
            RangeWeapon currentWeapon = weapon.GetComponent<RangeWeapon>();
            weaponZoom = weapon.GetComponent<WeaponZoom>();
            if(Ammo.Instance.GetAmmoInSlot(currentWeapon.AmmoType) != Ammo.Instance.GetAmmoPerSlot(currentWeapon.AmmoType) && Ammo.Instance.GetAmmoAmount(currentWeapon.AmmoType) > 0)
            {
                StartCoroutine(reloadInitialization(currentWeapon,currentWeapon.TimeToReload));
            }

            
            

        }
    }

    IEnumerator reloadInitialization(RangeWeapon weapon,float timeToReload)
    {
        bool couldShootBefore = weapon.CanAttack;
        AudioManager.playSound(weapon.AudioSource,weapon.weaponSounds.ReloadSound);
        weapon.CanAttack = false;
        weaponZoom.CanZoom = false;
        reloadCanvas.enabled = true;
        
        currentlyReloading = true;
        float leftTime = timeToReload;

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
                weapon.AudioSource.Stop();
//                Debug.Log("Stopped playing");
                break;
            }
            yield return null;
        }
//        Debug.Log("After loop:"+leftTime);

//If timer went to 0 ,which means that reload process ended
        if(leftTime <= 0)
        {
            Ammo ammos = GetComponent<Ammo>();
            ammos.ReloadAmmo(weapon.AmmoType);
            weapon.CanAttack = true;
        }
        else
        {
            weapon.CanAttack = couldShootBefore;
        }
        reloadCanvas.enabled = false;
        //Setting it to 1 after disabling so next time when player try to reload bar will be full
        reloadImage.fillAmount = 1;
        bgImage.fillAmount = 1;
        currentlyReloading = false;
        weaponZoom.CanZoom = true;
    }
}
