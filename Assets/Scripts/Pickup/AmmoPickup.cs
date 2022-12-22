using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPickup : MonoBehaviour
{
    [SerializeField] AmmoType ammoType;
    [SerializeField] int ammoCount;
    public int AmmoCount{get{return ammoCount;}}
    public AmmoType AmmoType {get{return ammoType;}}

    
    public void GetAmmo()
    {
        WeaponsManager.Instance.ammo.AddAmmo(ammoType,ammoCount);

    }
}
