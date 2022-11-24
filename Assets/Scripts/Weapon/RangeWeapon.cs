using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(WeaponZoom))]
public class RangeWeapon : Weapon
{
    
    [HideInInspector] public Ammo ammoContainer;
    [SerializeField] AmmoType ammoType;
    public AmmoType AmmoType{get{ return ammoType; }}
    public float TimeToReload {get {return weaponSounds.ReloadSound.length;}}
    private WeaponZoom weaponZoom;
    [SerializeField] Sprite bulletIcon;
    public Sprite BulletIcon{ get {return bulletIcon;}}
    private void OnEnable() {
        WeaponShootingTypeChanger.onChangeShootingType += SwapConstantShooting;
        
    }
    private void OnDisable() {
        WeaponShootingTypeChanger.onChangeShootingType -= SwapConstantShooting;
        
    }

    void Start()
    {
        ammoContainer = Ammo.Instance;
        weaponZoom = GetComponent<WeaponZoom>();
    }
    public override void Attack()
    {
        if(ammoContainer.GetAmmoInSlot(ammoType) > 0)
        {
            AudioManager.playSound(AudioSource,weaponSounds.ShootSound);
            PlayAttackSound();
            ProcessRaycast();
            ammoContainer.UseAmmo(ammoType);
        }else
        {
            AudioManager.playSound(AudioSource,weaponSounds.EmptySound);
            canAttack = false;
        }
    }
    // Update is called once per frame
    protected override void Update()
    {
           base.Update();
    }
    protected void SwapConstantShooting()
    {
        if(canConstantAttack == false) return;
        isConstantAttacking = !isConstantAttacking;
        AudioManager.playSound(AudioSource,weaponSounds.ShootingTypeSound);
        WeaponDisplayer.instance.DisplayChangeShootingType();
    }
}
