using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
[RequireComponent(typeof(AudioSource))]
public class Weapon : MonoBehaviour
{
    public int WeaponIndex {get; private set;}
    

    [Header("Weapon settings")]
    [SerializeField] float range = 100f;
    [SerializeField] float damage;
    [SerializeField] float shootingDelay;
    [SerializeField] bool canConstantShoot = false;
    [SerializeField] bool constantShooting;
    [SerializeField] bool canWieldFlashLight;
    
    //Weapon Properties and fields
    public bool CanWieldFlashLight{get{return canWieldFlashLight;}}
    public bool CanConstantShoot{get{return canConstantShoot;}}
    public bool ConstantShooting{get{return constantShooting;}set{constantShooting = value;}}
    public bool CanShoot{get{return canShoot;} set{canShoot = value;}}
    public bool EmptyAmmo{get{return emptyAmmo;} set{emptyAmmo = value;}}
    public float TimeToReload {get {return weaponSounds.ReloadSound.length;}}
    private bool canShoot = true;
    private bool emptyAmmo = false;
    private WaitForSeconds wShootingDelay;
    [HideInInspector] public Ammo ammoSlot;
    [SerializeField] AmmoType ammoType;
    public AmmoType AmmoType{get{ return ammoType; }}

    
    [Header("Audio")]
    public WeaponSoundKit weaponSounds; 
    public AudioSource AudioSource{get; private set;}
    private bool firstEnable = true;

    
    [Header("VFX/Sprites")]
    [SerializeField] ParticleSystem shootVFX;
    [SerializeField] GameObject hitEffect;
    [SerializeField] Sprite weaponIcon;
    public Sprite WeaponIcon{ get {return weaponIcon;}}
    [SerializeField] Sprite bulletIcon;
    public Sprite BulletIcon{ get {return bulletIcon;}}


    
    
    void OnEnable() {
        WeaponShootingTypeChanger.onChangeShootingType += SwapConstantShooting;
        if(firstEnable == true)
        {
            firstEnable = false;
        }else
        {
//
           // AudioManager.Instance.playSound(audioSource,WeaponSwitcher.Instance.weaponSwitch);

        }
        canShoot = true;

    }
    void OnDisable() {
        WeaponShootingTypeChanger.onChangeShootingType -= SwapConstantShooting;
    }
    private void Awake() 
    {
        //To avoid GC
        wShootingDelay = new WaitForSeconds(shootingDelay);
        ammoSlot = GetComponentInParent<Ammo>();
        WeaponIndex = transform.GetSiblingIndex();
        constantShooting = canConstantShoot;
        AudioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if((Input.GetMouseButton(0) && canShoot) && constantShooting && emptyAmmo == false)
        {
            Shoot();
        }
        else if((Input.GetMouseButtonDown(0) && canShoot) && (constantShooting == false || emptyAmmo == true))
        {
            Shoot();

        }
    }
    private void Shoot()
    {
        
        StartCoroutine(Shooting());
    }
    IEnumerator Shooting()
    {
        canShoot = false;
        if(ammoSlot.GetAmmoInSlot(ammoType) > 0)
        {
            AudioManager.playSound(AudioSource,weaponSounds.ShootSound);
            PlayMuzzleFlash();
            ProcessRaycast();
            ammoSlot.UseAmmo(ammoType);
        }else
        {
            AudioManager.playSound(AudioSource,weaponSounds.EmptySound);
            emptyAmmo = true;
        }
        //To avoid GC
        yield return wShootingDelay;
        canShoot = true;
        yield return null;
    }

    private void PlayMuzzleFlash()
    {
        shootVFX.Play();
    }

    private void ProcessRaycast()
    {
        RaycastHit hit;
        //point that we're looking at already
        Ray ray = Camera.main.ViewportPointToRay(new Vector2(0.5f,0.5f));

        if (Physics.Raycast(ray.origin, ray.direction, out hit, range))
        {
            if (hit.transform.tag == "Enemy")
            {
                CreateHitImpact(hit);
                Enemy target = hit.transform.gameObject.GetComponent<Enemy>();
                target.TakeDamage(damage);
            }
            

        }
        else
        {
            return;
        }
    }

    private void CreateHitImpact(RaycastHit hit)
    {
        
        GameObject hitUFX = Instantiate(hitEffect,hit.point,Quaternion.LookRotation(hit.normal));
        ParticleSystem hitParticle = hitUFX.transform.GetChild(0).GetComponent<ParticleSystem>();
        Destroy(hitUFX,hitParticle.main.duration);
    }
    
    public void SwapConstantShooting()
    {
        if(canConstantShoot == false) return;
        constantShooting = !constantShooting;
        AudioManager.playSound(AudioSource,weaponSounds.ShootingTypeSound);
        WeaponDisplayer.instance.DisplayChangeShootingType();
    }
}
