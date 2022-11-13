using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionAmmo : InteractionContainer
{
    public AmmoPickup ammoPickup;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip pickupSound;
    public override void OnInteractionStart()
    {        
        
        audioSource.PlayOneShot(pickupSound);
        ammoPickup.GetAmmoFromPickup();
        
        gameObject.SetActive(false);

    }
}
