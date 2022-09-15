using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashLightSystem : MonoBehaviour
{
    [SerializeField] float lightDecay = .1f;
    [SerializeField] float angleDecay = 1f;
    [SerializeField] float minimumAngle = 40f;
    [SerializeField] float decreasingSpeed = 10f;
    bool isDecreasing = false;
    Light Mylight;
    void Start()
    {
        Mylight = GetComponent<Light>();
    }

    // Update is called once per frame
    void Update()
    {
       DecreaseLightIntensity();
       DecreaseLightAngle();

        
    }
    public void RestoreLightAngle(float restoreAngle)
    {
        Mylight.spotAngle = restoreAngle;
    }
    public void RestoreLightIntensity(float intensityAmount)
    {
        Mylight.intensity += intensityAmount;
    }

    void DecreaseLightIntensity()
    {
        if(Mylight.intensity>0)
        {
            Mylight.intensity -= lightDecay * Time.deltaTime *decreasingSpeed;
        }
    }

    void DecreaseLightAngle()
    {
        if(Mylight.spotAngle > minimumAngle)
        {
            Mylight.spotAngle-=angleDecay * Time.deltaTime*decreasingSpeed;
        }
    }
}
