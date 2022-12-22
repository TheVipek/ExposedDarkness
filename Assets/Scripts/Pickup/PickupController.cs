using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PickupController : MonoBehaviour
{
    [SerializeField] GameObject pickupContainer;
    private static List<GameObject> pickupChildrens = new List<GameObject>();

    private void Awake() {
        if(!pickupContainer) Debug.LogWarning($"Not all objects assigned in {GetType()}");

        Debug.Log($"Before:{pickupChildrens.Count}");
        if(pickupChildrens.Count>0) pickupChildrens.Clear();
        foreach (Transform child in pickupContainer.transform)
        {
            pickupChildrens.Add(child.gameObject);
        }
        Debug.Log($"After:{pickupChildrens.Count}");

    }
    public static void AddPickup(string itemCount,string itemName)
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
    public static GameObject GetFreeChildren()
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
