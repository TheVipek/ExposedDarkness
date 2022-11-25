using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class WeaponSwitcher : MonoBehaviour
{
    [SerializeField] int currentWeaponIndex = 0;
    public int CurrentWeaponIndex{get { return currentWeaponIndex; } }
    [SerializeField] Weapon currentWeapon;
    public Weapon CurrentWeapon{get{return currentWeapon;}}

    private List<Weapon> alLWeapons = new List<Weapon>();
    public List<Weapon> AllWeapons {get {return alLWeapons;}}
    [SerializeField] WeaponReloader weaponReloader;
    [SerializeField] WeaponShootingTypeChanger weaponShootingTypeChanger;
    int previousWeaponIdx;
    Weapon previousWeapon;
    static WeaponSwitcher instance;
    public static WeaponSwitcher Instance{get{return instance;}}
    // public delegate void OnWeaponChange();
    // public static event OnWeaponChange onWeaponChange;
    public static Action onWeaponChange;
    
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
    }

    void Start() 
    {
        AudioManager.playSound(audioSource,weaponSwitch.Sound); 
        previousWeaponIdx = currentWeaponIndex;
        SetWeaponActive();    
        getAllWeapons();
        getCurrentWeapon();
        onWeaponChange();
    }
    void Update() 
    {
        ProcessKeyInput();
        ProcessScrollWheel();

        if(previousWeaponIdx!=currentWeaponIndex || previousWeapon != currentWeapon)
        {
            AudioManager.playSound(audioSource,weaponSwitch.Sound); 
            previousWeaponIdx=currentWeaponIndex;
            previousWeapon = currentWeapon;
            SetWeaponActive();
            getCurrentWeapon();
            onWeaponChange();
        }
    }
    // public void weaponChange()
    // {
    //     onWeaponChange();
    // }
    private void ProcessScrollWheel()
    {
        if(Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            if(currentWeaponIndex >= transform.childCount-1)
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
                currentWeaponIndex = transform.childCount-1;
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

    private void SetWeaponActive()
    {
        int weaponIndex =0;
        foreach (Transform weapon in transform)
        {
            if(weaponIndex == currentWeaponIndex)
            {
                weapon.gameObject.SetActive(true);
                
            }else
            {
                weapon.gameObject.SetActive(false);
            }
            weaponIndex+=1;
        }
    }
    
    private void getAllWeapons()
    {
        //Debug.Log(transform.childCount);
        foreach (Transform child in transform)
        {
            Weapon childItem = child.gameObject.GetComponent<Weapon>();
            if(childItem != null)
            {
                //Debug.Log(childItem.name);
                alLWeapons.Add(childItem);
            }
        }
       // Debug.Log(alLWeapons.Count);
    }
    public void getCurrentWeapon()
    {
        currentWeapon = transform.GetChild(currentWeaponIndex).GetComponent<Weapon>();
        if(currentWeapon.WeaponType != WeaponType.Range)
        {
            weaponReloader.enabled = false;
            weaponShootingTypeChanger.enabled = false;
        }else
        {
            weaponReloader.enabled = true;
            weaponShootingTypeChanger.enabled = true;
        }
    }
}
