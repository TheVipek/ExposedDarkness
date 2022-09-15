using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class Weapon : MonoBehaviour
{
    [SerializeField] Camera FPCamera;
    [SerializeField] ParticleSystem shootVFX;
    [SerializeField] GameObject hitEffect;
    public Sprite weaponIcon;
    public Sprite bulletIcon;
    
    [SerializeField] float range = 100f;
    [SerializeField] float damage;
    [SerializeField] float shootingDelay;
    [SerializeField] bool canConstantShoot = false;
    public bool CanConstantShoot{get{return canConstantShoot;}}
    private bool constantShooting;
    public bool ConstantShooting{get{return constantShooting;}set{constantShooting = value;}}
    bool canShoot = true;
    [HideInInspector] public Ammo ammoSlot;
    [SerializeField] AmmoType ammoType;
    public AmmoType AmmoType{get{ return ammoType; }}
    void OnEnable() {
        canShoot = true;
    }
    void OnDisable() {
        
    }
    private void Awake() 
    {
        ammoSlot = GetComponentInParent<Ammo>();
        constantShooting = canConstantShoot;
    }

    void Update()
    {
        if((Input.GetMouseButton(0) && canShoot) && constantShooting)
        {
            Shoot();
        }
        else if((Input.GetMouseButtonDown(0) && canShoot) && !constantShooting)
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
        if(ammoSlot.GetAmmo(ammoType) > 0)
        {
            Debug.Log("Shooting!");
            PlayMuzzleFlash();
            ProcessRaycast();
            ammoSlot.UseAmmo(ammoType);
            WeaponSwitcher.instance.weaponEvent.Invoke();
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
}
