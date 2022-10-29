using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "New AmmoCharge", menuName = "Items/AmmoCharge")]
public class Inventory_AmmoCharge : Inventory_Object 
{
    private void Awake() {
        type = InventoryObjectType.Ammo;
    }
    public AmmoType ammoType;

    /*public Inventory_AmmoCharge(int _ammoAmount , AmmoType _ammoType)
    {
        this.ammoAmount = _ammoAmount;
        this.ammoType = _ammoType;
    }*/
}

