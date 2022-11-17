using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class PickupController : MonoBehaviour
{
    public static PickupController Instance{get; private set;}
    [SerializeField] GameObject PickupContainer;
    [SerializeField] GameObject[] pickupChildrens;

    private void Awake() {
        if(Instance != this && Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }
    public void AddPickup(string itemCount,string itemName)
    {
        GameObject freeChildren = GetFreeChildren();
        if(freeChildren != null)
        {
            Pickup pickup = freeChildren.GetComponent<Pickup>();
            //pickup.pickupName.text = itemName;
            pickup.pickupDescription.text = "Picked "+itemCount+"x "+itemName;
            freeChildren.SetActive(true);
        }
    }
    public GameObject GetFreeChildren()
    {
        for(int i=0;i<pickupChildrens.Length;i++)
        {
            if(pickupChildrens[i].activeSelf == false)
            {
                return pickupChildrens[i];
            }
        }
        return null;
    }
}
