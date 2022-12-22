using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class WeaponSwitcher : MonoBehaviour
{
    
    [Header("Current Weapon")]
    private int currentWeaponIndex = 0;
    public int CurrentWeaponIndex{get { return currentWeaponIndex; } }
    private Weapon currentWeapon;
    public Weapon CurrentWeapon{get{return currentWeapon;}}
    [Header("Previous Weapon")]
    private Weapon previousWeapon;
    private int previousWeaponIndex;
    public static Action onWeaponChange;
    public InputActionReference weaponSwitchMouse,weaponSwitchButtons;

    [Header("Audio")]
    [SerializeField] DefaultSoundKit weaponSwitch; 
    [SerializeField] AudioSource audioSource;
    private void OnEnable() {

        weaponSwitchMouse.action.started += OnMouseScroll;

        weaponSwitchButtons.action.started += OnButtonClick;

    }
    private void OnDisable() {

        weaponSwitchMouse.action.started -= OnMouseScroll;
        
        weaponSwitchButtons.action.started -= OnButtonClick;
    }
    void Start() 
    {
       SetWeaponActive();
    }
    void Update() 
    {
       // if(previousWeaponIndex!=currentWeaponIndex || previousWeapon != currentWeapon) weaponChange();
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
        if(ctx.started)
        {
            float value = ctx.ReadValue<float>();
            if(value > 0)
            {
                if(currentWeaponIndex >= WeaponsManager.Instance.AllWeapons.Count - 1) currentWeaponIndex = 0;
                else currentWeaponIndex += 1;
            }
            else
            {
                if(currentWeaponIndex <= 0) currentWeaponIndex = WeaponsManager.Instance.AllWeapons.Count -1;
                else currentWeaponIndex -= 1;
            }
            weaponChange();
        }
    }
    public void OnButtonClick(InputAction.CallbackContext ctx)
    {
        if(ctx.started)
        {
            //Debug.Log($"{ctx.control} Clicked!");
            if(ctx.control.name == "1") currentWeaponIndex=0;
            else if(ctx.control.name == "2") currentWeaponIndex=1;
            else if(ctx.control.name == "3") currentWeaponIndex=2;
        
            weaponChange();
        }
    }
    /// <summary> Setting weapon that has the same index as currentWeaponIndex </summary>
    private void SetWeaponActive(int weaponToSet = -1)
    {
        
        //Set currentWeapon and currentWeaponIdx to previous variables 
        if(previousWeapon != currentWeapon)
        {
            previousWeapon = currentWeapon;
            previousWeaponIndex = WeaponsManager.Instance.AllWeapons.IndexOf(previousWeapon);

        }

        //Active wanted weapon and set it as currentWeapon
        if(weaponToSet != -1)
        {
           currentWeapon = WeaponsManager.Instance.AllWeapons[weaponToSet];
           currentWeaponIndex = weaponToSet;

        }else
        {
            currentWeapon = WeaponsManager.Instance.AllWeapons[currentWeaponIndex];
        }
        if(previousWeaponIndex == currentWeaponIndex) return;
        //Disable previous weapon
        if(previousWeaponIndex != -1) WeaponsManager.Instance.WeaponActivate(previousWeaponIndex,false);
        //Enable current
        if(currentWeaponIndex != -1) WeaponsManager.Instance.WeaponActivate(currentWeaponIndex,true);
        
        //If previous weapon parent is different that means that we need to disable it and activate current weapon parent
        if(currentWeapon != null && previousWeapon != null)
        {
            SetWeaponsContainer(currentWeapon.transform.parent);
        }
    }
    private void SetWeaponsContainer(Transform containterTransform)
    {
        foreach (GameObject container in WeaponsManager.Instance.WeaponsContainers)
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
    
    
}
