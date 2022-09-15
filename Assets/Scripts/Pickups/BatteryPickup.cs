using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatteryPickup : MonoBehaviour
{
    FlashLightSystem lightToCharge;
    [SerializeField] float intensityCharge =10f;
    [SerializeField] float angleCharge =70f;
    private void Start() {
        lightToCharge = FindObjectOfType<FlashLightSystem>();
    }
    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == "Player")
        {
            lightToCharge.RestoreLightIntensity(intensityCharge);
            lightToCharge.RestoreLightAngle(angleCharge);
            Destroy(gameObject);
        }
    }
}
