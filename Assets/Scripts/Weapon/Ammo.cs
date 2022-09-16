using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammo : MonoBehaviour
{
    [Tooltip("Getting reference from inventory to get ammos")]
    [SerializeField] List<AmmoSlot> ammoSlots;


    
    [System.Serializable]
    private class AmmoSlot
    {
        public AmmoType ammoType;
        public int ammoInSlot;
        public int ammoPerSlot;
        public int ammoAmount;

        public void Reload()
        {
            if(ammoInSlot == ammoPerSlot){ return; }
            int needToCharge = ammoPerSlot - ammoInSlot;
            if(ammoAmount >= needToCharge)
            {
                ammoInSlot+=needToCharge;
                ammoAmount-=needToCharge;
            }
            else
            {
                ammoInSlot += ammoAmount;
                ammoAmount = 0;
            }

        }
    }
    public int GetAmmoInSlot(AmmoType ammoType)
    {
        return GetAmmoSlot(ammoType).ammoInSlot;
    }
    public int GetAmmoAmount(AmmoType ammoType)
    {
        return GetAmmoSlot(ammoType).ammoAmount;
    }
    public int GetAmmoPerSlot(AmmoType ammoType)
    {
        return GetAmmoSlot(ammoType).ammoPerSlot;
    }
    public void ReloadAmmo(AmmoType ammoType)
    {
        GetAmmoSlot(ammoType).Reload();
    }
    public void UseAmmo(AmmoType ammoType)
    {
       GetAmmoSlot(ammoType).ammoInSlot--;
    }
    public void PickupAmmo(AmmoType ammoType,int ammoCount)
    {
         GetAmmoSlot(ammoType).ammoAmount+=ammoCount;

    }
    private AmmoSlot GetAmmoSlot(AmmoType ammoType)
    {
        foreach (AmmoSlot slot in ammoSlots)
        {
            if(slot.ammoType == ammoType)
            {
                return slot;
            }
        }
        return null;
    }
}

