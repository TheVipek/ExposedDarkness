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

    private Weapon currentWeapon;
    private Ammo ammos;

    public static WeaponDisplayer instance;
    private void Awake() {
        if(instance!=this && instance != null)
        {
            Destroy(this);
        }else
        {
            instance = this;
            ammos = weaponsParent.GetComponent<Ammo>();
        }
    }
    private void OnEnable() {
        WeaponSwitcher.onWeaponChange += DisplayChangeWeapon;
        WeaponSwitcher.onWeaponChange += DisplayChangeAmunition;
        WeaponReloader.onWeaponReload += DisplayChangeAmunition;
        WeaponShootingTypeChanger.onChangeShootingType += DisplayChangeShootingType;
    }
    private void OnDisable() {
        WeaponSwitcher.onWeaponChange -= DisplayChangeWeapon;
        WeaponSwitcher.onWeaponChange -= DisplayChangeAmunition;
        WeaponReloader.onWeaponReload -= DisplayChangeAmunition;
        WeaponShootingTypeChanger.onChangeShootingType -= DisplayChangeShootingType;

    }
    private void Start() {
    }
    void Update()
    {
        
    }
    private void getCurrentWeapon()
    {
        int _currentWeapon = weaponsParent.GetComponent<WeaponSwitcher>().CurrentWeapon;
        currentWeapon = weaponsParent.transform.GetChild(_currentWeapon).GetComponent<Weapon>();
    }
    public void DisplayChangeAmunition()
    {
        getCurrentWeapon();
        
        ammoInSlot.text = ammos.GetAmmo(currentWeapon.AmmoType).ToString();

        //which means that weapon has been changed or reloaded
        if(totalAmmo.text != ammos.GetTotalAmmo(currentWeapon.AmmoType).ToString())
        {
            totalAmmo.text = ammos.GetTotalAmmo(currentWeapon.AmmoType).ToString();
            
        }
        

    }
    public void DisplayChangeWeapon()
    {
        getCurrentWeapon();
        if(weaponName.text != currentWeapon.name)
            {
                weaponName.text = currentWeapon.name;
                weaponArt.sprite = currentWeapon.weaponIcon;
                float imageWidth = weaponArt.sprite.rect.width/2 > maxWeaponImageWidth ? maxWeaponImageWidth : weaponArt.sprite.rect.width;
                weaponArt.GetComponent<RectTransform>().sizeDelta = new Vector2(imageWidth,maxWeaponImageHeight);
                if(currentWeapon.ConstantShooting == false)
                {
                    bulletArt.ForEach(x => x.gameObject.SetActive(false));
                    bulletArt[0].gameObject.SetActive(true);
                    bulletArt[0].sprite = currentWeapon.bulletIcon;
                }
                else
                {
                    foreach (var item in bulletArt)
                    {
                        item.gameObject.SetActive(true);
                        item.sprite = currentWeapon.bulletIcon;
                    }
                }
                
                
            }
    }
    public void DisplayChangeShootingType()
    {
        getCurrentWeapon();
        Debug.Log(currentWeapon.CanConstantShoot);
        if(currentWeapon.CanConstantShoot == true)
            {
                if(currentWeapon.ConstantShooting == true)
                {
                    bulletArt.ForEach(x => x.gameObject.SetActive(false));
                    bulletArt[0].gameObject.SetActive(true);
                    bulletArt[0].sprite = currentWeapon.bulletIcon;
                }
                else
                {
                    foreach (var item in bulletArt)
                    {
                        item.gameObject.SetActive(true);
                        item.sprite = currentWeapon.bulletIcon;
                    }
                }
                
                currentWeapon.ConstantShooting = !currentWeapon.ConstantShooting;
            }
        else
            {
                bulletArt.ForEach(x => x.gameObject.SetActive(false));
                bulletArt[0].gameObject.SetActive(true);
                bulletArt[0].sprite = currentWeapon.bulletIcon;

            }
    }
}
