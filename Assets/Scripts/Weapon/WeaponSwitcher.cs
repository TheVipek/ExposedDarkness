using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class WeaponSwitcher : MonoBehaviour
{
    [Header("Current Weapon Settings")]
    [SerializeField] int currentWeaponIndex = 0;
    public int CurrentWeaponIndex{get { return currentWeaponIndex; } }
    [SerializeField] Weapon currentWeapon;
    public Weapon CurrentWeapon{get{return currentWeapon;} set{currentWeapon = value;}}

    [Header("Weapons containers")]
    // [SerializeField] GameObject meeleWeaponsContainer;
    // [SerializeField] GameObject rangeWeaponsContainer;
    [SerializeField] const int rangeContaienrWeaponIndex = 0;
    [SerializeField] const int meeleContainerWeaponIndex = 1;
    [SerializeField] List<GameObject> weaponsContainers = new List<GameObject>();
    [SerializeField] List<Weapon> allWeapons = new List<Weapon>();
    public List<Weapon> AllWeapons {get {return allWeapons;}}
    [Header("References")]
    [SerializeField] WeaponReloader weaponReloader;
    [SerializeField] WeaponShootingTypeChanger weaponShootingTypeChanger;
    int previousWeaponIndex;
    Weapon previousWeapon;
    static WeaponSwitcher instance;
    public static WeaponSwitcher Instance{get{return instance;}}
    // public delegate void OnWeaponChange();
    // public static event OnWeaponChange onWeaponChange;
    public static Action onWeaponChange;
    public Action onWeaponPickup;
    
    //  [Header("Get onWeaponChange so it may be"+"\n"+" called every time player shoots.")]
    // public UnityEvent weaponEvent;
    [Header("Audio")]
    [SerializeField] DefaultSoundKit weaponSwitch; 
    [SerializeField] AudioSource audioSource;
    private void Awake() {
        if(instance != null && instance != this)
        {
            Destroy(this);
        }else
        {
            instance = this;

        }
        getAllWeapons();
    }
    private void OnEnable() {
        onWeaponPickup += getAllWeapons;
    }
    private void OnDisable() {
        onWeaponPickup -= getAllWeapons;
        
    }
    void Start() 
    {
        weaponChange();
    }
    void Update() 
    {
        ProcessKeyInput();
        ProcessScrollWheel();

        if(previousWeaponIndex!=currentWeaponIndex || previousWeapon != currentWeapon)
        {
            weaponChange();
        }
    }
    public void weaponChange(int weaponToSet = -1)
    {
        
        AudioManager.playSound(audioSource,weaponSwitch.Sound);
        
        //getAllWeapons();
        //Look for weapon with the same index as currentWeapon
        SetWeaponActive(weaponToSet);
        onWeaponChange();
    }
    
    
    private void ProcessScrollWheel()
    {
        if(Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            if(currentWeaponIndex >= allWeapons.Count - 1)
            {
                currentWeaponIndex = 0;
            }else
            {
                currentWeaponIndex+=1;
            }
            
        }
        if(Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            if(currentWeaponIndex <= 0)
            {
                currentWeaponIndex = allWeapons.Count - 1;
            }else
            {
                currentWeaponIndex-=1;
            }

        }
    }
    private void ProcessKeyInput()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            currentWeaponIndex=0;
        }else if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            currentWeaponIndex=1;
        }else if(Input.GetKeyDown(KeyCode.Alpha3))
        {
            currentWeaponIndex=2;
        }
    }

    private void SetWeaponActive(int weaponToSet)
    {
        //Set currentWeapon and currentWeaponIdx to previous variables 
        if(currentWeapon != null)
        {
            previousWeaponIndex = currentWeaponIndex;
            previousWeapon = currentWeapon;
        }

        //Disable all weapons
        allWeapons.ForEach(x => x.gameObject.SetActive(false));
        //Active wanted weapon and set it as currentWeapon
        if(weaponToSet != -1)
        {
           currentWeapon = allWeapons[weaponToSet];
           currentWeaponIndex = weaponToSet;

        }else
        {
            currentWeapon = AllWeapons[currentWeaponIndex];
        }
        currentWeapon.gameObject.SetActive(true);
        
        //If previous weapon parent is different that means that we need to disable it and activate current weapon parent
        if(currentWeapon != null && previousWeapon != null)
        {
            SetWeaponsContainer(currentWeapon.transform.parent);
        }
        // int weaponIndex =0;
        // foreach (Transform weapon in transform)
        // {
        //     if(weaponIndex == currentWeaponIndex)
        //     {
        //         weapon.gameObject.SetActive(true);
        //         currentWeapon = weapon.GetComponent<Weapon>();
                
        //     }else
        //     {
        //         weapon.gameObject.SetActive(false);
        //     }
        //     weaponIndex+=1;
        // }
    }
    private void SetWeaponsContainer(Transform containterTransform)
    {
        foreach (GameObject container in weaponsContainers)
        {
            if(container.transform == containterTransform)
            {
                container.SetActive(true);
            }else
            {
                container.SetActive(false);
            }
        }
    }
    private void getAllWeapons()
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
                    Debug.Log(childItem.name);
                } 
            }
        }
    }
}
