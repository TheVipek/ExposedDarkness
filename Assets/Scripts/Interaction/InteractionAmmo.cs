using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionAmmo : InteractionContainer
{
    public AmmoPickup ammoPickup;
    public override void OnInteractionStart()
    {        
        base.OnInteractionStart();
        Debug.Log("OnInteractionDerived");
        Debug.Log(ammoPickup);
        ammoPickup.GetAmmoFromPickup();
        StartCoroutine(interactionWaiter(interactionSound.length));
    }
    public override IEnumerator interactionWaiter(float delay)
    {
        yield return StartCoroutine(base.interactionWaiter(delay));
        gameObject.SetActive(false);
    }

}
