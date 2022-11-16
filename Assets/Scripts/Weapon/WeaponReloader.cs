using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class WeaponReloader : MonoBehaviour
{
    [SerializeField] Canvas reloadCanvas;
    [SerializeField] Image reloadImage;
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
            weaponZoom = weapon.GetComponent<WeaponZoom>();
            if(weapon.ammoSlot.GetAmmoInSlot(weapon.AmmoType) != weapon.ammoSlot.GetAmmoPerSlot(weapon.AmmoType) && weapon.ammoSlot.GetAmmoAmount(weapon.AmmoType) > 0)
            {
                StartCoroutine(reloadInitialization(weapon,weapon.TimeToReload));
            }
            

        }
    }
    IEnumerator reloadInitialization(Weapon weapon,float timeToReload)
    {
        AudioManager.playSound(weapon.AudioSource,weapon.weaponSounds.ReloadSound);

        weapon.enabled = false;
        weaponZoom.enabled = false;

        currentlyReloading = true;
        float leftTime = timeToReload;
        reloadCanvas.enabled = true;
        while(leftTime >= 0)
        {
            leftTime-= Time.deltaTime;
            reloadImage.fillAmount = leftTime/timeToReload;
            if(weapon.WeaponIndex !=weaponSwitcher.CurrentWeaponIndex)
            {
                weapon.AudioSource.Stop();
                break;
            }
            yield return null;
        }
        if(leftTime <= 0)
        {
            Ammo ammos = GetComponent<Ammo>();
            ammos.ReloadAmmo(weapon.AmmoType);
            weapon.EmptyAmmo = false;
        }

        reloadCanvas.enabled = false;
        reloadImage.fillAmount =1;
        
        currentlyReloading = false;

        weapon.enabled = true;
        weaponZoom.enabled = true;
    }
}
