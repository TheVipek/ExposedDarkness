using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(AudioSource),typeof(WeaponAnimation))]
public abstract class Weapon : MonoBehaviour,IPrimaryAction
{  
    [Header("Weapon settings")]
    [SerializeField] protected KeyCode primaryActionKey = KeyCode.Mouse0;
    public KeyCode PrimaryActionKey { get{return primaryActionKey;}}
    

    [SerializeField] float range = 100f;
    [SerializeField] protected float primaryAttackDamage;
    [SerializeField] protected float primaryAttackDelay;

    [SerializeField] WeaponType weaponType;
    [SerializeField] protected WeaponAnimation weaponAnimation;
    public WeaponType WeaponType{get{ return weaponType;}}
    
    protected float nextPrimaryAttackTime;
    
    [Header("WeaponRole")]
    [SerializeField] WeaponRole weaponRole;
    public WeaponRole WeaponRole {get{return weaponRole;}}

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
   
    
    [Header("Weapon VFX/Sprites/Needed references")]
    [SerializeField] ParticleSystem attackVFX;
    [SerializeField] GameObject hitEffect;
    [SerializeField] Sprite weaponIcon;
    public Sprite WeaponIcon{ get {return weaponIcon;}}


    private void OnEnable() {
        CanAttack = true;
    }
    private void Awake() 
    {
        isConstantAttacking = canConstantAttack;
        weaponAnimation = GetComponent<WeaponAnimation>();
        audioSource = GetComponent<AudioSource>();
        
    }


    protected virtual void Update()
    {

        if(canAttack)
        {
            
            if(Input.GetKey(primaryActionKey) && Time.time > nextPrimaryAttackTime && isConstantAttacking == true)
            {
                PrimaryAction();
                nextPrimaryAttackTime = Time.time + primaryAttackDelay;
            }
            else if(Input.GetKeyDown(primaryActionKey) && Time.time > nextPrimaryAttackTime && isConstantAttacking == false)
            {
                PrimaryAction();
                nextPrimaryAttackTime = Time.time + primaryAttackDelay;  
            }
        }
    }

    public abstract void PrimaryAction();
    
    protected void PlayAttackSound()
    {
        if(attackVFX == null) { Debug.LogWarning("No attack VFX attached"); return; }
        attackVFX.Play();
    }

    protected void ProcessRaycast(float hitDamage)
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
                target.TakeDamage(hitDamage);
            }
            

        }
        else
        {
            return;
        }
    }

    protected virtual void CreateHitImpact(RaycastHit hit)
    {
        if(attackVFX == null) { Debug.LogWarning("No Hit Effect attached"); return; }
        
        GameObject hitUFX = Instantiate(hitEffect,hit.point,Quaternion.LookRotation(hit.normal));
        ParticleSystem hitParticle = hitUFX.transform.GetChild(0).GetComponent<ParticleSystem>();
        Destroy(hitUFX,hitParticle.main.duration);
    }
    
    public IEnumerator DisableCanAttack(float disabledTime)
    {
        canAttack = false;
        yield return new WaitForSecondsRealtime(disabledTime);
        canAttack = true;
    }
}
