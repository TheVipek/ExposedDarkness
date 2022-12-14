using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(WeaponZoom))]
public class RangeWeapon : Weapon
{
    [SerializeField] Sprite bulletIcon;

    [HideInInspector] public Ammo ammoContainer;
    [Header("Ammo")]
    [SerializeField] AmmoType ammoType;
    public AmmoType AmmoType{get{ return ammoType; }}
    public float TimeToReload {get {return weaponSounds.ReloadSound.length;}}
    private WeaponZoom weaponZoom;
    [Header("SoundKit")]
    public RangeWeaponSoundKit weaponSounds; 
    public Sprite BulletIcon{ get {return bulletIcon;}}
    private void OnEnable() {
        WeaponShootingTypeChanger.onChangeShootingType += SwapConstantShooting;
        
    }
    private void OnDisable() {
        WeaponShootingTypeChanger.onChangeShootingType -= SwapConstantShooting;
        
    }
    protected override void Awake() {
        base.Awake();
        weaponZoom = GetComponent<WeaponZoom>();
    }
    protected override void Start()
    {
        base.Start();
        ammoContainer = WeaponsManager.Instance.ammo;
    }
    public override void PrimaryAction()
    {
        if(ammoContainer.GetAmmoInSlot(ammoType) > 0)
        {
            AudioManager.playSound(AudioSource,weaponSounds.ShootSound);
            PlayAttackSound();
            ProcessRaycast(primaryAttackDamage);
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
        WeaponsManager.Instance.weaponDisplayer.DisplayChangeShootingType();
    }
}
