using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Weapon",menuName ="Items/Weapon")]
public class Inventory_WeaponObject : Inventory_Object
{
    public int damage;
    public float range;

    public Inventory_WeaponObject(string _name,string _description,Sprite _art,int _damage , float _range)
    {
        this.name = _name;
        this.description = _description;
        this.art = _art;
        this.damage = _damage;
        this.range = _range;
    }
}

