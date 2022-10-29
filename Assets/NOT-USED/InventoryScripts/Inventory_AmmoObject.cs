using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Ammo",menuName ="Items/Ammo")]
public class Inventory_AmmoObject : Inventory_Object
{
    private void Awake() {
        type = InventoryObjectType.Ammo;
    }
    public AmmoType ammoType;

    /*public Inventory_AmmoObject(string _name,string _description,Sprite _art,int _ammoAmount , AmmoType _ammoType)
    {
        this.name = _name;
        this.description = _description;
        this.art = _art;
        this.ammoAmount = _ammoAmount;
        this.ammoType = _ammoType;
    }*/
}
