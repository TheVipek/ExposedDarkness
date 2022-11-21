using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class WeaponDisplayer : MonoBehaviour
{
    [SerializeField] GameObject weaponsParent;
    [SerializeField] TMP_Text ammoInSlot;
    [SerializeField] TMP_Text totalAmmo;
    
    [SerializeField] TMP_Text weaponName;
    [SerializeField] Image weaponArt;
    [SerializeField] List<Image> bulletArt;
    [SerializeField] TMP_Text bulletText;
    [SerializeField] float maxWeaponImageHeight;
    [SerializeField] float maxWeaponImageWidth;


    Ammo ammos;
    WeaponSwitcher weaponSwitcher;

    public static WeaponDisplayer instance;
    private void Awake() {
        if(instance!=this && instance != null)
        {
            Destroy(this);
        }else
        {
            instance = this;
        }
    }
    private void OnEnable() 
    {
        WeaponSwitcher.onWeaponChange += DisplayChangeWeapon;
        WeaponSwitcher.onWeaponChange += DisplayChangeAmunition;
        Ammo.OnAmmoChange += DisplayChangeAmunition;
    }
    private void OnDisable() 
    {
        WeaponSwitcher.onWeaponChange -= DisplayChangeWeapon;
        WeaponSwitcher.onWeaponChange -= DisplayChangeAmunition;
        Ammo.OnAmmoChange -= DisplayChangeAmunition;
    }
    private void Start() {
        ammos = Ammo.instance;
        weaponSwitcher = WeaponSwitcher.Instance;
    }
    public void DisplayChangeAmunition()
    {
        ammoInSlot.text = ammos.GetAmmoInSlot(weaponSwitcher.CurrentWeapon.AmmoType).ToString();
        //which means that weapon has been changed or reloaded
        if(totalAmmo.text != ammos.GetAmmoAmount(weaponSwitcher.CurrentWeapon.AmmoType).ToString())
        {
            totalAmmo.text = ammos.GetAmmoAmount(weaponSwitcher.CurrentWeapon.AmmoType).ToString();
        }
    }
   
    public void DisplayChangeWeapon()
    {
        if(weaponName.text != weaponSwitcher.CurrentWeapon.name)
            {
                weaponName.text = weaponSwitcher.CurrentWeapon.name;
                weaponArt.sprite = weaponSwitcher.CurrentWeapon.WeaponIcon;
                float imageWidth = weaponArt.sprite.rect.width > maxWeaponImageWidth ? maxWeaponImageWidth : weaponArt.sprite.rect.width;
                weaponArt.GetComponent<RectTransform>().sizeDelta = new Vector2(imageWidth,maxWeaponImageHeight);
                DisplayChangeShootingType();
            }
    }
    public void DisplayChangeShootingType()
    {
        if(weaponSwitcher.CurrentWeapon.ConstantShooting == true)
        {
             foreach (var item in bulletArt)
            {
                item.gameObject.SetActive(true);
                item.sprite = weaponSwitcher.CurrentWeapon.BulletIcon;
            }
            
        }
        else
        {
            bulletArt.ForEach(x => x.gameObject.SetActive(false));
            bulletArt[1].gameObject.SetActive(true);
            bulletArt[1].sprite = weaponSwitcher.CurrentWeapon.BulletIcon;
        }    
        DisplayConstantShootingText();
    }
    public void DisplayConstantShootingText()
    {
        if(weaponSwitcher.CurrentWeapon.CanConstantShoot == true)
        {
            bulletText.enabled = true;
        }
        else
        {
            bulletText.enabled = false;
        }
    }
}
