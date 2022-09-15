using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum InventoryObjectType
{
    Ammo,
    General
}
[CreateAssetMenu(fileName ="New Item",menuName ="Items/Quick Item")]
public class Inventory_Object : ScriptableObject
{
    public GameObject prefab;
    public InventoryObjectType type;
    public new string name;
    public string description;
    public Sprite art;

    
}
