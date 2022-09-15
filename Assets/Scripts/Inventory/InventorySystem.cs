using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    public delegate void OnAddToInventory();
    public static event OnAddToInventory onAddToInventory;

    /*Dictionary<string,List<Inventory_Item>> items = new Dictionary<string,List<Inventory_Item>>()
    {
        { "ammos" , new List<Inventory_Item>(){}},
        { "weapons" , new List<Inventory_Item>(){}},
        { "general" , new List<Inventory_Item>(){}},
    };*/
    void Start()
    {
        
    }
    void Update()
    {
        
    }
}
