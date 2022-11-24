using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(AudioSource),typeof(WeaponAnimation))]
public abstract class Weapon : MonoBehaviour,IAttack
{  
    [Header("Weapon settings")]
    [SerializeField] protected KeyCode primaryAction = KeyCode.Mouse0;

    [SerializeField] float range = 100f;
    [SerializeField] float damage;
    [SerializeField] protected float attackDelay;
    [SerializeField] WeaponType weaponType;
    [SerializeField] protected WeaponAnimation weaponAnimation;
    public WeaponType WeaponType{get{ return weaponType;}}
    protected float nextAttackTime;
    
    [Header("Weapon properties")]
    [Tooltip("Which means ; does player need to click every time to attack")]

    [SerializeField] protected bool canConstantAttack = false;
    public bool CanConstantAttack{get{return canConstantAttack;}}
    public bool IsConstantAttacking{get{return isConstantAttacking;}set{isConstantAttacking = value;}}
    protected bool isConstantAttacking;
    public bool CanAttack{get{return canAttack;} set{canAttack = value;}}
    [SerializeField] protected bool canAttack = true;



    
    [Header("Weapon audio")]
    [SerializeField] AudioSource audioSource;
    public AudioSource AudioSource{get {return audioSource;}}
    public WeaponSoundKit weaponSounds; 
    
    [Header("Weapon VFX/Sprites/Needed references")]
    [SerializeField] ParticleSystem attackVFX;
    [SerializeField] GameObject hitEffect;
    [SerializeField] Sprite weaponIcon;
    public Sprite WeaponIcon{ get {return weaponIcon;}}


    private void Awake() 
    {
        isConstantAttacking = canConstantAttack;

    }


    protected virtual void Update()
    {
        if(canAttack)
        {
            
            if(Input.GetKey(primaryAction) && Time.time > nextAttackTime && isConstantAttacking == true)
            {
                Attack();
                nextAttackTime = Time.time + attackDelay;
            }
            else if(Input.GetKeyDown(primaryAction) && Time.time > nextAttackTime && isConstantAttacking == false)
            {
                Attack();
                nextAttackTime = Time.time + attackDelay;
                
            }
        }
    }

    public abstract void Attack();
    
        // if(ammoContainer.GetAmmoInSlot(ammoType) > 0)
        // {
        //     AudioManager.playSound(AudioSource,weaponSounds.ShootSound);
        //     PlayAttackSound();
        //     ProcessRaycast();
        //     ammoContainer.UseAmmo(ammoType);
        // }else
        // {
        //     AudioManager.playSound(AudioSource,weaponSounds.EmptySound);
        //     canAttack = false;
        // }
    
    protected void PlayAttackSound()
    {
        if(attackVFX == null) { Debug.LogWarning("No attack VFX attached"); return; }
        attackVFX.Play();
    }

    protected void ProcessRaycast()
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

    protected void CreateHitImpact(RaycastHit hit)
    {
        if(attackVFX == null) { Debug.LogWarning("No Hit Effect attached"); return; }
        
        GameObject hitUFX = Instantiate(hitEffect,hit.point,Quaternion.LookRotation(hit.normal));
        ParticleSystem hitParticle = hitUFX.transform.GetChild(0).GetComponent<ParticleSystem>();
        Destroy(hitUFX,hitParticle.main.duration);
    }
    
    
}
