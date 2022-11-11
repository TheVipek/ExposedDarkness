using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionAmmo : InteractionContainer
{
    public AmmoPickup ammoPickup;
    public override void OnInteractionStart()
    {        
        ammoPickup.GetAmmoFromPickup();
        
        gameObject.SetActive(false);

    }
}
