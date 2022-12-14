using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class WeaponsManager : MonoBehaviour
{
    [Header("Current Weapon Settings")]

    

    [Header("Weapons containers")]
    const int rangeContaienrWeaponIndex = 0;
    const int meeleContainerWeaponIndex = 1;
    [SerializeField] List<GameObject> weaponsContainers = new List<GameObject>();
    public List<GameObject> WeaponsContainers {get {return weaponsContainers;}}
    [SerializeField] List<Weapon> allWeapons = new List<Weapon>();
    public List<Weapon> AllWeapons {get {return allWeapons;}}
    [Header("References")]
    public WeaponReloader weaponReloader;
    public Ammo ammo;
    public WeaponShootingTypeChanger weaponShootingTypeChanger;
    public WeaponSwitcher weaponSwitcher;
    public WeaponDisplayer weaponDisplayer;
    public Action onWeaponPickup;

    public static WeaponsManager Instance {get; private set;}

    private void Awake() {
        if(Instance!= this && Instance != null) Destroy(this);
        else Instance = this;
    }
    private void OnEnable() {
        
        onWeaponPickup += getAllWeapons;
        
    }
    private void OnDisable() {
        onWeaponPickup -= getAllWeapons;
        
    }
    public void getAllWeapons()
    {
        if(allWeapons.Count > 0) allWeapons.Clear();
        for(int i=0 ; i < weaponsContainers.Count ; i++)
        {
            foreach(Transform child in weaponsContainers[i].transform)
            {
                Weapon childItem = child.gameObject.GetComponent<Weapon>();
                if(childItem != null)
                {
                    allWeapons.Add(childItem);
                } 
            }
        }
    }
    public Weapon getCurrentWeapon() => AllWeapons.First(x => x.gameObject.activeSelf == true);
    public Weapon getWeapon(int weaponIdx) => AllWeapons[weaponIdx];
    public void WeaponActivate(int weaponIdx,bool activate) => allWeapons[weaponIdx].gameObject.SetActive(activate);

    public void WeaponsControls(bool activate)
    {
        
    }

}

