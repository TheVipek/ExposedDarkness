using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    public Inventory_Container container;
    public static InventoryController instance;
    
    public delegate void OnAmmoChange();
    public static event OnAmmoChange onAmmoChange;
    private void Awake() {
        if(instance!=this && instance !=null)
        {
            Destroy(this);
        }else
        {
            instance = this;
        }
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
   
    void getAllTypeItems()
    {

    }
}
