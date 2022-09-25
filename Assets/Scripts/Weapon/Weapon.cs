using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
[RequireComponent(typeof(AudioSource))]
public class Weapon : MonoBehaviour
{
    public int weaponIndex;
    [SerializeField] ParticleSystem shootVFX;
    [SerializeField] GameObject hitEffect;
    public Sprite weaponIcon;
    public Sprite bulletIcon;
    
    [SerializeField] float range = 100f;
    [SerializeField] float damage;
    [SerializeField] float shootingDelay;
    [SerializeField] bool canConstantShoot = false;
    public float timeToReload;
    public bool CanConstantShoot{get{return canConstantShoot;}}
    private bool constantShooting;
    public bool ConstantShooting{get{return constantShooting;}set{constantShooting = value;}}
    bool canShoot = true;
    [SerializeField] bool emptyAmmo = false;
    public bool EmptyAmmo{set{emptyAmmo = value;}}
    [HideInInspector] public Ammo ammoSlot;
    [SerializeField] AmmoType ammoType;
    public AmmoType AmmoType{get{ return ammoType; }}

    public AudioSource audioSource{get; private set;}
    [Header("Audio")]
    [SerializeField] string reload;
    public string Reload{get{return reload;}}
    [SerializeField] string empty;    
    
    [SerializeField] List<string> shoot;



    void OnEnable() {
        canShoot = true;
        AudioManager.Instance.playSound(audioSource,WeaponSwitcher.Instance.weaponSwitch);

    }
    void OnDisable() {
        
    }
    private void Awake() 
    {
        ammoSlot = GetComponentInParent<Ammo>();
        weaponIndex = transform.GetSiblingIndex();
        constantShooting = canConstantShoot;
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if((Input.GetMouseButton(0) && canShoot) && constantShooting && emptyAmmo == false)
        {
            Shoot();
        }
        else if((Input.GetMouseButtonDown(0) && canShoot) && (!constantShooting || emptyAmmo == true))
        {
            Shoot();

        }
        Debug.DrawRay(transform.position,transform.forward*range);
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
            AudioManager.Instance.playSound(audioSource,shoot[UnityEngine.Random.Range(0,shoot.Count)]);
            PlayMuzzleFlash();
            ProcessRaycast();
            ammoSlot.UseAmmo(ammoType);
            WeaponSwitcher.Instance.weaponEvent.Invoke();
        }else
        {
            AudioManager.Instance.playSound(audioSource,empty);
            emptyAmmo = true;
        }
        yield return new WaitForSeconds(shootingDelay);
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
                EnemyHealth target = hit.transform.gameObject.GetComponent<EnemyHealth>();
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
        AudioManager.Instance.playSound(audioSource,WeaponShootingTypeChanger.Instance.BulletTypeChange);
        constantShooting = !constantShooting;
    }
}
