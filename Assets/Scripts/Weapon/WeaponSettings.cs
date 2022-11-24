using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "WeaponSettings", menuName = "ZombieFPS/WeaponSettings", order = 0)]
public class WeaponSettings : ScriptableObject {

    //DEFAULT VALUES
    public float range;
    public float damage;
    public float attackDelay;
    public WeaponType weaponType;
    [Tooltip("Which means ; does player need to click every time to attack")]
    [SerializeField] bool canConstantAttack;
    public bool CanConstantAttack{get{return canConstantAttack;}}
    public bool isConstantAttacking;
    public bool canAttack = true;
    [SerializeField] AmmoType ammoType;
    public AmmoType AmmoType{get{ return ammoType; }}
}



