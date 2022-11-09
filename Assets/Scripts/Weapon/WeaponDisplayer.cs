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
    private void OnEnable() {

        
        WeaponSwitcher.onWeaponChange += DisplayChangeWeapon;
        WeaponSwitcher.onWeaponChange += DisplayChangeAmunition;
        WeaponReloader.onWeaponReload += DisplayChangeAmunition;
        Ammo.onAmmoAdd += DisplayChangeAmunition;

        WeaponShootingTypeChanger.onChangeShootingType += DisplayChangeShootingType;
    }
    private void OnDisable() {
        WeaponSwitcher.onWeaponChange -= DisplayChangeWeapon;
        WeaponSwitcher.onWeaponChange -= DisplayChangeAmunition;
        WeaponReloader.onWeaponReload -= DisplayChangeAmunition;
        Ammo.onAmmoAdd -= DisplayChangeAmunition;

        WeaponShootingTypeChanger.onChangeShootingType -= DisplayChangeShootingType;

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
                weaponArt.sprite = weaponSwitcher.CurrentWeapon.weaponIcon;
                float imageWidth = weaponArt.sprite.rect.width > maxWeaponImageWidth ? maxWeaponImageWidth : weaponArt.sprite.rect.width;
                weaponArt.GetComponent<RectTransform>().sizeDelta = new Vector2(imageWidth,maxWeaponImageHeight);
                if(weaponSwitcher.CurrentWeapon.ConstantShooting == false)
                {
                    bulletArt.ForEach(x => x.gameObject.SetActive(false));
                    bulletArt[1].gameObject.SetActive(true);
                    bulletArt[1].sprite = weaponSwitcher.CurrentWeapon.bulletIcon;
                }
                else
                {
                    foreach (var item in bulletArt)
                    {
                        item.gameObject.SetActive(true);
                        item.sprite = weaponSwitcher.CurrentWeapon.bulletIcon;
                    }
                }
            }
    }
    public void DisplayChangeShootingType()
    {
        if(weaponSwitcher.CurrentWeapon.CanConstantShoot == true)
            {
                if(weaponSwitcher.CurrentWeapon.ConstantShooting == true)
                {
                    bulletArt.ForEach(x => x.gameObject.SetActive(false));
                    bulletArt[1].gameObject.SetActive(true);
                    bulletArt[1].sprite = weaponSwitcher.CurrentWeapon.bulletIcon;
                }
                else
                {
                    foreach (var item in bulletArt)
                    {
                        item.gameObject.SetActive(true);
                        item.sprite = weaponSwitcher.CurrentWeapon.bulletIcon;
                    }
                }
                
                weaponSwitcher.CurrentWeapon.SwapConstantShooting();
                
            }
        else
            {
                bulletArt.ForEach(x => x.gameObject.SetActive(false));
                bulletArt[1].gameObject.SetActive(true);
                bulletArt[1].sprite = weaponSwitcher.CurrentWeapon.bulletIcon;

            }
    }
}
