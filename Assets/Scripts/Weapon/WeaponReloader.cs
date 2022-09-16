using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class WeaponReloader : MonoBehaviour
{
    public delegate void OnWeaponReload();
    public static event OnWeaponReload onWeaponReload;
    public Image reloadImage;
    public Image bgImage;
    public bool currentlyReloading= false;
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.R) && currentlyReloading == false) 
        {
            int currentWeapon = GetComponent<WeaponSwitcher>().CurrentWeapon;
            //will work if hierarchy is the same as ammo slots sequence
            Weapon weapon = transform.GetChild(currentWeapon).GetComponent<Weapon>();
            if(weapon.ammoSlot.GetAmmoInSlot(weapon.AmmoType) != weapon.ammoSlot.GetAmmoPerSlot(weapon.AmmoType))
            {
                StartCoroutine(reloadInitialization(weapon,weapon.timeToReload));
            }
            

        }
    }
    IEnumerator reloadInitialization(Weapon weapon,float timeToReload)
    {
        currentlyReloading = true;
        float leftTime = timeToReload;
        
        reloadImage.enabled = true;
        bgImage.enabled = true;
        while(leftTime >= 0)
        {
            leftTime-= Time.deltaTime;
            reloadImage.fillAmount = leftTime/timeToReload;
            if(weapon.weaponIndex != WeaponSwitcher.instance.CurrentWeapon) break;
            yield return null;
        }
        if(leftTime <= 0)
        {
            Ammo ammos = GetComponent<Ammo>();
            ammos.ReloadAmmo(weapon.AmmoType);
            onWeaponReload();
        }
        reloadImage.enabled = false;
        bgImage.enabled = false;
        reloadImage.fillAmount =1;
        currentlyReloading = false;
    }
}
