using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPickup : MonoBehaviour
{
    Ammo ammo;
    [SerializeField] AmmoType ammoType;
    [SerializeField] int ammoCount;
    private void Start() {
        ammo = Ammo.instance;
    }
    private void OnTriggerEnter(Collider other) 
    {
        if(other.gameObject.tag == "Player")
        {
            GetAmmoFromPickup();
            gameObject.SetActive(false);
        }
    }
    
    public void GetAmmoFromPickup()
    {
        ammo.AddAmmo(ammoType,ammoCount);

    }
}
