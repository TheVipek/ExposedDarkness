using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PickupController : MonoBehaviour
{
    public static PickupController Instance{get; private set;}
    [SerializeField] GameObject PickupContainer;
    private List<GameObject> pickupChildrens = new List<GameObject>();

    private void Awake() {
        if(Instance != this && Instance != null) Destroy(this);
        else Instance = this;

        foreach (Transform child in PickupContainer.transform)
        {
            pickupChildrens.Add(child.gameObject);
        }
    }
    public void AddPickup(string itemCount,string itemName)
    {
        GameObject freeChildren = GetFreeChildren();
        if(freeChildren != null)
        {
            Pickup pickup = freeChildren.GetComponent<Pickup>();
            //pickup.pickupName.text = itemName;
            if(int.Parse(itemCount) == 1) 
                pickup.pickupDescription.text = $"Picked {itemName}";
            else
                pickup.pickupDescription.text = $"Picked {itemCount}x {itemName}";
                
            freeChildren.SetActive(true);
        }
    }
    public GameObject GetFreeChildren()
    {
        for(int i=0;i<pickupChildrens.Count;i++)
        {
            if(pickupChildrens[i].activeSelf == false)
            {
                return pickupChildrens[i];
            }
        }
        return null;
    }
}
