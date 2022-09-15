using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponReloader : MonoBehaviour
{
    public delegate void OnWeaponReload();
    public static event OnWeaponReload onWeaponReload;
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            int currentWeapon = GetComponent<WeaponSwitcher>().CurrentWeapon;
            //will work if hierarchy is the same as ammo slots sequence
            Weapon weapon = transform.GetChild(currentWeapon).GetComponent<Weapon>();
            Ammo ammos = GetComponent<Ammo>();
            ammos.ReloadAmmo(weapon.AmmoType);
            onWeaponReload();

        }
    }
}
