using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPickup : MonoBehaviour
{
    Ammo ammo;
    [SerializeField] AmmoType ammoType;
    [SerializeField] int ammoCount;
    private void Start() {
        ammo = FindObjectOfType<Ammo>();
    }
    private void OnTriggerEnter(Collider other) 
    {
        if(other.gameObject.tag == "Player")
        {
            GetAmmoFromPickup();
            Destroy(gameObject);
        }
    }

    public void GetAmmoFromPickup()
    {
        ammo.PickupAmmo(ammoType,ammoCount);

    }
}
