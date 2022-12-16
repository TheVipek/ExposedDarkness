using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(WeaponPickup))]
public class InteractionWeapon : InteractionContainer
{
    public WeaponPickup weaponPickup;

    public override void OnInteractionStart()
    {
        base.OnInteractionStart();
        PickupController.AddPickup("1",weaponPickup.WeaponName);
        weaponPickup.GetWeapon();
    }
}
