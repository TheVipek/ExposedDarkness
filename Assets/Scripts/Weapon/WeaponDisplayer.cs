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


    private Ammo ammoContainer;
    private WeaponSwitcher weaponSwitcher;
    public static WeaponDisplayer Instance;
    private void Awake() {
        if(Instance!=this && Instance != null) Destroy(this);
        else Instance = this;
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
        ammoContainer = Ammo.Instance;
        weaponSwitcher = WeaponsManager.Instance.weaponSwitcher;
    }
    public void DisplayChangeAmunition()
    {
        if(weaponSwitcher.CurrentWeapon.WeaponType != WeaponType.Range)
        {
            ammoInSlot.enabled = false;
            totalAmmo.enabled = false;
        }
        else
        {
            ammoInSlot.enabled = true;
            totalAmmo.enabled = true;
            RangeWeapon currentWeapon = weaponSwitcher.CurrentWeapon.GetComponent<RangeWeapon>();

//            Debug.Log("Current weapon type is : "+weaponSwitcher.CurrentWeapon.GetType());

            ammoInSlot.text = ammoContainer.GetAmmoInSlot(currentWeapon.AmmoType).ToString();
            //which means that weapon has been changed or reloaded
            if(totalAmmo.text != ammoContainer.GetAmmoAmount(currentWeapon.AmmoType).ToString())
            {
                totalAmmo.text = ammoContainer.GetAmmoAmount(currentWeapon.AmmoType).ToString();
            }
        }
    }
   
    public void DisplayChangeWeapon()
    {
      //  Debug.Log(weaponName.text);
     //   Debug.Log(weaponSwitcher.CurrentWeapon.name);
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
        if(weaponSwitcher.CurrentWeapon.WeaponType != WeaponType.Range)
        {
            bulletArt.ForEach(x => x.gameObject.SetActive(false));
            bulletText.enabled = false;
        }
        else
        {
            RangeWeapon currentWeapon = weaponSwitcher.CurrentWeapon.GetComponent<RangeWeapon>();
            if(weaponSwitcher.CurrentWeapon.IsConstantAttacking == true)
            {
                foreach (var item in bulletArt)
                {
                    item.gameObject.SetActive(true);
                    item.sprite = currentWeapon.BulletIcon;
                }
                
            }
            else
            {
                bulletArt.ForEach(x => x.gameObject.SetActive(false));
                bulletArt[1].gameObject.SetActive(true);
                bulletArt[1].sprite = currentWeapon.BulletIcon;
            }    
            DisplayConstantShootingText();
        }
    }
    public void DisplayConstantShootingText()
    {
        if(weaponSwitcher.CurrentWeapon.CanConstantAttack == true)
        {
            bulletText.enabled = true;
        }
        else
        {
            bulletText.enabled = false;
        }
    }
}
