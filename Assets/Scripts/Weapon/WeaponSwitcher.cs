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
    public static Action onWeaponChange;
    public Action onWeaponPickup;
    [Header("Audio")]
    [SerializeField] DefaultSoundKit weaponSwitch; 
    [SerializeField] AudioSource audioSource;
    [SerializeField] InputActionReference weaponSwitchMouse,weaponSwitchButtons;
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
        weaponSwitchMouse.action.started += OnMouseScroll;
        weaponSwitchButtons.action.started += OnButtonClick;
    }
    private void OnDisable() {
        onWeaponPickup -= getAllWeapons;
        weaponSwitchMouse.action.started -= OnMouseScroll;
        weaponSwitchButtons.action.started -= OnButtonClick;
    }
    void Start() 
    {
        weaponChange();
    }
    void Update() 
    {
        if(previousWeaponIndex!=currentWeaponIndex || previousWeapon != currentWeapon) weaponChange();
    }
    public void weaponChange(int weaponToSet = -1)
    {
        
        AudioManager.playSound(audioSource,weaponSwitch.Sound);
        //Look for weapon with the same index as currentWeapon
        SetWeaponActive(weaponToSet);
        onWeaponChange();
    }
    
    public void OnMouseScroll(InputAction.CallbackContext ctx)
    {
        Debug.Log(ctx.phase);
        if(ctx.started)
        {
            float value = ctx.ReadValue<float>();
            if(value > 0)
            {
                if(currentWeaponIndex >= allWeapons.Count - 1) currentWeaponIndex = 0;
                else currentWeaponIndex += 1;
            }
            else
            {
                if(currentWeaponIndex <= 0) currentWeaponIndex = allWeapons.Count -1;
                else currentWeaponIndex -= 1;
            }
        }
    }
    public void OnButtonClick(InputAction.CallbackContext ctx)
    {
        Debug.Log(ctx.phase);
        
        if(ctx.started)
        {
            if(ctx.control.name == "1") currentWeaponIndex=0;
            else if(ctx.control.name == "2") currentWeaponIndex=1;
            else if(ctx.control.name == "3") currentWeaponIndex=2;
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
                } 
            }
        }
    }
}
