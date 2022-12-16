using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AmmoPickup))]
public class InteractionAmmo : InteractionContainer
{
    public AmmoPickup ammoPickup;
    public override void OnInteractionStart()
    {

        base.OnInteractionStart();
        ammoPickup.GetAmmo();
        PickupController.AddPickup(ammoPickup.AmmoCount.ToString(),ammoPickup.AmmoType.ToString());
        StartCoroutine(interactionWaiter(interactionSound.length));
    }
    public override IEnumerator interactionWaiter(float delay)
    {

        yield return StartCoroutine(base.interactionWaiter(delay));
        gameObject.SetActive(false);
    }
    

}
