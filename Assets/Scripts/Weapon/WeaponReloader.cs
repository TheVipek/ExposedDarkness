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
    WeaponSwitcher weaponSwitcher;
    private void Start() {
        weaponSwitcher = WeaponSwitcher.Instance;
    }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.R) && currentlyReloading == false) 
        {
            //will work if hierarchy is the same as ammo slots sequence
            Weapon weapon = weaponSwitcher.CurrentWeapon;
            if(weapon.ammoSlot.GetAmmoInSlot(weapon.AmmoType) != weapon.ammoSlot.GetAmmoPerSlot(weapon.AmmoType) && weapon.ammoSlot.GetAmmoAmount(weapon.AmmoType) > 0)
            {
                StartCoroutine(reloadInitialization(weapon,weapon.timeToReload));
            }
            

        }
    }
    IEnumerator reloadInitialization(Weapon weapon,float timeToReload)
    {

        AudioManager.Instance.playSound(weapon.audioSource,weapon.Reload);
        currentlyReloading = true;
        float leftTime = timeToReload;
        
        reloadImage.enabled = true;
        bgImage.enabled = true;
        while(leftTime >= 0)
        {
            leftTime-= Time.deltaTime;
            reloadImage.fillAmount = leftTime/timeToReload;
            if(weapon.weaponIndex !=weaponSwitcher.CurrentWeaponIndex)
            {
                weapon.audioSource.Stop();
                break;
            }
            yield return null;
        }
        if(leftTime <= 0)
        {
            Ammo ammos = GetComponent<Ammo>();
            ammos.ReloadAmmo(weapon.AmmoType);
            onWeaponReload();
            weapon.EmptyAmmo = false;
        }
        reloadImage.enabled = false;
        bgImage.enabled = false;
        reloadImage.fillAmount =1;
        currentlyReloading = false;
    }
}
