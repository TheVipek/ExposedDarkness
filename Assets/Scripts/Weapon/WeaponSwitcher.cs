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
    int previousWeapon;
    static WeaponSwitcher instance;
    public static WeaponSwitcher Instance{get{return instance;}}
    public delegate void OnWeaponChange();
    public static event OnWeaponChange onWeaponChange;
    
     [Header("Get onWeaponChange so it may be"+"\n"+" called every time player shoots.")]
    public UnityEvent weaponEvent;
    [Header("Audio")]
    public string weaponSwitch; 
    private void Awake() {
        if(instance != null && instance != this)
        {
            Destroy(this);
        }else
        {
            instance = this;

        }
    }
    private void OnEnable() {
    }
    void Start() 
    {
        previousWeapon = currentWeaponIndex;
        SetWeaponActive();    
        getCurrentWeapon();
        onWeaponChange();
    }
    void Update() 
    {
        ProcessKeyInput();
        ProcessScrollWheel();

        if(previousWeapon!=currentWeaponIndex)
        {
            previousWeapon=currentWeaponIndex;
            SetWeaponActive();
            getCurrentWeapon();
            onWeaponChange();
        }
    }
    public void weaponChange()
    {
        onWeaponChange();
    }
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
        private void getCurrentWeapon()
    {
        currentWeapon = transform.GetChild(currentWeaponIndex).GetComponent<Weapon>();
    }
}
