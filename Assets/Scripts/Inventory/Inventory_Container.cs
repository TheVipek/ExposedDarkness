using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "Inventory_Container", menuName = "Inventory_Container")]
public class Inventory_Container : ScriptableObject {

    public List<InventorySlot> slots = new List<InventorySlot>();


    void Awake() {
        
    }


    void itemAdd(Inventory_Object _item,int amount)
    {
        for(int i=0;i<slots.Count;i++)
        {
            if(slots[i].item == _item)
            {
                slots[i].AddAmount(amount);
                return;
            }
        }
        slots.Add(new InventorySlot(_item,amount));
    }
    /*void InitializeEntryBackpack()
    {
        foreach (var item in entryItems)
        {
            itemAdd(item,);
        }
    }*/
       private void OnApplicationQuit() {
        slots.Clear();
    }
}

    [System.Serializable]
    public class InventorySlot
    {
        public Inventory_Object item;
        public int amount;
        public InventorySlot(Inventory_Object _item,int _amount)
        {
            this.item = _item;
            this.amount = _amount;
        }
        public void AddAmount(int value)
        {
            amount+= value;
        }
    }
 


